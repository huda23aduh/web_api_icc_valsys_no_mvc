using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace web_api_icc_valsys_no_mvc.Models
{
    public class NewLinkParams
    {
        public String username_ad { get; set; }
        public String password_ad { get; set; }

        public int cust_id { get; set; }
        public int so_id { get; set; }

        public int is_inet { get; set; }
        public int is_HD { get; set; }

        //public int the_total_format_tech_prod_id { get; set; }
        public List<int> the_format_tech_prod_id_list { get; set; }

        //public int the_total_format_serial_number { get; set; }
        public List<string> the_serial_number_list { get; set; }
    }
}