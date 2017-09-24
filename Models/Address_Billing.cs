using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace web_api_icc_valsys_no_mvc.Models
{
    public class Address_Billing
    {
        public int CountryId { get; set; }
        public int ProvinceId { get; set; }
        public String PostalCode { get; set; }
        public String BigCity_billing { get; set; }
        public String SmallCity_billing { get; set; }
        public String Street_billing { get; set; }
        public String Directions_Addr_billing { get; set; }
        public String Address_line1_Addr_billing { get; set; }
        public String Address_line2_Addr_billing { get; set; }
        public String MobilePhone_Addr_billing { get; set; }
        public String WorkPhone_Addr_billing { get; set; }
        public String EmergencyPhone_Addr_billing { get; set; }
        public String Landmark_Addr_billing { get; set; }
        public String HomePhone_Addr_billing { get; set; }
        public String Email_Addr_billing { get; set; }
    }
}