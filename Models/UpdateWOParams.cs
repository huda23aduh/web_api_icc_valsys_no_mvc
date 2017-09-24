using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace web_api_icc_valsys_no_mvc.Models
{
    public class UpdateWOParams
    {
        public String username_ad { get; set; }
        public String password_ad { get; set; }

        public int workOrderId { get; set; }
        public int customerId { get; set; }
        public int workOrderStatusId { get; set; }
        public int servProvId { get; set; }
        public int servTypeId { get; set; }
        public String servDateTime { get; set; }
        public int servAddressId { get; set; }
        public String ProbDescription { get; set; }
        public int servId { get; set; }
        public int TitleId { get; set; }
        public String FirstName { get; set; }
        public String MiddleName { get; set; }
    }
}