﻿using System;
using System.Collections.Generic;

namespace CIM.API.Models
{
    public partial class ProductDetail
    {
        public string ProductId { get; set; }
        public string ProductDescription { get; set; }
        public string ProductFamily { get; set; }
        public string ProductGroup { get; set; }
        public string ProductType { get; set; }
        public string Speed { get; set; }
    }
}
