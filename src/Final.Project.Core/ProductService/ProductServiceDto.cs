﻿using Final.Project.Core.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Final.Project.Core.ProductServices
{
    public class ProductDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public int ColorId { get; set; }
        public int BrandId { get; set; }
        public int UsingStatusId { get; set; }
        public string Image { get; set; }
        public decimal Price { get; set; }
        public bool IsOfferable { get; set; }
        public bool IsSold { get; set; }
    }
    public class CreateProductInput
    {
        //[Required]
        //[StringLength(100, ErrorMessage = "{0} field max {1} chars")]
        public string ProductName { get; set; }
        //[Required]
        //[StringLength(500, ErrorMessage = "{0} field max {1} chars")]
        public string Description { get; set; }
        //[Required]
        public int CategoryId { get; set; }
        public int ColorId { get; set; }
        public int BrandId { get; set; }
        //[Required]
        public int UsingStatusId { get; set; }
        //[Required]
        public string Image { get; set; }
        //[Required]
        public decimal Price { get; set; }
        public bool IsOfferable { get; set; }

    }
    public class UpdateProductInput : CreateProductInput
    {
        //[Required]
        public int Id { get; set; }
        public bool IsSold { get; set; }
    }
}
