using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using PayMedia.ApplicationServices.ClientProxy;
using PayMedia.ApplicationServices.SharedContracts;
using PayMedia.ApplicationServices.Devices.ServiceContracts;
using PayMedia.ApplicationServices.Customers.ServiceContracts;
using PayMedia.ApplicationServices.Finance.ServiceContracts;
using PayMedia.ApplicationServices.Finance.ServiceContracts.DataContracts;
using PayMedia.ApplicationServices.Users;
using PayMedia.ApplicationServices.Customers.ServiceContracts.DataContracts;
using System.Collections;
using PayMedia.ApplicationServices.Authentication.ServiceContracts;
using System.Net.Http.Formatting;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using web_api_icc_valsys_no_mvc.Models;
using System.IO;
using System.Diagnostics;
using Newtonsoft.Json;
using System.Xml;
using System.Collections.Specialized;
using PayMedia.ApplicationServices.Contacts.ServiceContracts;
using PayMedia.ApplicationServices.Contacts.ServiceContracts.DataContracts;
using PayMedia.ApplicationServices.Authentication;
using PayMedia.ApplicationServices.AgreementManagement.ServiceContracts;
using PayMedia.ApplicationServices.AgreementManagement.ServiceContracts.DataContracts;
using PayMedia.ApplicationServices.ProductCatalog.ServiceContracts;
using PayMedia.ApplicationServices.ProductCatalog.ServiceContracts.DataContracts;
using PayMedia.ApplicationServices.SandBoxManager.ServiceContracts;
using PayMedia.ApplicationServices.Factory;
using PayMedia.ApplicationServices.Pricing.ServiceContracts;
using PayMedia.ApplicationServices.Pricing.ServiceContracts.DataContracts;
using PayMedia.ApplicationServices.Users.ServiceContracts;
using PayMedia.ApplicationServices.InvoiceRun.ServiceContracts;
using PayMedia.ApplicationServices.InvoiceRun.ServiceContracts.DataContracts;


namespace web_api_icc_valsys_no_mvc.Models
{
    public class ICC_customerResponse
    {
        public int the_cust_id { get; set; }
        public ICC_customer Club { get; set; }
    }
}