﻿using AutoMapper;
using Final.Project.Core.Shared;
using Final.Project.Domain.Entities;
using Final.Project.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Final.Project.Core.ProductServices
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public ProductService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.mapper = mapper;
            this.unitOfWork = unitOfWork;
        }

        public async Task<ApplicationResult<ProductDto>> Create(CreateProductInput input, User user)
        {
            try
            {
                // Validate input
                var validator = new CreateProductValidator();
                var validationResult = validator.Validate(input);
                if (!validationResult.IsValid)
                {
                    return new ApplicationResult<ProductDto> { Succeeded = false, ErrorMessage = validationResult.ToString() };
                }

                Product product = mapper.Map<Product>(input);
                product.UserId = user.Id;
                product.IsSold = false;
                await unitOfWork.Product.Add(product);
                unitOfWork.Complete();
                ApplicationResult<ProductDto> result = new ApplicationResult<ProductDto>
                {
                    Result = mapper.Map<ProductDto>(product),
                    Succeeded = true
                };

                return result;
            }
            catch (Exception ex)
            {
                // log ex
                return new ApplicationResult<ProductDto>
                {
                    Succeeded = false,
                    ErrorMessage = "Error Occured!"
                };
            }
        }

        public async Task<ApplicationResult> Delete(int id, User user)
        {
            try
            {
                var willDelete = unitOfWork.Product.Where(x => x.Id == id && x.UserId == user.Id).FirstOrDefault();
                if (willDelete != null)
                {
                    await unitOfWork.Product.Delete(id);
                    unitOfWork.Complete();
                    return new ApplicationResult { Succeeded = true };
                }
                return new ApplicationResult { Succeeded = false, ErrorMessage = "Record cannot be found!" };
            }
            catch (Exception ex)
            {
                return new ApplicationResult { Succeeded = false, ErrorMessage = "Error occured!" };
            }
        }

        public async Task<ApplicationResult<ProductDto>> Get(int id, User user)
        {
            try
            {
                var product = await unitOfWork.Product.GetById(id);
                var dto = mapper.Map<ProductDto>(product);
                
                return new ApplicationResult<ProductDto>
                {
                    Result = dto,
                    Succeeded = true
                };

            }
            catch (Exception ex)
            {
                return new ApplicationResult<ProductDto>
                {
                    Succeeded = false,
                    ErrorMessage = "Error Occured!"
                };
            }
        }

        public async Task<ApplicationResult<List<ProductDto>>> GetAll(User user)
        {
            try
            {
                var list = await unitOfWork.Product.GetAll();
                var dtoList = new List<ProductDto>();
                foreach(var product in list)
                {
                    dtoList.Add(mapper.Map<ProductDto>(product));
                }

                return new ApplicationResult<List<ProductDto>>
                {
                    Result = dtoList,
                    Succeeded = true
                };

            }
            catch (Exception ex)
            {
                return new ApplicationResult<List<ProductDto>>
                {
                    Succeeded = false,
                    ErrorMessage = "Error Occured!"
                };
            }
        }

        public async Task<ApplicationResult<ProductDto>> Update(UpdateProductInput input, User user)
        {
            try
            {
                var existingProduct = await unitOfWork.Product.GetById(input.Id);
                if(existingProduct == null)
                {
                    return new ApplicationResult<ProductDto> { Succeeded = false, ErrorMessage = "Product not found!" };
                }

                if(existingProduct.UserId != user.Id)
                {
                    return new ApplicationResult<ProductDto> { Succeeded = false, ErrorMessage = "You cannot modify this product!" };
                }

                var product = mapper.Map<Product>(input);
                await unitOfWork.Product.Update(product);
                unitOfWork.Complete();
                return new ApplicationResult<ProductDto> { Succeeded = true, Result = mapper.Map<ProductDto>(product) };
            }
            catch (Exception ex)
            {
                return new ApplicationResult<ProductDto>
                {
                    Succeeded = false,
                    ErrorMessage = "Error Occured!"
                };
            }
        }
    }
}
