using PayMedia.ApplicationServices.ClientProxy;
using PayMedia.ApplicationServices.Customers.ServiceContracts;
using PayMedia.ApplicationServices.Finance.ServiceContracts;
using PayMedia.ApplicationServices.Finance.ServiceContracts.DataContracts;
using PayMedia.ApplicationServices.SharedContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using web_api_icc_valsys_no_mvc.Models;

namespace web_api_icc_valsys_no_mvc.Controllers
{
    public class ManualChargeOrCreditController : ApiController
    {
        [HttpGet]
        [Route("api/{username_ad}/{password_ad}/manualchargeordeposit/addManualChargeOrCredit/{customer_id}/{ledger_account_id}/{deposite_amount}")]
        public HttpResponseMessage addManualChargeOrCredit(String username_ad, String password_ad, int customer_id, int ledger_account_id, int deposite_amount)
        {
            Authentication_class var_auth = new Authentication_class();
            AuthenticationHeader authHeader = var_auth.getAuthHeader(username_ad, password_ad);
            AsmRepository.SetServiceLocationUrl(var_auth.var_service_location_url);

            var customersService = AsmRepository.GetServiceProxyCachedOrDefault<ICustomersService>(authHeader);
            var financeService = AsmRepository.GetServiceProxyCachedOrDefault<IFinanceService>(authHeader);
            var financeConfigurationService = AsmRepository.GetServiceProxyCachedOrDefault<IFinanceConfigurationService>(authHeader);

            var fa = financeService.GetFinancialAccountsForCustomer(customer_id, new CriteriaCollection() {
                new Criteria() {
                    Key="TypeId",
                    Operator=Operator.Equal,
                    Value="1"     // 1 means quote billing financial account
                    }
                }, 0);

            if (fa.TotalCount == 0)
            {
                //Console.WriteLine("There are no finanical account on customer with id : " + deposite_amount);
                return null;
            }
            CreateManualAdjustmentFTsResult the_result = financeService.CreateManualAdjustmentFTs(new CreateManualAdjustmentFTsRequest()
            {
                FinancialTransactions = new FinancialTransactionCollection()
                    {
                        new FinancialTransaction()
                        {
                            BaseAmount = deposite_amount,
                            Comment = "",
                            CurrencyId = 1,
                            CreatedByEvent = 500,
                            LedgerAccountId = ledger_account_id,
                            FinancialAccountId = fa.Items[0].Id.Value,
                            PeriodFrom = DateTime.Now,
                            PeriodTo = DateTime.Now
                        }
                    }
            });

            if (the_result != null )
            {
                return Request.CreateResponse(HttpStatusCode.OK, the_result);
            }
            else
            {
                var message = string.Format("error");
                HttpError err = new HttpError(message);
                return Request.CreateResponse(HttpStatusCode.OK, message);
            }
            
            
        }
    }
}
