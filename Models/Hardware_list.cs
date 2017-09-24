using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace web_api_icc_valsys_no_mvc.Models
{
    public class Hardware_list
    {
        public string serial_number { get; set; }
        public string description { get; set; }
        public int hardware_model_id { get; set; }
    }
}