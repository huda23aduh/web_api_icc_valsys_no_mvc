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
    public class YangProductPriceItems
    {
        public String the_Name { get; set; }
        public List<int> the_CommercialProductIds { get; set; }
        public List<ListPriceCondition> the_ListPriceConditions { get; set; }
        public List<ListPrice> the_ListPriceCollection { get; set; }
        public List<int> the_AgreementTypeIds { get; set; }
        public List<int> the_BusinessUnitAttributeValues { get; set; }
        public int the_ChargePeriodId { get; set; }
        public List<int> the_CountryIds { get; set; }
        public int the_CurrencyId { get; set; }
        public List<int> the_CustomerClassIds { get; set; }
        public List<int> the_CustomerTypeIds { get; set; }
        public List<int> the_FinanceOptionIds { get; set; }
        public List<int> the_FinancialAccountTypeIds { get; set; }
        public List<String> the_PostalCodes { get; set; }
        public List<int> the_ProvinceIds { get; set; }
        public String the_LedgerAccountDesc { get; set; }
        List<ListPrice> the_itemListPrice { get; set; }


    }
}