using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace web_api_icc_valsys_no_mvc.Models
{
    public class DocumentRequest
    {
        public String username_ad { get; set; }
        public String password_ad { get; set; }

        public string file_name { get; set; }
        public string file_uri { get; set; }
        public long file_size { get; set; }
        public int cust_id { get; set; }
    }
}