using PayMedia.ApplicationServices.AgreementManagement.ServiceContracts;
using PayMedia.ApplicationServices.ClientProxy;
using PayMedia.ApplicationServices.Pricing.ServiceContracts.DataContracts;
using PayMedia.ApplicationServices.SharedContracts;
using PayMedia.ApplicationServices.ViewFacade.ServiceContracts.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using web_api_icc_valsys_no_mvc.Models;

namespace web_api_icc_valsys_no_mvc.Controllers
{
    public class PriceController : ApiController
    {
        
        [HttpGet]
        [Route("api/{username_ad}/{password_ad}/price/YangOverwritePrice/{productId}/{amount}/{dt}")]
        public int GetAgreementByTypeId(String username_ad, String password_ad, int productId, int amount, String dt)
        {
            Authentication_class var_auth = new Authentication_class();
            AuthenticationHeader ah = var_auth.getAuthHeader(username_ad, password_ad);

            AsmRepository.SetServiceLocationUrl(var_auth.var_service_location_url);
            var priceServce = AsmRepository.AllServices.GetPricingService(ah);
            var agService = AsmRepository.GetServiceProxyCachedOrDefault<IAgreementManagementService>(ah);
            int listprice_condition_id = 0;
            var listprice_conditions = agService.GetAgreementDetailWithPricing(productId);

            DateTime myDate = DateTime.ParseExact(dt, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);


            if (listprice_conditions.PricingInfoCollection.Items.Count > 0)
            {
                listprice_condition_id = listprice_conditions.PricingInfoCollection.Items[0].ListPriceConditionId.Value;
                return listprice_condition_id;
            }
            else
            {
                //Console.WriteLine("There are no pirce found on this product, please check your configuraitons! agreement detail id : " + agreement_detail_id);
                return 0;
            }

            var priceOverride = priceServce.GetPriceOverridesByListPriceConditionId(listprice_condition_id, (int)Entities.AgreementDetail, productId, 0);

            if (priceOverride.Items.Count == 0)
            {
                priceServce.CreatePriceOverride(new PriceOverride()
                {
                    Amount = amount,
                    CurrencyId = 1,
                    ListPriceConditionId = listprice_condition_id,
                    EntityId = productId,
                    EntityType = (int)Entities.AgreementDetail,
                    OverrideType = PayMedia.ApplicationServices.Pricing.ServiceContracts.OverrideTypes.FixedPrice,
                    Recurring = true,
                    //OverrideInvoiceText = "New Price from 2017-5-16"
                }, 0);

            }
            else
            {
                priceOverride.Items[0].Amount = amount;

                if (myDate < DateTime.Now)
                {
                    priceServce.UpdatePriceOverride(priceOverride.Items[0], 0);
                }
                else
                {
                    priceServce.ScheduleUpdatePriceOverride(priceOverride.Items[0], myDate, 0);
                }
            }

            
        }
    }
}
