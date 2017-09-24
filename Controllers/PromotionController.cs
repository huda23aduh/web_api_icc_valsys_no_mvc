using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using PayMedia.ApplicationServices.ClientProxy;
using PayMedia.ApplicationServices.SharedContracts;
using PayMedia.ApplicationServices.ProductCatalog.ServiceContracts;
using PayMedia.ApplicationServices.OfferManagement.ServiceContracts;
using PayMedia.ApplicationServices.ProductCatalog.ServiceContracts.DataContracts;
using PayMedia.ApplicationServices.OfferManagement.ServiceContracts.DataContracts;
using web_api_icc_valsys_no_mvc.Models;
using PayMedia.ApplicationServices.AgreementManagement.ServiceContracts;
using PayMedia.ApplicationServices.Customers.ServiceContracts;
using PayMedia.ApplicationServices.PriceAdjustment.ServiceContracts;

namespace web_api_icc_valsys_no_mvc.Controllers
{
    public class PromotionController : ApiController
    {
        

        [HttpGet]
        [Route("api/{username_ad}/{password_ad}/promotion/GetPromoNow")]
        public OfferDefinitionCollection GetPromoNow(String username_ad, String password_ad)
        {
            Authentication_class var_auth = new Authentication_class();
            AuthenticationHeader authHeader = var_auth.getAuthHeader(username_ad, password_ad);
            AsmRepository.SetServiceLocationUrl(var_auth.var_service_location_url);

            IProductCatalogConfigurationService prodService = AsmRepository.AllServices.GetProductCatalogConfigurationService(authHeader);

            IOfferManagementConfigurationService offService = AsmRepository.AllServices.GetOfferManagementConfigurationService(authHeader);
            OfferDefinitionCollection offers =  offService.GetOfferDefinitions(new BaseQueryRequest()
            {
                FilterCriteria = new CriteriaCollection()
                {
                    new Criteria()
                    {
                        Key = "Active",
                        Operator = Operator.Equal,
                        Value = "1"
                    }
                }
            });



            return offers;
        }

        [HttpGet]
        [ActionName("Get8params")]
        public String Get8params(int id1, int id2, int id3, int id4, int id5, int id6, int id7, int id8)
        {
            int aa = id1 + id2 + id3 + id4 + id5 + id6 + id7 + id8;

            var b = aa.ToString();


            return b;
        }

        [HttpGet]
        [Route("api/{username_ad}/{password_ad}/promotion/YangOfferDefinition")]
        public List<OfferDefinition> YangOfferDefinition(String username_ad, String password_ad)
        {
            Authentication_class var_auth = new Authentication_class();
            AuthenticationHeader authHeader = var_auth.getAuthHeader(username_ad, password_ad);
            AsmRepository.SetServiceLocationUrl(var_auth.var_service_location_url);

            IOfferManagementConfigurationService offercService = null;
            IOfferManagementService offerService = null;
            IPriceAdjustmentService priceadService = null;
            IPriceAdjustmentConfigurationService priceadcService = null;
            ICustomersConfigurationService custcService = null;
            IProductCatalogConfigurationService productcService = null;
            IAgreementManagementService agService = null;



            offercService = AsmRepository.AllServices.GetOfferManagementConfigurationService(authHeader);
            offerService = AsmRepository.AllServices.GetOfferManagementService(authHeader);
            priceadService = AsmRepository.AllServices.GetPriceAdjustmentService(authHeader);
            priceadcService = AsmRepository.AllServices.GetPriceAdjustmentConfigurationService(authHeader);
            custcService = AsmRepository.AllServices.GetCustomersConfigurationService(authHeader);
            productcService = AsmRepository.AllServices.GetProductCatalogConfigurationService(authHeader);
            agService = AsmRepository.AllServices.GetAgreementManagementService(authHeader);

            try
            {
                var offers = offercService.GetOfferDefinitions(new BaseQueryRequest()
                {
                    FilterCriteria = Op.Eq("Active", true),
                    DeepLoad = true,
                    PageCriteria = new PageCriteria()
                    {
                        Page = 0,
                    }
                });

                //Console.WriteLine("Find " + offers.TotalCount + " Promo defenitions...");

                return offers.Items;

                //foreach (var offer in offers.Items)
                //{
                //    Console.WriteLine("-----------------------------------------------------------------------");
                //    Console.WriteLine("Promo Name : " + offer.Description);

                //    var offer_adjustments = priceadcService.GetPriceAdjustmentDefinitionsForOffer(offer.Id.Value, 0);

                //    Console.WriteLine(" Find total price adjustment : " + offer_adjustments.TotalCount);

                //    foreach (var offer_adjust in offer_adjustments.Items)
                //    {

                //        string condition = "";
                //        //string apply_level = "";
                //        string price = "";
                //        string charge_type = "";

                //        string apply_to_customer_class = "";
                //        string apply_to_customer_type = "";
                //        string apply_to_product = "";



                //        foreach (var cust_class in offer_adjust.ApplicableCustomerClasses)
                //        {
                //            apply_to_customer_class += custcService.GetCustomerClass(cust_class).Description + ",";
                //        }

                //        foreach (var cust_type in offer_adjust.ApplicableCustomerTypes)
                //        {
                //            apply_to_customer_type += custcService.GetCustomerType(cust_type).LongDescription + ",";
                //        }

                //        if (offer_adjust.ApplicableProducts != 0)
                //        {
                //            apply_to_product = productcService.GetCommercialProduct(offer_adjust.ApplicableProducts.Value).Name;
                //        }

                //        if (apply_to_customer_class != "")
                //        {
                //            condition += "  apply to customer class : " + apply_to_customer_class;
                //        }

                //        if (apply_to_customer_type != "")
                //        {
                //            condition += "  apply to customer type : " + apply_to_customer_type;
                //        }

                //        if (apply_to_product != "")
                //        {
                //            condition += "  apply to product : " + apply_to_product;
                //        }

                //        if (offer_adjust.Type == PriceAdjustmentTypes.PercentOff)
                //        {
                //            price = " discount is percent :" + offer_adjust.Value;
                //        }

                //        if (offer_adjust.Type == PriceAdjustmentTypes.FixedPrice)
                //        {
                //            price = " discount is fixed price : " + offer_adjust.Value;
                //        }

                //        if (offer_adjust.Type == PriceAdjustmentTypes.AmountOff)
                //        {
                //            price = " discount is off amount " + offer_adjust.Value;
                //        }

                //        if (offer_adjust.ChargeType == ChargeTypes.OnceOff)
                //        {
                //            charge_type = " charge type is  Once-Off";
                //        }
                //        else if (offer_adjust.ChargeType == ChargeTypes.Recurring)
                //        {
                //            charge_type = " charge type is Recurring";
                //        }

                //        Console.WriteLine("Apply level :  0 - ListPrice, 1 - Settlement 2 - OrderableEventDiscount 3 - PrepaidFAPayment 4 - QuoteProduct");
                //        Console.WriteLine("Adjustment " + offer_adjust.Description + " apply level : " + offer_adjust.ApplyToType.ToString() + "   |   " + condition + " Price : " + price + " " + charge_type);



                //    }
                //    Console.WriteLine("########################################################################");
                //}

                //Console.WriteLine("End");

            }
            catch (Exception ex)
            {
                return null;
                //Console.WriteLine("Errors : " + ex.Message);
                //Console.WriteLine("Exception Stack : " + ex.StackTrace);
            }
            
        }


    }
}
