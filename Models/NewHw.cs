using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace web_api_icc_valsys_no_mvc.Models
{
    public class NewHw
    {
        public String username_ad { get; set; }
        public String password_ad { get; set; }

        public int customer_id { get; set; }
        public int agreement_id { get; set; }
        public int fa_id { get; set; }

        public int the_total_com_prod { get; set; }
        public List<int> the_list_com_prod_id { get; set; }

        public int the_total_promo { get; set; }
        public List<int> the_offers { get; set; }

        public int the_total_segmentation { get; set; }
        public List<int> the_segmentation_list { get; set; }

        public int the_total_finance_option_id { get; set; }
        public List<int> the_finance_option_id_list { get; set; }
    }
}