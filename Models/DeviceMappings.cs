using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace web_api_icc_valsys_no_mvc.Models
{
    public class DeviceMappings
    {
        public string SerialNumber { get; set; }

        public int TechnicalProductId { get; set; }

        public int ModelId { get; set; }

        public bool IsValidForShipping { get; set; }
    }
}