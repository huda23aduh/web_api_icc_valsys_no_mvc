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
using System.Security.Cryptography;
using System.Web.Http;
using web_api_icc_valsys_no_mvc.Models;

namespace web_api_icc_valsys_no_mvc.Controllers
{
    public class PaymentController : ApiController
    {
        [HttpGet]
        [Route("api/{username_ad}/{password_ad}/payment/doPaymentWithFulfillQuote/{customer_id}/{deposite_amount}/{merchant_bank}")]
        public HttpResponseMessage doPaymentWithFulfillQuote(String username_ad, String password_ad, int customer_id, int deposite_amount, string merchant_bank)
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
                //Console.WriteLine("There are no finanical account on customer with id : " + customerid);
                return null;
            }

            int ledgerid = 3; //3 means that customer do payment
            int faid = fa.Items[0].Id.Value;
            int userid = 1;  // You should use your login user id(ICC User ID) to replace this number
            var cu = customersService.GetCustomerWithoutCustomFields(customer_id);
            int businessunitid = cu.BusinessUnitId.Value;
            SHA1 sha = new SHA1CryptoServiceProvider();

            FinancialTransactionCollection ft = financeService.CreatePaymentTransactions(new FinancialTransactionCollection() {
                new FinancialTransaction()
                {
                    //CreatedByUserId=userid,
                    CustomerId = customer_id,
                    BusinessUnitId = businessunitid,
                    LedgerAccountId = ledgerid,
                    FinancialAccountId = faid,
                    CreatedByEvent = 514, //for customer payment with fullfill quote
					//CreatedByEvent = 500, //for manual debit or credit
                    //CreatedByEvent = 515, //for manual payment without fullfill quote
                    //PaidForInvoice = ,
                    
                    CreateDate = DateTime.Now,
					BaseAmount = deposite_amount,
                    //FinanceBatchCode = "Payment Mode : Cash",
                    //ThirdPartyTransactionDescription = "Cheque # :123",
                    ThirdPartyTransactionDescription = merchant_bank,
                    //ExtraPaymentInfo = "Transfer Mode :/Cheque Bank : /Payment By : ",
                    BankDate = DateTime.Now,  // Cheque Date 
                    //PaymentReferenceNumber = "Reference Number : 12456",
					//UniqueTransactionId = sha.ComputeHash(Encoding.ASCII.GetBytes("Reference Number : 124567")),
					Comment = "",
                    Extended = "",
                    

					//TransactionType = FinancialTransactionTypes.Payment,
					//TransactionSubType = FinancialTransactionSubtypes.AccountsReceivable
				}

            }, PaymentReceiptNumberingMethod.Automatic);
            //return ft;

            if (ft != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, ft);
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
