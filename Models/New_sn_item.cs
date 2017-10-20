using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace web_api_icc_valsys_no_mvc.Models
{
    public class New_sn_item
    {
        public string username_ad { get; set; }
        public string password_ad { get; set; }

        public int stock_receive_Detail_id { get; set; }
        public int stock_handler_id { get; set; }
        public int model_id { get; set; }

        public string serial_number { get; set; }

    }
}