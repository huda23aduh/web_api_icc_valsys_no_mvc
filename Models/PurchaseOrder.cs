using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace web_api_icc_valsys_no_mvc.Models
{
    public class PurchaseOrder
    {
        public List<CartItem> CartItems { get; set; }
        public string DeliveryAddress { get; set; }
        public int TotalPrice { get; set; }
        public int User_Id { get; set; }
    }
}