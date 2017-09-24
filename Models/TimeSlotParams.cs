using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace web_api_icc_valsys_no_mvc.Models
{
    public class TimeSlotParams
    {
        public String username_ad { get; set; }
        public String password_ad { get; set; }

        public String Date { get; set; }
        public String PostalCode { get; set; }
        public int customer_id { get; set; }
        public int serv_type_id { get; set; }
        public int serv_prov_id { get; set; }
        public int serv_addr_id { get; set; }
        public int validAddressIdCust { get; set; }
    }
}