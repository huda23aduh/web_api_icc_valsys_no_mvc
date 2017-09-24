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
using PayMedia.ApplicationServices.Customers.ServiceContracts;
using PayMedia.ApplicationServices.Finance.ServiceContracts;
using PayMedia.ApplicationServices.AgreementManagement.ServiceContracts;

using web_api_icc_valsys_no_mvc.Models;
using PayMedia.ApplicationServices.AgreementManagement.ServiceContracts.DataContracts;

using PayMedia.ApplicationServices.ProductCatalog.ServiceContracts; //Provides enums and methods
using PayMedia.ApplicationServices.ProductCatalog.ServiceContracts.DataContracts; //Provides

namespace web_api_icc_valsys_no_mvc.Controllers
{
    public class ProductController : ApiController
    {

        //[HttpGet]
        //[ActionName("GetProductNow")]
        //public CommercialProductCollection GetProductNow()
        //{
        //    AuthenticationHeader authHeader;

        //    authHeader = new AuthenticationHeader();
        //    authHeader.Dsn = "MSKY-TRA";
        //    authHeader.UserName = "ICCAPI";
        //    authHeader.Proof = "api9hsn!";
        //    AsmRepository.SetServiceLocationUrl(var_auth.var_service_location_url);

        //    IProductCatalogConfigurationService prodService = AsmRepository.AllServices.GetProductCatalogConfigurationService(authHeader);

        //    CommercialProductCollection products = prodService.GetCommercialProducts(new BaseQueryRequest()
        //    {
        //        FilterCriteria = new CriteriaCollection()
        //        {
        //            new Criteria("SellTo",DateTime.Now,Operator.GreaterThanOrEqualTo)
        //        }
        //    });

        //    return products;
        //}

        //[HttpGet]
        //[ActionName("YangProduct")]
        //public CommercialProductCollection YangProduct()
        //{
        //    YangProductResponse aa = new YangProductResponse();

        //    IAgreementManagementService agService = null;
        //    IAgreementManagementConfigurationService agcService = null;
        //    IProductCatalogService productService = null;
        //    IProductCatalogConfigurationService productcService = null;
        //    ICustomersConfigurationService custcService = null;
        //    IFinanceConfigurationService fincService = null;

        //    AuthenticationHeader authHeader = new AuthenticationHeader
        //    {
        //        UserName = "ICCAPI",
        //        Proof = "api9hsn!",
        //        Dsn = "MSKY-TRA",
        //        ExternalAgent = "QuotesSystem"
        //    };
        //    AsmRepository.SetServiceLocationUrl(var_auth.var_service_location_url);

        //    agService = AsmRepository.AllServices.GetAgreementManagementService(authHeader);
        //    agcService = AsmRepository.AllServices.GetAgreementManagementConfigurationService(authHeader);
        //    productService = AsmRepository.AllServices.GetProductCatalogService(authHeader);
        //    productcService = AsmRepository.AllServices.GetProductCatalogConfigurationService(authHeader);
        //    custcService = AsmRepository.AllServices.GetCustomersConfigurationService(authHeader);
        //    fincService = AsmRepository.AllServices.GetFinanceConfigurationService(authHeader);

        //    CommercialProductCollection pl = productcService.GetCommercialProducts(new BaseQueryRequest()
        //    {
        //        FilterCriteria = Op.Le("SellFrom", DateTime.Now) && Op.Ge("SellTo", DateTime.Now),
        //        DeepLoad = true,
        //        PageCriteria = new PageCriteria()
        //        {
        //            Page = 0
        //        }

        //    });




        //    return pl;
        //}

        
        [HttpGet]
        [Route("api/{username_ad}/{password_ad}/product/GetCommercialProduct")]
        public HttpResponseMessage GetCommercialProduct(String username_ad, String password_ad)
        {
            Authentication_class var_auth = new Authentication_class();
            AuthenticationHeader ah = var_auth.getAuthHeader(username_ad, password_ad);
            AsmRepository.SetServiceLocationUrl(var_auth.var_service_location_url);
            var productCatalogConfigurationService = AsmRepository.GetServiceProxyCachedOrDefault<IProductCatalogConfigurationService>(ah);
            //As of MR26, these are the properties you can use in the BaseQueryRequest.
            //Use Visual Studio's Intellisense function or see the API Reference Library
            //for the latest properties and descriptions.
            //CommercialProduct c = new CommercialProduct();
            //c.AgreementDetailSpearId;
            //c.AllowedBusinessUnits;
            //c.AllowedDisconnectSettings;
            //c.AllowedEventsId;
            //c.AllowedForPurchasedOfferDefinition;
            //c.AllowedMarketSegmentIds;
            //c.AllowedModels;
            //c.AllowedPaymentType;
            //c.AllowIndefiniteSleep;
            //c.AllowQtyToBeUpdated;
            //c.AllowRedistributeFunds;
            //c.AllowSleep;
            //c.ApplyRemainingFunds;
            //c.AreDevicesPresent;
            //c.Availability;
            //c.AvailableDate;
            //c.BusinessUnitId;
            //c.CAEntitlements;
            //c.CaptureRuleId;
            //c.CategoryId;
            //c.CategoryWeight;
            //c.ChargeForUnderlyingTP;
            //c.CommercialProductIds;
            //c.ContractPeriodIdToOverrideOnAD;
            //c.DefaultChargePeriod;
            //c.DefaultDisconnectionSetting;
            //c.DefaultFinanceOptionId;
            //c.DefaultPaymentType;
            //c.Description;
            //c.EditRulesId;
            //c.EPCMappings;
            //c.ExcludedCommercialProducts;
            //c.ExternalId;
            //c.ExternalProductCodeId;
            //c.Hyperlink;
            //c.IconId;
            //c.Id;
            //c.InvoiceLineTexts;
            //c.IsServiceContract;
            //c.IsShippingOrderProduct;
            //c.IsStockHandlerOrderProduct;
            //c.IsUnitOfMeasurementRequired;
            //c.MatchQuantity;
            //c.MaxSleepDays;
            //c.MinSleepDays;
            //c.Name;
            //c.Notes;
            //c.OverrideCAEntitlements;
            //c.PrepaidRequiredBalance;
            //c.PrepaidRequiredBalanceReconnect;
            //c.RequiredCommercialProducts;
            //c.SellFrom;
            //c.SellTo;
            //c.TaxExternalProductCodeId;
            //c.TechnicalProductIds;
            //c.TechnicalProducts;
            //c.Terms;
            //c.UnitOfMeasurementId;
            //c.UsersRoles;
            //Instantiate and initialize the filter criteria (key-value pairs)
            //for the BaseQueryRequest object.
            BaseQueryRequest request = new BaseQueryRequest();
            CriteriaCollection criteria = new CriteriaCollection();
            //request.FilterCriteria.Add(new Criteria("ExternalId", 92591));
            request.FilterCriteria.Add(new Criteria("IconId", ""));
            //Call the method and display the results.
            CommercialProductCollection coll = productCatalogConfigurationService.GetCommercialProducts(request);

            if (coll != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, coll);
            }
            else
            {
                var message = string.Format("error");
                HttpError err = new HttpError(message);
                return Request.CreateResponse(HttpStatusCode.OK, message);
            }

            
        }

        [HttpGet]
        [Route("api/{username_ad}/{password_ad}/product/YangProductPrice")]
        public YangProductPriceResponse YangProductPrice(String username_ad, String password_ad)
        {
            YangProductPriceResponse db = new YangProductPriceResponse();
            var cc = new List<YangProductPriceItems>();

            List<int> CustomerClassIds_local = null;
            List<int> CountryIds_local = null; 
            List<int> CustomerTypeIds_local = null; 
            List<int> FinanceOptionIds_local = null;
            List<int> FinancialAccountTypeIds_local = null; 
            List<string> PostalCodes_local = null;
            List<int> ProvinceIds_local = null;
            List<ListPriceCondition> ListPriceCondition_local = null;

            IAgreementManagementService agService = null;
            IAgreementManagementConfigurationService agcService = null;
            IProductCatalogService productService = null;
            IProductCatalogConfigurationService productcService = null;
            ICustomersConfigurationService custcService = null;
            IFinanceConfigurationService fincService = null;


            Authentication_class var_auth = new Authentication_class();
            AuthenticationHeader ah = var_auth.getAuthHeader(username_ad, password_ad);
            AsmRepository.SetServiceLocationUrl(var_auth.var_service_location_url);

            agService = AsmRepository.AllServices.GetAgreementManagementService(ah);
            agcService = AsmRepository.AllServices.GetAgreementManagementConfigurationService(ah);
            productService = AsmRepository.AllServices.GetProductCatalogService(ah);
            productcService = AsmRepository.AllServices.GetProductCatalogConfigurationService(ah);
            custcService = AsmRepository.AllServices.GetCustomersConfigurationService(ah);
            fincService = AsmRepository.AllServices.GetFinanceConfigurationService(ah);

            var pl = productcService.GetCommercialProducts(new BaseQueryRequest()
            {
                FilterCriteria = Op.Le("SellFrom", DateTime.Now) && Op.Ge("SellTo", DateTime.Now),
                DeepLoad = true,
                PageCriteria = new PageCriteria()
                {
                    Page = 0
                }

            });



            //Console.WriteLine("Total Commercial Product Count : " + pl.TotalCount);
            db.the_TotalCount = pl.TotalCount;

            foreach (var pr in pl.Items)
            {
                int commercial_product_id = pr.Id.Value;
                var prices = productcService.GetListPrices(new BaseQueryRequest()
                {
                    FilterCriteria = Op.Eq("UsedBy", 0) && Op.Eq("Active", true) && Op.Eq("ApplyToId", commercial_product_id),
                    DeepLoad = true,
                    PageCriteria = new PageCriteria()
                    {
                        Page = 0
                    }

                });

                //Console.WriteLine("Product Name : " + pr.Name + "\n");

                foreach (var ccc in pr.CommercialProductIds)
                {
                    //Console.WriteLine("value = {0}" + ccc.ToString());

                }
                


                //Console.WriteLine("Start to search all prices for commecial products : " + pr.Name + "\n");

                if (prices == null)
                {
                    continue;
                }

                foreach (var listprice in prices.Items)
                {
                    string msg = "";

                    if (listprice.PriceConditions == null)
                    {
                        continue;
                    }

                    foreach (var pricecondition in listprice.PriceConditions.Items)
                    {
                        msg = "";
                        if (pricecondition.Active == false)
                        {
                            continue;
                        }

                        // Type - OnceOff, Recurrence
                        msg = " Price Type : " + pricecondition.Type + "\n";

                        // Charge Period

                        if (pricecondition.ChargePeriodId != 0)
                        {
                            msg = msg + " Charge Period " + pricecondition.ChargePeriodId + " Month" + "\n";
                        }

                        // The price amount
                        if (pricecondition.PriceAmounts != null)
                        {
                            string priceamount = "";
                            foreach (var pa in pricecondition.PriceAmounts)
                            {
                                priceamount += " Amount : " + pa.Amount + " From " + pa.FromDate + ",";
                            }

                            if (priceamount != "")
                            {
                                msg = msg + " " + priceamount + "\n";
                            }

                            priceamount = "";
                        }


                        // Allowed Agreement Type conditions

                        if (pricecondition.AgreementTypeIds != null)
                        {
                            string agreement_type_list = "";
                            foreach (var agtid in pricecondition.AgreementTypeIds)
                            {
                                agreement_type_list += agcService.GetAgreementType(agtid).Description + ",";

                            }
                            if (agreement_type_list != "")
                            {
                                msg = msg + "Allowed Agreement Types :  " + agreement_type_list + "\n";
                            }
                            agreement_type_list = "";
                        }

                        // Allowed Business Unit Attributes

                        if (pricecondition.BusinessUnitAttributeValues != null)
                        {
                            string business_unit_attribute = "";
                            foreach (var bu in pricecondition.BusinessUnitAttributeValues)
                            {
                                business_unit_attribute += bu + ",";
                            }
                            if (business_unit_attribute != "")
                            {
                                msg = msg + "   Allowed Business Unit Attribute : " + business_unit_attribute + "\n";
                            }
                            business_unit_attribute = "";
                        }

                        // Allowed Charge Period

                        if (pricecondition.ChargePeriodId != null)
                        {
                            string charge_period = "";
                            if (pricecondition.ChargePeriodId == 1)
                            {
                                charge_period = "Monthly";
                            }

                            if (pricecondition.ChargePeriodId == 3)
                            {
                                charge_period = "Quauterly";
                            }

                            if (pricecondition.ChargePeriodId == 6)
                            {
                                charge_period = "HalfYearly";
                            }
                            if (pricecondition.ChargePeriodId == 12)
                            {
                                charge_period = "Yearly";
                            }
                            if (charge_period != "")
                            {
                                msg = msg + "   Allowed Charge Period : " + charge_period + "\n";
                            }
                            charge_period = "";
                        }


                        // Allowed Country

                        if (pricecondition.CountryIds != null)
                        {
                            string country_list = "";
                            foreach (var c in pricecondition.CountryIds)
                            {
                                country_list += custcService.GetCountry(c).Description + ",";
                            }
                            if (country_list != "")
                            {
                                msg = msg + " Allowed Country : " + country_list + "\n";
                            }
                            country_list = "";
                            CountryIds_local = pricecondition.CountryIds;
                        }



                        // Allowed Currency

                        if (pricecondition.CurrencyId != null)
                        {
                            string currency = "";

                            currency = fincService.GetCurrency(pricecondition.CurrencyId.Value).Description;

                            if (currency != "")
                            {
                                msg = msg + "  Allowed Currency : " + currency + "\n";
                            }
                            currency = "";
                        }

                        // Allowed Customer Class

                        if (pricecondition.CustomerClassIds != null)
                        {
                            string customer_class = "";

                            foreach (var ci in pricecondition.CustomerClassIds)
                            {
                                customer_class += custcService.GetCustomerClass(ci).Description + ",";
                            }
                            if (customer_class != "")
                            {
                                msg = msg + "  Allowed Customer Class : " + customer_class + "\n";
                            }
                            customer_class = "";

                            CustomerClassIds_local = pricecondition.CustomerClassIds;
                        }

                        // Allowed Customer Type

                        if (pricecondition.CustomerTypeIds != null)
                        {
                            string customer_type = "";

                            foreach (var ct in pricecondition.CustomerTypeIds)
                            {
                                customer_type += custcService.GetCustomerType(ct).Name + ",";
                            }
                            if (customer_type != null)
                            {
                                msg = msg + "  Allowed Customer Type : " + customer_type + "\n";
                            }
                            customer_type = "";
                            CustomerTypeIds_local = pricecondition.CustomerTypeIds;
                        }


                        // Allowed Finance Options

                        if (pricecondition.FinanceOptionIds != null)
                        {
                            string financeoption = "";

                            foreach (var fo in pricecondition.FinanceOptionIds)
                            {
                                financeoption += productcService.GetFinanceOption(fo).Description + ",";
                            }

                            if (financeoption != "")
                            {
                                msg = msg + " Allowed Finance Options : " + financeoption + "\n";
                            }
                            FinanceOptionIds_local = pricecondition.FinanceOptionIds;
                        }

                        // Allowed Finance Account Type

                        if (pricecondition.FinancialAccountTypeIds != null)
                        {
                            string fatype = "";

                            foreach (var fatypeid in pricecondition.FinancialAccountTypeIds)
                            {
                                fatype += fincService.GetFinancialAccountType((fatypeid)).Description + ",";
                            }

                            if (fatype != "")
                            {
                                msg = msg + "  Allowed Financial Account Type : " + fatype + "\n";
                            }
                        }

                        // Allowed Postal Codes

                        if (pricecondition.PostalCodes != null)
                        {
                            string postcode = "";

                            foreach (var pcode in pricecondition.PostalCodes)
                            {
                                postcode += pcode + ",";
                            }

                            if (postcode != "")
                            {
                                msg = msg + "  Allowed Postal Code : " + postcode + "\n";
                            }
                            PostalCodes_local = pricecondition.PostalCodes;
                        }


                        // Allowed Province

                        if (pricecondition.ProvinceIds != null)
                        {
                            string province = "";

                            foreach (var pro in pricecondition.ProvinceIds)
                            {
                                province += custcService.GetProvince(pro).Description + ",";
                            }

                            if (province != "")
                            {
                                msg = msg + "  Allowed Province : " + province + "\n";
                            }
                            ProvinceIds_local = pricecondition.ProvinceIds;
                        }
                        ListPriceCondition_local = listprice.PriceConditions.Items;
                        //Console.WriteLine("Product : " + pr.Name + " ---- " + msg + "\n");
                    }

                    //Console.WriteLine("Product : " + pr.Name + " ---- " + msg);

                }

                //Console.WriteLine("\n---------------------------------------------------------------\n");

                ///////////////////////
                cc.Add(new YangProductPriceItems {
                    the_Name = pr.Name,  the_CommercialProductIds = pr.CommercialProductIds,
                    the_ListPriceCollection = prices.Items , the_CustomerClassIds = CustomerClassIds_local,
                    the_CountryIds = CountryIds_local, the_CustomerTypeIds = CustomerTypeIds_local,
                    the_ListPriceConditions = ListPriceCondition_local
                });
            }
            db.the_items = cc;
            return db;

        }

        [HttpGet]
        [Route("api/{username_ad}/{password_ad}/product/YangProductPriceByProductId/{id}")]
        public YangProductPriceResponse YangProductPriceByProductId(String username_ad, String password_ad, int id)
        {
            YangProductPriceResponse db = new YangProductPriceResponse();
            var cc = new List<YangProductPriceItems>();

            List<int> CustomerClassIds_local = null;
            List<int> CountryIds_local = null;
            List<int> CustomerTypeIds_local = null;
            List<int> FinanceOptionIds_local = null;
            List<int> FinancialAccountTypeIds_local = null;
            List<string> PostalCodes_local = null;
            List<int> ProvinceIds_local = null;
            List<ListPriceCondition> ListPriceCondition_local = null;

            IAgreementManagementService agService = null;
            IAgreementManagementConfigurationService agcService = null;
            IProductCatalogService productService = null;
            IProductCatalogConfigurationService productcService = null;
            ICustomersConfigurationService custcService = null;
            IFinanceConfigurationService fincService = null;


            Authentication_class var_auth = new Authentication_class();
            AuthenticationHeader ah = var_auth.getAuthHeader(username_ad, password_ad);
            AsmRepository.SetServiceLocationUrl(var_auth.var_service_location_url);

            agService = AsmRepository.AllServices.GetAgreementManagementService(ah);
            agcService = AsmRepository.AllServices.GetAgreementManagementConfigurationService(ah);
            productService = AsmRepository.AllServices.GetProductCatalogService(ah);
            productcService = AsmRepository.AllServices.GetProductCatalogConfigurationService(ah);
            custcService = AsmRepository.AllServices.GetCustomersConfigurationService(ah);
            fincService = AsmRepository.AllServices.GetFinanceConfigurationService(ah);

            BaseQueryRequest request = new BaseQueryRequest();
            CriteriaCollection criteria = new CriteriaCollection();
            //request.FilterCriteria.Add(new Criteria("ExternalId", 92591));
            request.FilterCriteria.Add(new Criteria("Id", id));
            //Call the method and display the results.
            CommercialProductCollection pl = productcService.GetCommercialProducts(request);

            //CommercialProductCollection pl = productcService.GetCommercialProducts(new BaseQueryRequest()
            //{
            //    //FilterCriteria = Op.Le("SellFrom", DateTime.Now) && Op.Ge("SellTo", DateTime.Now),
            //    FilterCriteria = Op.Equals("Id", id),
            //    DeepLoad = true,
            //    PageCriteria = new PageCriteria()
            //    {
            //        Page = 0
            //    }

            //});



            //Console.WriteLine("Total Commercial Product Count : " + pl.TotalCount);
            db.the_TotalCount = pl.TotalCount;

            foreach (var pr in pl.Items)
            {
                int commercial_product_id = pr.Id.Value;
                var prices = productcService.GetListPrices(new BaseQueryRequest()
                {
                    FilterCriteria = Op.Eq("UsedBy", 0) && Op.Eq("Active", true) && Op.Eq("ApplyToId", commercial_product_id),
                    DeepLoad = true,
                    PageCriteria = new PageCriteria()
                    {
                        Page = 0
                    }

                });

                //Console.WriteLine("Product Name : " + pr.Name + "\n");

                foreach (var ccc in pr.CommercialProductIds)
                {
                    //Console.WriteLine("value = {0}" + ccc.ToString());

                }



                //Console.WriteLine("Start to search all prices for commecial products : " + pr.Name + "\n");

                if (prices == null)
                {
                    continue;
                }

                foreach (var listprice in prices.Items)
                {
                    string msg = "";

                    if (listprice.PriceConditions == null)
                    {
                        continue;
                    }

                    foreach (var pricecondition in listprice.PriceConditions.Items)
                    {
                        msg = "";
                        if (pricecondition.Active == false)
                        {
                            continue;
                        }

                        // Type - OnceOff, Recurrence
                        msg = " Price Type : " + pricecondition.Type + "\n";

                        // Charge Period

                        if (pricecondition.ChargePeriodId != 0)
                        {
                            msg = msg + " Charge Period " + pricecondition.ChargePeriodId + " Month" + "\n";
                        }

                        // The price amount
                        if (pricecondition.PriceAmounts != null)
                        {
                            string priceamount = "";
                            foreach (var pa in pricecondition.PriceAmounts)
                            {
                                priceamount += " Amount : " + pa.Amount + " From " + pa.FromDate + ",";
                            }

                            if (priceamount != "")
                            {
                                msg = msg + " " + priceamount + "\n";
                            }

                            priceamount = "";
                        }


                        // Allowed Agreement Type conditions

                        if (pricecondition.AgreementTypeIds != null)
                        {
                            string agreement_type_list = "";
                            foreach (var agtid in pricecondition.AgreementTypeIds)
                            {
                                agreement_type_list += agcService.GetAgreementType(agtid).Description + ",";

                            }
                            if (agreement_type_list != "")
                            {
                                msg = msg + "Allowed Agreement Types :  " + agreement_type_list + "\n";
                            }
                            agreement_type_list = "";
                        }

                        // Allowed Business Unit Attributes

                        if (pricecondition.BusinessUnitAttributeValues != null)
                        {
                            string business_unit_attribute = "";
                            foreach (var bu in pricecondition.BusinessUnitAttributeValues)
                            {
                                business_unit_attribute += bu + ",";
                            }
                            if (business_unit_attribute != "")
                            {
                                msg = msg + "   Allowed Business Unit Attribute : " + business_unit_attribute + "\n";
                            }
                            business_unit_attribute = "";
                        }

                        // Allowed Charge Period

                        if (pricecondition.ChargePeriodId != null)
                        {
                            string charge_period = "";
                            if (pricecondition.ChargePeriodId == 1)
                            {
                                charge_period = "Monthly";
                            }

                            if (pricecondition.ChargePeriodId == 3)
                            {
                                charge_period = "Quauterly";
                            }

                            if (pricecondition.ChargePeriodId == 6)
                            {
                                charge_period = "HalfYearly";
                            }
                            if (pricecondition.ChargePeriodId == 12)
                            {
                                charge_period = "Yearly";
                            }
                            if (charge_period != "")
                            {
                                msg = msg + "   Allowed Charge Period : " + charge_period + "\n";
                            }
                            charge_period = "";
                        }


                        // Allowed Country

                        if (pricecondition.CountryIds != null)
                        {
                            string country_list = "";
                            foreach (var c in pricecondition.CountryIds)
                            {
                                country_list += custcService.GetCountry(c).Description + ",";
                            }
                            if (country_list != "")
                            {
                                msg = msg + " Allowed Country : " + country_list + "\n";
                            }
                            country_list = "";
                            CountryIds_local = pricecondition.CountryIds;
                        }



                        // Allowed Currency

                        if (pricecondition.CurrencyId != null)
                        {
                            string currency = "";

                            currency = fincService.GetCurrency(pricecondition.CurrencyId.Value).Description;

                            if (currency != "")
                            {
                                msg = msg + "  Allowed Currency : " + currency + "\n";
                            }
                            currency = "";
                        }

                        // Allowed Customer Class

                        if (pricecondition.CustomerClassIds != null)
                        {
                            string customer_class = "";

                            foreach (var ci in pricecondition.CustomerClassIds)
                            {
                                customer_class += custcService.GetCustomerClass(ci).Description + ",";
                            }
                            if (customer_class != "")
                            {
                                msg = msg + "  Allowed Customer Class : " + customer_class + "\n";
                            }
                            customer_class = "";

                            CustomerClassIds_local = pricecondition.CustomerClassIds;
                        }

                        // Allowed Customer Type

                        if (pricecondition.CustomerTypeIds != null)
                        {
                            string customer_type = "";

                            foreach (var ct in pricecondition.CustomerTypeIds)
                            {
                                customer_type += custcService.GetCustomerType(ct).Name + ",";
                            }
                            if (customer_type != null)
                            {
                                msg = msg + "  Allowed Customer Type : " + customer_type + "\n";
                            }
                            customer_type = "";
                            CustomerTypeIds_local = pricecondition.CustomerTypeIds;
                        }


                        // Allowed Finance Options

                        if (pricecondition.FinanceOptionIds != null)
                        {
                            string financeoption = "";

                            foreach (var fo in pricecondition.FinanceOptionIds)
                            {
                                financeoption += productcService.GetFinanceOption(fo).Description + ",";
                            }

                            if (financeoption != "")
                            {
                                msg = msg + " Allowed Finance Options : " + financeoption + "\n";
                            }
                            FinanceOptionIds_local = pricecondition.FinanceOptionIds;
                        }

                        // Allowed Finance Account Type

                        if (pricecondition.FinancialAccountTypeIds != null)
                        {
                            string fatype = "";

                            foreach (var fatypeid in pricecondition.FinancialAccountTypeIds)
                            {
                                fatype += fincService.GetFinancialAccountType((fatypeid)).Description + ",";
                            }

                            if (fatype != "")
                            {
                                msg = msg + "  Allowed Financial Account Type : " + fatype + "\n";
                            }
                        }

                        // Allowed Postal Codes

                        if (pricecondition.PostalCodes != null)
                        {
                            string postcode = "";

                            foreach (var pcode in pricecondition.PostalCodes)
                            {
                                postcode += pcode + ",";
                            }

                            if (postcode != "")
                            {
                                msg = msg + "  Allowed Postal Code : " + postcode + "\n";
                            }
                            PostalCodes_local = pricecondition.PostalCodes;
                        }


                        // Allowed Province

                        if (pricecondition.ProvinceIds != null)
                        {
                            string province = "";

                            foreach (var pro in pricecondition.ProvinceIds)
                            {
                                province += custcService.GetProvince(pro).Description + ",";
                            }

                            if (province != "")
                            {
                                msg = msg + "  Allowed Province : " + province + "\n";
                            }
                            ProvinceIds_local = pricecondition.ProvinceIds;
                        }
                        ListPriceCondition_local = listprice.PriceConditions.Items;
                    }

                }
                
                cc.Add(new YangProductPriceItems
                {
                    the_Name = pr.Name,
                    the_CommercialProductIds = pr.CommercialProductIds,
                    the_ListPriceCollection = prices.Items,
                    the_CustomerClassIds = CustomerClassIds_local,
                    the_CountryIds = CountryIds_local,
                    the_CustomerTypeIds = CustomerTypeIds_local,
                    the_ListPriceConditions = ListPriceCondition_local
                });
            }
            db.the_items = cc;
            return db;

        }
        
        [HttpGet]
        [Route("api/{username_ad}/{password_ad}/product/GetCommercialProductByCustomerId/{id}")]
        public CategoryProductPriceExCollection GetCommercialProductByCustomerId(String username_ad, String password_ad, int id)
        {
            Authentication_class var_auth = new Authentication_class();
            AuthenticationHeader ah = var_auth.getAuthHeader(username_ad, password_ad);
            AsmRepository.SetServiceLocationUrl(var_auth.var_service_location_url);
            var agService = AsmRepository.AllServices.GetAgreementManagementService(ah);
            try
            {
                var agreements = agService.GetAgreements(new BaseQueryRequest()
                {
                    FilterCriteria = Op.Eq("CustomerId", id)
                });
                var map = agService.GetManageAgreementProductReference(agreements.Items[0].Id.Value);
                return map.CategoryProductPriceExCollection;
                //foreach (var cp in map.CategoryProductPriceExCollection)
                //{
                //    Console.WriteLine("Allowed Product Category : " + cp.CommercialProductCategoryName + " | Product Name : " + cp.CommercialProductName + " Price : " + cp.ListPriceAmount);
                //}
                //Console.WriteLine("total product : {0}", map.CategoryProductPriceExCollection.Count());
            }
            catch (Exception ex)
            {
                return null;
                //Console.WriteLine("Errors : " + ex.Message);
                //Console.WriteLine("Stack : " + ex.StackTrace);
            }
        }
        


        [HttpGet]
        [Route("api/{username_ad}/{password_ad}/product/ReconnectProduct/{agreement_detail_id}")]
        public AgreementDetailCollection ReconnectProduct(String username_ad, String password_ad, int agreement_detail_id)
        {
            Authentication_class var_auth = new Authentication_class();
            AuthenticationHeader authHeader = var_auth.getAuthHeader(username_ad, password_ad);
            var agService = AsmRepository.AllServices.GetAgreementManagementService(authHeader);
            var agcService = AsmRepository.AllServices.GetAgreementManagementConfigurationService(authHeader);
            AsmRepository.SetServiceLocationUrl(var_auth.var_service_location_url);
            try
            {
                int[] agreement_details = new int[1];
                var agreement_detail = agService.GetAgreementDetail(agreement_detail_id);
                if (agreement_detail != null)
                {
                    if (agreement_detail.Status.Value == 3 || agreement_detail.Status.Value == 9)
                    {
                        agreement_details[0] = agreement_detail_id;
                        var aa = agService.ReconnectAgreementDetails(agreement_details, 116);
                        return aa;
                        //Console.WriteLine("Reconnect product with id : " + agreement_detail_id + " successfully");
                    }
                    else
                    {
                        return null;
                        //Console.WriteLine("Product (ID : " + agreement_detail_id + ")Status is not Expired or Disconnected, can't been reconnected!");
                    }
                }
                else
                {
                    return null;
                    //Console.WriteLine("Product with id : " + agreement_detail_id + " not exist!");
                }

            }
            catch (Exception ex)
            {
                return null;
                //Console.WriteLine("Can't process reconnect on " + agreement_detail_id);
                //Console.WriteLine("Errors : " + ex.Message);
                //Console.WriteLine("Stack : " + ex.StackTrace);
            }
        }

    }
}
