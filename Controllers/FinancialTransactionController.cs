using PayMedia.ApplicationServices.ClientProxy;
using PayMedia.ApplicationServices.SharedContracts;
using PayMedia.ApplicationServices.ViewFacade.ServiceContracts;
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
    public class FinancialTransactionController : ApiController
    {
        [HttpGet]
        [Route("api/{username_ad}/{password_ad}/financialtransaction/GetFinancialTransactionByCustId/{Id_param}")]
        public HttpResponseMessage GetWorkOrderByWorkOrderId(int Id_param, String username_ad, String password_ad)
        {
            #region Authenticate and create proxies
            Authentication_class var_auth = new Authentication_class();
            AuthenticationHeader ah = var_auth.getAuthHeader(username_ad, password_ad);
            AsmRepository.SetServiceLocationUrl(var_auth.var_service_location_url);
            var viewFacade = AsmRepository.GetServiceProxyCachedOrDefault<IViewFacadeService>(ah);
            #endregion

            #region BaseQueryRequest FilterCriteria
            //As of MR24, these are the properties you can use in the
            //BaseQueryRequest filter. Use Visual Studio's Intellisense to get the
            //list of properties that are valid in your version of ICC.
            //FinancialTransactionView f = new FinancialTransactionView
            //f.AmtExclTax;
            //f.AmtInclTax;
            //f.AppearedOnInvoiceNumber;
            //f.ApprdOnInvoiceId;
            //f.BankDatetime;
            //f.BaseAmount;
            //f.BusinessUnitDescription;
            //f.BusinessUnitId;
            //f.CreateDatetime;
            //f.CreatedByEvent;
            //f.CreatedByUserId;
            //f.CreatedByUserName;
            //f.CurrencyId;
            //f.CurrencyMnemonic;
            //f.CustomerId;
            //f.DebitOrCredit;
            //f.EntityId;
            //f.EntityType;
            //f.ExternalAgentId;
            //f.ExternalAgentName;
            //f.ExternalTaxAreaId;
            //f.ExternalTransactionId;
            //f.ExtraPaymentInfo;
            //f.FinanceBatchId;
            //f.FinancialAccountId;
            //f.HistoryId;
            //f.Id;
            //f.InternalComment;
            //f.InvoiceLineText;
            //f.InvoiceRunId;
            //f.IsPending;
            //f.LedgerAccountDescription;
            //f.LedgerAccountId;
            //f.ListPriceAmount;
            //f.ListPriceConditionId;
            //f.ListPriceTypeName;
            //f.MarketSegmentId;
            //f.MarketSegmentName;
            //f.ModelId;
            //f.NumberOfUnits;
            //f.OriginalAmount;
            //f.OriginalCurrencyId;
            //f.PaidForAccountId;
            //f.PaidForCustomerName;
            //f.PaidForInformation;
            //f.PaidForInvoiceId;
            //f.PaymentRefNum;
            //f.PeriodFrom;
            //f.PeriodizationRunId;
            //f.PeriodTo;
            //f.PriceAdjustDefId;
            //f.ProductId;
            //f.ReceiptNumber;
            //f.ReversedTransId;
            //f.TaxAmt1;
            //f.TaxAmt2;
            //f.TaxAmt3;
            //f.TaxType1;
            //f.TaxType2;
            //f.TaxType3;
            //f.TransactionSubType;
            //f.TransactionType;
            //f.TransferAccountId;
            //f.UserKey;
            //f.UserLocationName;
            #endregion

            //Instantiate a request object.
            BaseQueryRequest request = new BaseQueryRequest();
            //Page 0 returns ALL matching records. Page 0 can have a major
            //performance impact. Use it only when you know that
            //the number of matching records is small.
            request.PageCriteria = new PageCriteria { Page = 0 };
            //Instantiate the CollectionCriteria.
            request.FilterCriteria = new CriteriaCollection();
            //In this example, we have only one criteria:
            //FinancialAccountId is 401.
            //To learn how to set selection criteria in a BaseQueryRequest,
            //see the code sample called Code_BaseQuery_Request.pdf.
            request.FilterCriteria.Add(new Criteria("CustomerId", Id_param));
            //Call the method.
            var ftViewCollection = viewFacade.GetFinancialTransactionView(request);
            //Display the results, one screenful at a time.

            if (ftViewCollection != null && ftViewCollection.Items.Count > 0)
            {
                return Request.CreateResponse(HttpStatusCode.OK, ftViewCollection);
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
