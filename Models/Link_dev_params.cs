using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace web_api_icc_valsys_no_mvc.Models
{
    public class Link_dev_params
    {
        public String username_ad { get; set; }
        public String password_ad { get; set; }

        public List<Link_hw> item_hw { get; set; }
        public List<Link_sw> item_sw { get; set; }
        public int cust_id { get; set; }
    }
}