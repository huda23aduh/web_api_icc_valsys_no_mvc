using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using PayMedia.ApplicationServices.ClientProxy;
using PayMedia.ApplicationServices.Factory;
using PayMedia.ApplicationServices.Finance.ServiceContracts;
using PayMedia.ApplicationServices.Finance.ServiceContracts.DataContracts;
using PayMedia.ApplicationServices.ServiceLocation;

using PayMedia.ApplicationServices.SharedContracts;
using PayMedia.Configuration;
using PayMedia.ApplicationServices.ViewFacade.ServiceContracts;
using PayMedia.ApplicationServices.ViewFacade.ServiceContracts.DataContracts;
using web_api_icc_valsys_no_mvc.Models;

namespace web_api_icc_valsys_no_mvc.Controllers
{
    public class FinancialAccountController : ApiController
    {
        
        [HttpGet]
        [ActionName("GetFinancialAccountById")]
        [Route("api/{username_ad}/{password_ad}/financialaccount/GetFinancialAccountById/{id}")]
        public HttpResponseMessage GetFinancialAccountById(int id, String username_ad, String password_ad)
        {
            Authentication_class var_auth = new Authentication_class();
            AuthenticationHeader authHeader = var_auth.getAuthHeader(username_ad, password_ad);
            AsmRepository.SetServiceLocationUrl(var_auth.var_service_location_url);

            var financeService = AsmRepository.GetServiceProxyCachedOrDefault<IFinanceService>(authHeader);
            int financialAccountId = id;
            FinancialAccount financialAccount = financeService.GetFinancialAccount(financialAccountId);

            if (financialAccount != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, financialAccount);
            }
            else
            {
                var message = string.Format("error");
                HttpError err = new HttpError(message);
                return Request.CreateResponse(HttpStatusCode.OK, message);
            }
            
        }

        [HttpGet]
        [ActionName("GetFinancialAccountByCustomerId")]
        [Route("api/{username_ad}/{password_ad}/financialaccount/GetFinancialAccountByCustomerId/{id}")]
        public HttpResponseMessage GetFinancialAccountByCustomerId(int id, String username_ad, String password_ad)
        {
            Authentication_class var_auth = new Authentication_class();
            AuthenticationHeader ah = var_auth.getAuthHeader(username_ad, password_ad);
            AsmRepository.SetServiceLocationUrl(var_auth.var_service_location_url);
            var financeService = AsmRepository.GetServiceProxyCachedOrDefault<IFinanceService>(ah);
            //As of MR29, these are the properties you can use to filter
            //the response. Check the API Reference Library (CHM) file for the
            //latest properties.
            //FinancialAccount f = new FinancialAccount();
            //f.AmountInDispute;
            //f.AnniversaryMonth;
            //f.Balance;
            //f.BankAccountId;
            //f.BankAccountType;
            //f.BankBranchId;
            //f.BankCodeId;
            //f.BankName;
            //f.CreatedDate;
            //f.CreditCard;
            //f.CurrencyId;
            //f.CustomerId;
            //f.CustomFields;
            //f.DailyBalance;
            //f.DueIn;
            //f.DunningLevel;
            //f.ExternalTaxExemptionCertificate;
            //f.FinalInvoice;
            //f.FirstNextBillDate;
            //f.HasBeenApplied;
            //f.IBANAccountId;
            //f.Id;
            //f.InvoiceDeliveryMethodId;
            //f.InvoicingMethod;
            //f.InvoicingProfileId;
            //f.IsDefault;
            //f.LastInvoiced;
            //f.MethodOfPaymentId;
            //f.MopId;
            //f.Name;
            //f.NextInvoiceText;
            //f.NotYetApplied;
            //f.ParentFinancialAccountId;
            //f.PrepaidBalance;
            //f.PrepaidBalanceAsOf;
            //f.PrepaidDisconnectDate;
            //f.PrepaidEndDate;
            //f.PrepaidManualDisconnectDate;
            //f.ProxyCode;
            //f.ProxyCodeId;
            //f.ReferenceNumber;
            //f.Status;
            //f.StatusId;
            //f.SuspendInvoicing;
            //f.SuspendInvoicingDueDate;
            //f.SWIFTCode;
            //f.Type;
            //f.TypeId;
            //Instantiate a BaseQueryRequest object for the input parameter and
            //pass in the criteria as a key-value pair.
            //For details, see the BaseQueryRequest code sample in the
            //ICC API Developer's Guide.
            BaseQueryRequest request = new BaseQueryRequest();
            request.FilterCriteria = new CriteriaCollection();
            //In the UI, the label for the Status property is
            //"External Reference ID."
            request.FilterCriteria.Add(new Criteria("CustomerId", id));
            //Call the method and display the results.
            FinancialAccountCollection faColl = financeService.GetFinancialAccounts(request);

            if (faColl != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, faColl);
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
