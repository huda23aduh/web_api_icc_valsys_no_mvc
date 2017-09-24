using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using PayMedia.ApplicationServices.ClientProxy;
using PayMedia.ApplicationServices.SharedContracts;
using PayMedia.ApplicationServices.ProductCatalog.ServiceContracts;
using PayMedia.ApplicationServices.OfferManagement.ServiceContracts;
using PayMedia.ApplicationServices.ProductCatalog.ServiceContracts.DataContracts;
using PayMedia.ApplicationServices.Customers.ServiceContracts;
using PayMedia.ApplicationServices.Finance.ServiceContracts;
using PayMedia.ApplicationServices.AgreementManagement.ServiceContracts;

namespace web_api_icc_valsys_no_mvc.Models
{
    public class YangProductPriceResponse
    {
        public int the_TotalCount { get; set; }
        public List<YangProductPriceItems> the_items { get; set; }
    }
}