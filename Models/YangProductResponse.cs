using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using PayMedia.ApplicationServices.ClientProxy;
using PayMedia.ApplicationServices.SharedContracts;
using PayMedia.ApplicationServices.AgreementManagement.ServiceContracts;
using PayMedia.ApplicationServices.ProductCatalog.ServiceContracts;
using PayMedia.ApplicationServices.Customers.ServiceContracts;
using PayMedia.ApplicationServices.Finance.ServiceContracts;
using PayMedia.ApplicationServices.ProductCatalog.ServiceContracts.DataContracts;

namespace web_api_icc_valsys_no_mvc.Models
{
    public class YangProductResponse
    {
        public String ProductName;
        public String PriceType;
        public String ChangePeriod;
        public String PriceAmount;
        public List<CommercialProductCollection> The_Com_Prod_Col;
        public List<ListPrice> The_ListPrice;
        public List<ListPriceCondition> The_ListPriceCondition;
        public List<int> AgreementTypeIds;
        public List<int> BusinessUnitAttributeValues;
        public int ChargePeriodId;
        public List<int> CountryIds;
        public String currency;
        public String customer_class;
        public List<int> CustomerTypeIds;
        public List<int> FinanceOptionIds;
        public List<int> FinancialAccountTypeIds;
        public List<String> PostalCodes;
        public List<int> ProvinceIds;


    }
}