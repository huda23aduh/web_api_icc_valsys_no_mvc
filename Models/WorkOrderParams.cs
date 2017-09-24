using PayMedia.ApplicationServices.Workforce.ServiceContracts.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace web_api_icc_valsys_no_mvc.Models
{
    public class WorkOrderParams
    {
        public string username_ad { get; set; }
        public string password_ad { get; set; }

        public int agreementId { get; set; }
        public int customerId { get; set; }
        public int FinancialAccountId { get; set; }
        public int servProvId { get; set; }
        public int servTypeId { get; set; }
        public string servDateTime { get; set; }
        public int servAddressId { get; set; }
        public string ProbDescription { get; set; }
        public int servId { get; set; }
        public int TitleId { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string postal_code { get; set; }

        public int SERV_PROV_SERV_ID { get; set; }

        public int total_services { get; set; }
        public WorkOrderServiceCollection the_services { get; set; }

    }
}