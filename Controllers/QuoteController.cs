using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using PayMedia.ApplicationServices.BillingEngine.ServiceContracts;
using PayMedia.ApplicationServices.BillingEngine.ServiceContracts.DataContracts;
using PayMedia.ApplicationServices.Finance.ServiceContracts.DataContracts;
using PayMedia.ApplicationServices.Finance.ServiceContracts;
using PayMedia.ApplicationServices.SharedContracts;
using PayMedia.ApplicationServices.ClientProxy;
using System.Collections;

using web_api_icc_valsys_no_mvc.Models;
using PayMedia.ApplicationServices.AgreementManagement.ServiceContracts;
using PayMedia.ApplicationServices.AgreementManagement.BizObj;
using System.Threading.Tasks;

namespace web_api_icc_valsys_no_mvc.Controllers
{
    public class QuoteController : ApiController
    {
        

        [HttpGet]
        [Route("api/{username_ad}/{password_ad}/quote/GetPendingQuote")]
        public List<QuoteInvoiceCollection> GetPendingQuote(String username_ad, String password_ad)
        {
            Authentication_class var_auth = new Authentication_class();
            AuthenticationHeader ah = var_auth.getAuthHeader(username_ad, password_ad);
            AsmRepository.SetServiceLocationUrl(var_auth.var_service_location_url);
            var billService = AsmRepository.GetServiceProxy<IBillingEngineService>(ah);
            var finService = AsmRepository.AllServices.GetFinanceService(ah);

            QuoteInvoiceCollection quotes = null;
            decimal total_amount = 0;
            ArrayList falist = new ArrayList();
            long new_max_quote_id = 1;
            int total_record = 0;
            List<QuoteInvoiceCollection> the_items = new List<QuoteInvoiceCollection>();

                for (int i = 1; i < 10000; i++)
                {
                    quotes = billService.FindQuoteInvoices(new BaseQueryRequest()
                    {
                        FilterCriteria = Op.Eq("QuoteStatusId", "1") & Op.Gt("Id", 1),
                        PageCriteria = new PageCriteria()
                        {
                            Page = i,
                            PageSize = 300
                        },
                        SortCriteria = new SortCriteriaCollection()
                        {
                            new SortCriteria()
                            {
                                SortDirection = SortDirections.Ascending,
                                    Key = "Id"
                            }
                        },
                        DeepLoad = true
                    });
                    //Console.WriteLine("Loop " + i + " Times!");
                    if (quotes.TotalCount == 0)
                    {
                        //Console.WriteLine("End Loop ...... ");
                        break;
                    }
                    // set the new max quote id for next run

                    int count = quotes.TotalCount - 1;
                    new_max_quote_id = quotes.Items[count].Id.Value;

                    foreach (var quote in quotes.Items)
                    {
                        // Avoid duplicate records
                        if (falist.Contains(quote.FinancialAccountId))
                        {
                            continue;
                        }
                        else
                        {
                            falist.Add(quote.FinancialAccountId);
                        }

                        total_amount = 0;
                        total_amount = quote.TotalAmount.Value;

                        var fa = finService.GetFinancialAccount(quote.FinancialAccountId.Value);

                        // search all pending quote for same financial account and count total
                        // Why :  Customer may have multiple pending quote if he request more products or upgrade in different time, so you need count all of them.
                        foreach (var quotet in quotes.Items)
                        {
                            if (quotet.FinancialAccountId == quote.FinancialAccountId && quote.Id != quotet.Id)
                            {
                                total_amount = total_amount + quotet.TotalAmount.Value;
                                total_record++;
                                //Console.WriteLine("quotet.TotalAmount.Value " + quotet.TotalAmount.Value);
                            }
                        }

                        // Add the account debit like payment fee, downgrade fee...etc. so you need to add account balance
                        total_amount = total_amount + fa.Balance.Value;

                        //Console.WriteLine("Customer Id : " + quote.CustomerId.Value + " FA ID : " + fa.Id.Value + " , unpaid quote amount : " + total_amount);
                        
                        //if (quote.LineItems.TotalCount > 0)
                        //{
                        //    Console.WriteLine("From " + quote.LineItems.Items[0].StartDate + " To " + quote.LineItems.Items[0].EndDate);
                        //}
                        total_record++;
                        
                    }
                    the_items.Add(new QuoteInvoiceCollection(quotes));
                    quotes = null;
                    
                }

                //Console.WriteLine("Total " + total_record + " pending quotes");
                //Console.Read();

                //return new_max_quote_id;
              
            return the_items;

        }

        [HttpGet]
        [Route("api/{username_ad}/{password_ad}/quote/GetQuoteByCustomerId/{id}")]
        public QuoteInvoiceCollection GetQuoteByCustomerId(String username_ad, String password_ad, int id)
        {
            Authentication_class var_auth = new Authentication_class();
            AuthenticationHeader ah = var_auth.getAuthHeader(username_ad, password_ad);
            AsmRepository.SetServiceLocationUrl(var_auth.var_service_location_url);
            var billingService = AsmRepository.GetServiceProxyCachedOrDefault<IBillingEngineService>(ah);
            var financeService = AsmRepository.GetServiceProxyCachedOrDefault<IFinanceService>(ah);
            var agreementManagementService = AsmRepository.GetServiceProxyCachedOrDefault<IAgreementManagementService>(ah);
            #region Step 1
            //First, get Pending quotes to check their QuoteDates.
            //Instantiate a class for the request.
            BaseQueryRequest req = new BaseQueryRequest();
            req.FilterCriteria = new CriteriaCollection();
            //Define the selection criteria.
            //Replace 73948 with the CustomerId of the customer you want.
            req.FilterCriteria.Add("CustomerId", id);
            //Get the customer's QuoteInvoices.
            QuoteInvoiceCollection qts = billingService.FindQuoteInvoices(req);
            if (qts != null && qts.Items.Count > 0)
            {
                return qts;
                //foreach (var q in qts)
                //{
                //    Console.WriteLine("Customer ID {0}: QuoteDate = {1} QuoteInvoice ID = {2}",
                //    q.CustomerId, q.QuoteDate, q.Id);
                //    Console.WriteLine("Quote amount = {0} Quote type = {1}",
                //    q.TotalAmount, q.QuoteType);
                //    //If the QuoteDate < Today, then regenerate the quote.
                //    if (q.QuoteDate < System.DateTime.Today)
                //    {
                //        QuoteInvoiceRequest request = new QuoteInvoiceRequest();
                //        request.QuoteId = q.Id;
                //        //If Save = True, ICC saves the QuoteInvoice.
                //        //To get the quote info but not commit it to the database,
                //        //set the flag to False. This is useful when
                //        //you only want to know what the customer would pay next
                //        //period, but you don't want to create the quote.
                //        request.Save = true;
                //        //Regenerate the quote to save Today's quote info.
                //        QuoteInvoiceResponse response = billingService.RegenerateQuoteInvoice(request);
                //        Console.WriteLine("New quote generated. ID = {0}; Amount = {1}",
                //        response.NewQuoteInvoice.Id, response.NewQuoteInvoice.TotalAmount);
                //    }
                //    else
                //    {
                //        Console.WriteLine("Customer's quotes are up-to-date.");
                //    }
                //}
            }
            else
                return null;
            #endregion step 1
                    

        }

        [HttpGet]
        [Route("api/{username_ad}/{password_ad}/quote/GetTotalAmountQuoteByCustomerId/{id}")]
        public HttpResponseMessage GetTotalAmountQuoteByCustomerId(String username_ad, String password_ad, int id)
        {
            Authentication_class var_auth = new Authentication_class();
            AuthenticationHeader authHeader = var_auth.getAuthHeader(username_ad, password_ad);
            AsmRepository.SetServiceLocationUrl(var_auth.var_service_location_url);
            var billService = AsmRepository.GetServiceProxy<IBillingEngineService>(authHeader);
            var finService = AsmRepository.AllServices.GetFinanceService(authHeader);

            decimal totalamount = 0;

            var fa = finService.GetFinancialAccountsForCustomer(id, new CriteriaCollection() {
                new Criteria() {
                    Key="TypeId",
                    Operator=Operator.Equal,
                    Value="1"
                    }
                }, 0).Items[0];

            //Console.WriteLine("Financial Account Id : " + fa.Id.Value);

            var quoteinvoice = billService.FindQuoteInvoices(new BaseQueryRequest()
            {
                FilterCriteria = Op.Eq("FinancialAccountId", fa.Id.Value) & Op.Eq("QuoteStatusId", "1")
            });

            foreach (var quote in quoteinvoice)
            {
                string quote_type = "";
                //Console.WriteLine("Find quote Id : " + quote.Id.Value);
                if (quote.QuoteGenerationMethod == QuoteGenerationMethod.EventBased)
                {
                    quote_type = "Immediate Quote";
                }
                else if (quote.QuoteGenerationMethod == QuoteGenerationMethod.ServiceRetention)
                {
                    quote_type = "Regular Quote";
                }
                //Console.WriteLine("Quote Type" + quote_type);
                totalamount = quote.TotalAmount.Value + totalamount;

            }


            return Request.CreateResponse(HttpStatusCode.OK, "TotalAmount = " + totalamount);



        }

        //[HttpGet]
        //[Route("api/quote/useCreditByAgreementDetailId/{agreement_detail_id}")]
        //public QuoteInvoiceCollection useCreditByAgreementDetailId(int agreement_detail_id)
        //{
        //    Authentication_class var_auth = new Authentication_class();
        //    AuthenticationHeader authHeader = var_auth.getAuthHeader();
        //    AsmRepository.SetServiceLocationUrl(var_auth.var_service_location_url);
        //    var agService = AsmRepository.AllServices.GetAgreementManagementService(authHeader);
        //    var agcService = AsmRepository.AllServices.GetAgreementManagementConfigurationService(authHeader);
        //    var productService = AsmRepository.AllServices.GetProductCatalogService(authHeader);
        //    var productcService = AsmRepository.AllServices.GetProductCatalogConfigurationService(authHeader);
        //    var custcService = AsmRepository.AllServices.GetCustomersConfigurationService(authHeader);
        //    var fincService = AsmRepository.AllServices.GetFinanceConfigurationService(authHeader);
        //    var sandboxService = AsmRepository.AllServices.GetSandBoxManagerService(authHeader);
        //    var custService = AsmRepository.AllServices.GetCustomersService(authHeader);
        //    var priceServce = AsmRepository.AllServices.GetPricingService(authHeader);
        //    var billService = AsmRepository.GetServiceProxy<IBillingEngineService>(authHeader);
        //    var finService = AsmRepository.AllServices.GetFinanceService(authHeader);
        //    var prepaidcService = AsmRepository.GetServiceProxy<IPrepaidConfigurationService>(authHeader);
        //    try
        //    {
        //        int customer_id;
        //        int quote_id;
        //        var ag = agService.GetAgreementDetail(agreement_detail_id);

        //        if (ag == null)
        //        {
        //            //Console.WriteLine("Agreement detail id :" + agreement_detail_id + " not exist!");
        //            return null;
        //        }

        //        var fa = finService.GetFinancialAccountsForCustomer(ag.CustomerId.Value, new CriteriaCollection() {
        //        new Criteria() {
        //            Key="TypeId",
        //            Operator=Operator.Equal,
        //            Value="1"
        //            }
        //        }, 0);

        //        if (fa.Items.Count == 0)
        //        {
        //            //Console.WriteLine("There are no finanical account on customer with id : ");
        //            return null ;
        //        }

        //        //var quote = GetLastPendingQuoteByAgreementDetail(agreement_detail_id);   // method definition is blow

        //        //List<int> products = new List<int>();

        //        //if (quote != null)
        //        //{


        //        //    if (quote.QuoteStatusId != 1 || quote.LineItems == null || quote.LineItems.Items.Count == 0)
        //        //    {
        //        //        Console.WriteLine("quote with id :  " + quote.Id.Value + " can't being fulfilled! it is not in pending status or no line item in it!");
        //        //        return ;
        //        //    }

        //        //    foreach (QuoteInvoiceLineItem item in quote.LineItems)
        //        //    {
        //        //        if (item.EntityType == AgreementDetail.ENTITY_ID)
        //        //        {
        //        //            products.Add(item.EntityId.GetValueOrDefault());
        //        //        }
        //        //    }
        //        //}
        //        //else
        //        //{
        //        //    Console.WriteLine("There are no pending quote for product with id : " + agreement_detail_id);
        //        //    return;
        //        //}

        //        var criteria = new CreateVoucherChargesCriteria()
        //        {
        //            CreateCharges = true,
        //            AutomaticRedistributionForAccountId = fa.Items[0].Id.Value,
        //            QuoteId = quote.Id.Value,
        //            AutomaticRedistributionForProductIds = products,
        //            CreateInvoiceAfterDistribution = true,
        //            TransferFundsToVoucherProductReason = prepaidcService.GetPrepaidDefaults().ReasonForAutomaticFundTransfer

        //        };

        //        var result = billService.CreateConditionalVoucherCharges(criteria);

        //        if (result == null || result.SuccessfulVoucherCharges == null || result.SuccessfulVoucherCharges.Items.Count == 0)
        //        {
        //            if (result.FailedVoucherCharges != null && result.FailedVoucherCharges.Items.Count > 0)
        //                Console.WriteLine("Failed use credit because : " + result.FailedVoucherCharges.Items[0].ValidationMessage);
        //            else
        //                Console.WriteLine("Failed to use credit, maybe the credit is not enough!");
        //        }
        //        else
        //        {
        //            Console.WriteLine(" use Credit Successfully!");
        //        }

        //        Console.Read();

        //    }
        //    catch (Exception ex)
        //    {
        //        msg = "Error : " + ex.Message + " --- Exceptions Stack ---- ；  " + ex.StackTrace;
        //        logger.Error(msg);
        //        Console.WriteLine(msg);
        //    }

        //}


        //// Get pending quote by agreement_detail_id  
        //public QuoteInvoice GetLastPendingQuoteByAgreementDetail(int agreement_detail_id)
        //{
        //    try
        //    {
        //        QuoteInvoice qi = null;

        //        var ag = agService.GetAgreementDetail(agreement_detail_id);

        //        if (ag == null)
        //        {
        //            Console.WriteLine("Agreement detail id :" + agreement_detail_id + " not exist!");
        //            return null;
        //        }

        //        var fa = finService.GetFinancialAccountsForCustomer(ag.CustomerId.Value, new CriteriaCollection() {
        //        new Criteria() {
        //            Key="TypeId",
        //            Operator=Operator.Equal,
        //            Value="1"
        //            }
        //        }, 0).Items[0];

        //        Console.WriteLine("Financial Account Id : " + fa.Id.Value);

        //        var quoteinvoice = billService.FindQuoteInvoices(new BaseQueryRequest()
        //        {
        //            FilterCriteria = Op.Eq("FinancialAccountId", fa.Id.Value) & Op.Eq("QuoteStatusId", "1")
        //        });

        //        foreach (var quote in quoteinvoice)
        //        {
        //            string quote_type = "";
        //            Console.WriteLine("Find quote Id : " + quote.Id.Value);
        //            if (quote.QuoteGenerationMethod == QuoteGenerationMethod.EventBased)
        //            {
        //                quote_type = "Immediate Quote";
        //            }
        //            else if (quote.QuoteGenerationMethod == QuoteGenerationMethod.ServiceRetention)
        //            {
        //                quote_type = "Regular Quote";
        //            }
        //            Console.WriteLine("Quote Type" + quote_type);

        //            if (quote.LineItems.Items.Count > 0)
        //            {
        //                foreach (var item in quote.LineItems.Items)
        //                {
        //                    if (item.EntityId == agreement_detail_id)
        //                    {
        //                        qi = quote;
        //                    }
        //                }
        //            }

        //        }

        //        return qi;

        //    }
        //    catch (Exception ex)
        //    {
        //        msg = "Error : " + ex.Message + " --- Exceptions Stack ---- ；  " + ex.StackTrace;
        //        logger.Error(msg);
        //        Console.WriteLine(msg);
        //        return null;
        //    }


        //}


    }
}
