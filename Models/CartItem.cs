﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace web_api_icc_valsys_no_mvc.Models
{
    public class CartItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public int Price { get; set; }
    }
}