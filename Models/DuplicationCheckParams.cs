using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace web_api_icc_valsys_no_mvc.Models
{
    public class DuplicationCheckParams
    {
        public string username_ad { get; set; }
        public string password_ad { get; set; }

        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string Lastname { get; set; }
        public string MobilePhone { get; set; }
        public string HomePhone { get; set; }
        public string WorkPhone { get; set; }
        public string EmergencyPhone { get; set; }
        public string IdentityId { get; set; }
        public string PLNId { get; set; }
        public string Address { get; set; }
        public string ZipCode { get; set; }
    }
}