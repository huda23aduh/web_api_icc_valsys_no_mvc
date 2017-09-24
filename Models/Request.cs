using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace web_api_icc_valsys_no_mvc.Models
{
    public class Request
    {
        public String username_ad { get; set; }
        public String password_ad { get; set; }

        public int CustId { get; set; }
        public int direct_close { get; set; }
        public string Firstname { get; set; }
        public string Middlename { get; set; }
        public string Surname { get; set; }
        public string Lastname { get; set; }
        public string HomePhone { get; set; }
        public string WorkPhone { get; set; }
        public string MobilePhone { get; set; }
        public string PlnId { get; set; }
        public string IdentityId { get; set; }
        public string EcPhone { get; set; }
        public string Province { get; set; }
        public string City { get; set; }
        public string District { get; set; }
        public string Village { get; set; }
        public string PostalCode { get; set; }
        public string ProblemDescription { get; set; }
        public int CategoryKey { get; set; }
    }
}