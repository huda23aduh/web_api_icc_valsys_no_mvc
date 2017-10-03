using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using PayMedia.ApplicationServices.ClientProxy;
using PayMedia.ApplicationServices.SharedContracts;
using PayMedia.ApplicationServices.Devices.ServiceContracts;
using PayMedia.ApplicationServices.Customers.ServiceContracts;
using PayMedia.ApplicationServices.Finance.ServiceContracts;
using PayMedia.ApplicationServices.Finance.ServiceContracts.DataContracts;
using PayMedia.ApplicationServices.Users;
using PayMedia.ApplicationServices.Customers.ServiceContracts.DataContracts;
using System.Collections;
using Newtonsoft.Json.Linq;
using web_api_icc_valsys_no_mvc.Models;
using PayMedia.ApplicationServices.Contacts.ServiceContracts;
using PayMedia.ApplicationServices.Contacts.ServiceContracts.DataContracts;
using PayMedia.ApplicationServices.AgreementManagement.ServiceContracts;
using PayMedia.ApplicationServices.AgreementManagement.ServiceContracts.DataContracts;
using PayMedia.ApplicationServices.ProductCatalog.ServiceContracts;
using PayMedia.ApplicationServices.ProductCatalog.ServiceContracts.DataContracts;
using PayMedia.ApplicationServices.SandBoxManager.ServiceContracts;
using PayMedia.ApplicationServices.Pricing.ServiceContracts;
using PayMedia.ApplicationServices.Pricing.ServiceContracts.DataContracts;
using PayMedia.ApplicationServices.InvoiceRun.ServiceContracts;

using PayMedia.ApplicationServices.CustomFields.ServiceContracts;
using System.Text;

namespace web_api_icc_valsys_no_mvc.Controllers
{
    public class CustomerController : ApiController
    {

        [Route("api/customer/AddOrder")] //JSON EXAMPLE method
        [HttpPost]
        public PurchaseOrder AddOrder([FromBody] PurchaseOrder order)
        {
            return order;
        }

        [Route("api/customer/AddNewCustomerBundle")] //JSON EXAMPLE method to add new customer
        [HttpPost]
        public HttpResponseMessage AddNewCustomerBundle([FromBody]ICC_customer customer_data)
        {
            var period = ChargePeriods.Monthly;

            if (customer_data == null)
            {
                return null;
            }

            DateTime myDate = DateTime.ParseExact(customer_data.BirthDate, "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);


            #region AuthorizationHeader
            //Here we assign our login credentials.
            Authentication_class var_auth = new Authentication_class();
            AuthenticationHeader ah = var_auth.getAuthHeader(customer_data.username_ad, customer_data.password_ad);
            //This is the location of the ASM services.
            AsmRepository.SetServiceLocationUrl(var_auth.var_service_location_url);
            //Here we create proxies for all the services we will need.
            var customersService = AsmRepository.GetServiceProxyCachedOrDefault<ICustomersService>(ah);
            var customersConfigurationService = AsmRepository.GetServiceProxyCachedOrDefault<ICustomersConfigurationService>(ah);
            var financeService = AsmRepository.GetServiceProxyCachedOrDefault<IFinanceService>(ah);
            var financeConfigurationService = AsmRepository.GetServiceProxyCachedOrDefault<IFinanceConfigurationService>(ah);
            var invoiceRunService = AsmRepository.GetServiceProxyCachedOrDefault<IInvoiceRunService>(ah);
            var agreementManagementService = AsmRepository.GetServiceProxyCachedOrDefault<IAgreementManagementService>(ah);
            var pricingService = AsmRepository.GetServiceProxyCachedOrDefault<IPricingService>(ah);
            var productCatalogService = AsmRepository.GetServiceProxyCachedOrDefault<IProductCatalogService>(ah);
            var sandboxService = AsmRepository.GetServiceProxyCachedOrDefault<ISandBoxManagerService>(ah);
            #endregion


            #region Create Customer
            //Configuration controls many default values for a new customer.
            //Call this method to get the defaults. You can pass them in when you create
            //a new customer, or you can override the defaults by passing in different
            //values when you create the new customer.
            CustomerDefaults customerDefault = customersConfigurationService.GetCustomerDefaults();
            //This value is a reason ID for ICC system event 100.
            //You can get this from ICustomersConfigurationService.GetLookups
            //(LookupLists.ReasonCustomerCreate).
            int reasonKey = 0;
            //Instantiate a new Customer.
            Customer customer = new Customer();
            //Configuration controls which values are required. In our environment,
            //customer class and customer type are required, so we pass in the
            //default values here.
            //customer.ClassId = customerDefault.ClassId;
            customer.ClassId = customer_data.ClassId;
            customer.TypeId = customer_data.TypeId;
            customer.SegmentationKey = customer_data.SegmentationId;
            //customer.TypeId = 1;
            customer.BirthDate = myDate;
            customer.ReferenceTypeKey = customer_data.ReferenceTypeKey;
            customer.ReferenceNumber = customer_data.ReferenceNumber;
            customer.LanguageKey = "E";
            customer.BusinessUnitId = getBubyZipcode(customer_data.PostalCode, customer_data);
            

            //This value is a reference number from an external system. You can pass it in,
            //and ICC will store it.

            //Then, you can use it later to find the customer.
            customer.ExternalReference = "AB9829";
            //ICC can store many different types of addresses, but the DefaultAddress
            //is always required when you create a new Customer object.
            customer.DefaultAddress = new DefaultAddress();
            
            customer.DefaultAddress.Email = customer_data.Email;

            //Notice that the customer's name is stored in the DefaultAddress.
            customer.DefaultAddress.PropertyTypeId = customer_data.PropertyTypeId;
            customer.DefaultAddress.CustomerCaptureCategory = (CustomerCaptureCategory)customer_data.CustomerCaptureCategoryId;
            customer.DefaultAddress.FirstName = customer_data.FirstName;
            customer.DefaultAddress.MiddleName = customer_data.MiddleName;
            customer.DefaultAddress.Surname = customer_data.Surname;

            customer.DefaultAddress.BigCity = customer_data.BigCity;
            customer.DefaultAddress.SmallCity = customer_data.SmallCity;
            customer.DefaultAddress.WorkPhone = customer_data.WorkPhone;
            customer.DefaultAddress.Fax1 = customer_data.EmergencyPhone;
            customer.DefaultAddress.HomePhone = customer_data.HomePhone;

            customer.DefaultAddress.Fax2 = customer_data.MobilePhone;
            customer.DefaultAddress.ValidAddressId = customer_data.ValidAddressId;
            customer.DefaultAddress.PropertyTypeId = customer_data.PropertyTypeId;
            customer.DefaultAddress.Street = customer_data.Street;
            customer.DefaultAddress.Extra = customer_data.Address_line2;
            customer.DefaultAddress.ExtraExtra = customer_data.Address_line3;
            customer.DefaultAddress.TitleId = customer_data.TitleId;
            customer.DefaultAddress.CountryId = customer_data.CountryId;
            customer.DefaultAddress.PostalCode = customer_data.PostalCode;
            customer.DefaultAddress.HouseNumberNumeric = customer_data.HouseNumberNumeric;

            customer.DefaultAddress.HouseNumberAlpha = customer_data.HouseNumberAlpha;
            customer.DefaultAddress.FlatOrApartment = customer_data.Flat;
            customer.DefaultAddress.LandMark = customer_data.Landmark;
            customer.DefaultAddress.Directions = customer_data.Directions;

            //NOTE: If your configuration requires a Valid Address, or if it is
            //configured to validate addresses, you must pass in
            //values that correspond to a Valid Address in the database. Otherwise, ICC will
            //throw an error. If your configuration requires ValidAddresses, you can
            //simply pass in a ValidAddressId like this:
            //customer.DefaultAddress.ValidAddressId = 217;
            //This call creates the customer and commits him to the database.
            Customer newCustomer = customersService.CreateCustomer(customer, reasonKey);


            //Test that the customer was created.
            //Console.WriteLine("Customer ID {0}: Name = {1} {2}", newCustomer.Id, newCustomer.DefaultAddress.FirstName, newCustomer.DefaultAddress.Surname, newCustomer.BirthDate, newCustomer.ReferenceNumber);
            //Console.WriteLine("Press Enter to continue.");
            //Console.ReadLine();
            #endregion

            #region emergency field
            var cf = new customFieldHandler(ah, customer_data);
            cf.addAddressCustomField("Emergency Contact Name", newCustomer.DefaultAddress.Id.Value, customer_data.EmergencyContactName);
            cf.addAddressCustomField("Emergency Contact Relationship", newCustomer.DefaultAddress.Id.Value, customer_data.EmergencyContactRelationship);
            cf.addAddressCustomField("Emergency Contact Address", newCustomer.DefaultAddress.Id.Value, customer_data.EmergencyContactAddress);
            cf.addAddressCustomField("GPS latitude and longitude", newCustomer.DefaultAddress.Id.Value, customer_data.GeoLocation);

            #endregion emergency field

            #region Create multiple address
            //address biling
            addAddress(1, customer_data, customer, customer_data.AddressBilling[0].PostalCode, customer_data.AddressBilling[0].Address_line1_Addr_billing, customer_data.AddressBilling[0].Address_line2_Addr_billing, newCustomer.Id.Value, customer_data.AddressBilling[0].MobilePhone_Addr_billing, customer_data.AddressBilling[0].HomePhone_Addr_billing, customer_data.AddressBilling[0].WorkPhone_Addr_billing, customer_data.AddressBilling[0].WorkPhone_Addr_billing, customer_data.AddressBilling[0].Email_Addr_billing, customer_data.AddressBilling[0].Landmark_Addr_billing, customer_data.AddressBilling[0].Directions_Addr_billing);

            //address work order
            addAddress(2, customer_data, customer, customer_data.AddressInstall[0].PostalCode, customer_data.AddressInstall[0].Address_line1_Addr_install, customer_data.AddressInstall[0].Address_line2_Addr_install, newCustomer.Id.Value, customer_data.AddressInstall[0].MobilePhone_Addr_install, customer_data.AddressInstall[0].HomePhone_Addr_install, customer_data.AddressInstall[0].WorkPhone_Addr_install, customer_data.AddressInstall[0].WorkPhone_Addr_install, customer_data.AddressInstall[0].Email_Addr_install, customer_data.AddressInstall[0].Landmark_Addr_install, customer_data.AddressInstall[0].Directions_Addr_install);

            //address agreement
            addAddress(3, customer_data, customer, customer_data.AddressAgreement[0].PostalCode, customer_data.AddressAgreement[0].Address_line1_Addr_agreement, customer_data.AddressAgreement[0].Address_line2_Addr_agreement, newCustomer.Id.Value, customer_data.AddressAgreement[0].MobilePhone_Addr_agreement, customer_data.AddressAgreement[0].HomePhone_Addr_agreement, customer_data.AddressAgreement[0].WorkPhone_Addr_agreement, customer_data.AddressAgreement[0].WorkPhone_Addr_agreement, customer_data.AddressInstall[0].Email_Addr_install, customer_data.AddressAgreement[0].Landmark_Addr_agreement, customer_data.AddressInstall[0].Directions_Addr_install);


            #endregion Create multiple address


            #region Create Financial Account
            //If you ever need to get the default values for a new financial account,
            //you can use these methods.
            //This method returns general financial defaults.
            //FinanceDefaults financeDefault = financeConfigurationService.GetFinanceDefaults();
            //This method returns the default invoice profile ID for a given customer.
            //InvoicingProfile invoicingProfile = invoiceRunService.GetDefaultInvoicingProfile(DateTime.Today, (int)newCustomer.Id,
            //(InvoicingMethod)financeDefault.DefaultInvoiceMethod);
            //This method returns the due date method for a given customer.
            //DueIn dueIn = financeConfigurationService.GetDefaultDueInByCustomerId
            //(int)newCustomer.Id);
            //This value is a reason ID for ICC system event 115.
            //You can get this from IFinancialConfigurationService.GetLookups
            //(LookupLists.FinancialAccountCreateReasons)
            int faReason = 0;
            //Instantiate a new FinancialAccount object.
            FinancialAccount financialAccount = new FinancialAccount();
            //You must pass in the customer ID.
            financialAccount.CustomerId = newCustomer.Id;
            financialAccount.Name = customer_data.FaName;
            
            //ICC uses configured default values to create the financial account.
            //You can override the defaults by passing in other values.
            //Here we override the default InvoicingProfileId.

            //financialAccount.InvoicingProfileId = customer_data.invoicing_profile_method;
            //Here we override the default Due Date Method.
            financialAccount.DueIn = 1;
            //Here we override the default currency.
            financialAccount.CurrencyId = 1;
            
            //Type is used to identify whether the customer buys his products on
            //a pre-paid or post-paid basis.
            financialAccount.TypeId = customerDefault.FinancialAccountType; // 1=prepaid quote, 2=prepaid wallet
            financialAccount.Balance = 10000;
            //financialAccount.BankAccountType = 1;
            financialAccount.AnniversaryMonth = 12;
            financialAccount.ProxyCodeId = 2;
            financialAccount.MopId = customer_data.MOPId;
            
            //Our system has been configured to require additional values when the MOP
            //is credit card, so here we instantiate a new CreditCardCollection
            //to hold those values.
            if (customer_data.tipe_pembayaran == 1) // kondisi cash
            {
                financialAccount.BankName = customer_data.BankName;

            }
            else if (customer_data.tipe_pembayaran == 2) // kondisi credit card
            {
                //If you need to explicitly assign the default Method of Payment ID, you can
                //use this statement:
                //financialAccount.MopId = customerDefault.MethodOfPayment;
                //In this example, we will override the default method of payment
                //to assign ID 1 ("VISA" credit card).
                //financialAccount.MopId = 82;
                
                financialAccount.BankName = customer_data.BankName;
                financialAccount.CreditCard = new CreditCardCollection();
                CreditCard creditcardinfo = new CreditCard();
                financialAccount.CreditCard.Add(creditcardinfo);
                
                //Use the CreditCardDisplay property to pass in the credit card number.
                //In the database, the credit card number is encrypted.
                //Configuration controls whether the credit card number is masked when it
                //is returned in a method call.
                //For information about encryption and masked credit card numbers,
                //see the "API FAQs" section
                //of the ICC API Developer's Guide.
                creditcardinfo.PostalCode = "123456789111222";
                creditcardinfo.CreditCardDisplay = customer_data.CreditCardNumber;
                creditcardinfo.FirstName = customer_data.CreditCardFirstName;
                creditcardinfo.SurName = customer_data.CreditCardLastName;
                creditcardinfo.SecurityCode = customer_data.SecurityCode;
                creditcardinfo.AuthorizationCode = "987";
                creditcardinfo.AuthorizationDate = new DateTime(2017, 1, 18);
                creditcardinfo.ExpirationDate = DateTime.ParseExact(customer_data.ExpirationDate_CC, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);

            }

            else if(customer_data.tipe_pembayaran == 3)
            {
                financialAccount.BankAccountId = customer_data.bank_account_id;
                financialAccount.IBANAccountId = customer_data.IBAN;
                //financialAccount.BankCodeId = customer_data.bank_code;
                //financialAccount.SWIFTCode = customer_data.swift_code;
                financialAccount.InvoicingProfileId = 1;
                //financialAccount.BankBranchId = "";
            }
            //DateTime d = DateTime.Today.Date;
            //if (customer_data.tipe_pembayaran != 3)
            //{
            //    financialAccount.InvoicingProfileId = 30;
            //}

            financialAccount.InvoicingProfileId = Int32.Parse(DateTime.Today.Day.ToString());
            
            financialAccount.ProxyCode = customer_data.ProxyCode;
            financialAccount.ProxyCodeId = customer_data.ProxyCodeId;
            financialAccount.InvoiceDeliveryMethodId = customer_data.invoicing_profile_method; //1=email, 2=printed, 3=sms
            financialAccount.NextInvoiceText = customer_data.Remark;
            //This call creates the financial account and commits it to the database.
            FinancialAccount newFinancialAccount = financeService.CreateFinancialAccount(financialAccount, faReason);
            //Test that the financial account was created.
            //Console.WriteLine("Financial account ID {0}: Account balance = {1} Credit Card number = {2}", newFinancialAccount.Id, newFinancialAccount.Balance, newFinancialAccount.CreditCard.Items[0].CreditCardDisplay);
            //Console.WriteLine("Press Enter to continue.");
            //Console.ReadLine();
            #endregion

            #region Get valid products for customer
            //This cast forces int? newCustomer.Id to be an int.
            int customerId = (int)newCustomer.Id;
            //The products a customer can buy is based on a wide
            //variety of configured values and configured business rules.
            //You can use this call to find out which Products the customer is
            //eligible to buy.
            CategoryProductPriceCollection products = productCatalogService.GetProductsForCustomerId(customerId);
            //Iterate through the list of products the customer can buy.
            //foreach (var item in products)
            //{
            //    #region Pause after a screenful of records
            //    //This snippet pauses when the screen is full. When the user presses
            //    //Enter, the screen clears and another screenful of records is loaded.
            //    if (Console.WindowHeight == Console.CursorTop)
            //    {
            //        Console.WriteLine("Press Enter for another screen of records.");
            //        Console.ReadLine();
            //        Console.Clear();
            //    }
            //    #endregion

            //    Console.WriteLine("Customer can buy {0}, ID {1}", item.CommercialProductName, item.CommercialProductId);
            //}
            //Console.WriteLine("Press Enter to continue.");
            //Console.ReadLine();
            #endregion
            #region Get Pricing
            //The price a customer pays for a product depends on a very large number
            //of conditions, including the customer's address, type, and class,
            //the product's finance option, charge period, and business unit, the currency,
            //the financial account type, the agreement type, and the pricing date
            //(to name a few).
            //To get the correct price for a particular product,
            //you need to know all the details related to the configured conditions.
            //The main methods to get prices are
            // o IPricingService.GetPricing()
            // o IAgreementManagementService.ManageProductCapture()
            // o IAgreementManagementService.GetManageFullProductDetails()
            //The benefit to GetPricing() is that you can use it before the customer
            //has an agreement.
            //For details, see the API Reference Library (CHM file).
            //This example shows how to get the price for a product that was
            //returned by GetProductsForCustomerId().
            //In this example, we will not bother checking for a price override.
            bool checkForOverrides = false;
            //Instantiate the collection and the criteria for the method.
            PricingCriteriaCollection pricingCriteriaCollection = new PricingCriteriaCollection();
            PricingCriteria pricingCriteria = new PricingCriteria();
            //Add the pricing criteria to the collection.
            pricingCriteriaCollection.Add(pricingCriteria);
            #region These properties in this region are required to get a price quote.
            //Choose the object for which you want prices (in this case, a CommercialProduct).
            pricingCriteria.PricingForType = PricingForType.CommercialProduct;
            //Pass in the ID of the product for which you want prices.
            pricingCriteria.PricingForId = 21;
            //You use Pricing Date to select the price that is effective today, or for a date in
            //the future or in the past.
            pricingCriteria.PricingDate = DateTime.Today;
            //Assign the currency of the financial account you created with
            //CreateFinancialAccount()
            pricingCriteria.CurrencyId = newFinancialAccount.CurrencyId;
            //PricingChargeTypes can be recurring, once-off, or both.
            pricingCriteria.PricingChargeType = PricingChargeType.Both;
            //Financial account type controls whether the customer can buy pre-paid or
            //post-paid products.
            pricingCriteria.FinancialAccountTypeId = newFinancialAccount.TypeId;
            //Not required but very important. This property helps prevent
            //a null response for those parameters where a null value means “none.”
            pricingCriteria.NullCriteriaMatchesAll = true;
            #endregion
            //Assign the customer ID you created with CreateCustomer().
            pricingCriteria.CustomerId = newCustomer.Id;
            //You can use this parameter to limit the returned prices to ones that match a
            //configured finance option. In our environment,
            //FinanceOptionId 1 = Rent and FinanceOptionId 2 = Sold.
            if(customer_data.TypeId == 1)
            {
                pricingCriteria.FinanceOptionId = 1;
            }
            else if (customer_data.TypeId == 2)
            {
                pricingCriteria.FinanceOptionId = 2;
            }

            //Call the method to get prices that match the input criteria.
            //For another example of GetPricing(), see the Code Samples in the API
            //Developer's Guide.
            PricingInfoCollection pricingInfo = pricingService.GetPricing(pricingCriteriaCollection, checkForOverrides);
            //Test that you can get prices for the product.
            //foreach (var p in pricingInfo)
            //{
            //    //We are writing a lot on info here, only so you can get an idea
            //    //of how prices can vary due to different conditions.
            //    Console.WriteLine("Product ID = {0}, Invoice Text = {1}",
            //    p.PricingForId, p.InvoiceText);
            //    Console.WriteLine("Price = {0}, Price Condition ID = {1}",
            //    p.ListPriceAmount, p.ListPriceConditionId);
            //    Console.WriteLine("Event ID = {0}, Reason ID = {1}, Charge Type = {2}",
            //    p.EventId, p.EventReasonId, p.PricingChargeType);
            //    Console.WriteLine();
            //}
            //Console.WriteLine("Press Enter to continue.");
            //Console.ReadLine();
            #endregion
            #region Create Agreement
            //This value is a reason ID for ICC system event 960.
            //You can get this from IAgreementManagementConfigurationService.GetLookups
            //(LookupLists.AgreementCreationEditReasons)
            int agReason = 0;
            //Instantiate a new Agreement.
            Agreement agreement = new Agreement();
            //Required. Assign the financial account ID to the agreement.
            agreement.FinancialAccountId = newFinancialAccount.Id;
            //Required. The default frequency for generating recurring charges
            //for products on the agreement. AgreementDetails on this agreement
            //inherit this value.



            if(customer_data.charge_period.ToLower() == "monthly")
                period = ChargePeriods.Monthly;
            else if (customer_data.charge_period.ToLower() == "halfyearly")
                period = ChargePeriods.HalfYearly;
            else if (customer_data.charge_period.ToLower() == "quarterly")
                period = ChargePeriods.Quarterly;
            else if (customer_data.charge_period.ToLower() == "yearly")
                period = ChargePeriods.Yearly;
            else if (customer_data.charge_period.ToLower() == "none")
                period = ChargePeriods.None;

            agreement.ChargePeriod = period;
            
            //The default Type is a configured value. You can override the default by passing
            //in a new value here.
            agreement.Type = 1;
            //A configured value that represents the duration of the agreement.
            agreement.ContractPeriodId = 12;
            //Market segment is derived from the customer's service address.
            //You can override the value, but it is not recommended because the choice
            //of available products can depend on market segment configuration.
            //agreement.MarketSegmentId = 1;
            //This call creates the agreement and commits it to the database.
            Agreement newAgreement = agreementManagementService.CreateAgreement(agreement, agReason);
            //Test that the agreement was created.
            //Console.WriteLine("Agreement ID {0}: Market Segment ID = {1}", newAgreement.Id, newAgreement.MarketSegmentId);
            //Console.WriteLine("Press Enter to continue.");
            //Console.ReadLine();
            #endregion

            #region add_commercial_product
            //int alert = 0;
            //int i = 0;
            //for (i = 0; i < customer_data.the_total_com_prod; i++)
            //{
            //    if (customer_data.the_list_com_prod_id[i] == 1) // add hardware
            //    {
            //        alert = addCommercialProduct(customer_data.the_list_com_prod_id[i], newCustomer.Id.Value, 3);
            //    }
            //    else // add software
            //        alert = addCommercialProduct(customer_data.the_list_com_prod_id[i], newCustomer.Id.Value, 2);
            //}
            #endregion add_commercial_product

            #region add_commercial_product_single_quote
            int alert = 0;
            int i = 0;

            List<int> offers = new List<int>();

            #region add offer/promo
            for (i = 0; i < customer_data.the_total_promo; i++)
            {
                offers.Add(customer_data.the_list_promo[i]);
            }
            #endregion add offer/promo

            alert = addCommercialProductsSingleQuote(customer_data, newCustomer.Id.Value, period, offers);
            #endregion add_commercial_product

            #region Create product, device links, shipping order, and work order.
            ////Before you can sell a product to a customer, the customer MUST have a
            ////financial account and an agreement.
            ////Create the sandbox workspace. A sandbox is required when
            ////you create a product.
            //int sandboxId = sandboxService.CreateSandbox();
            ////Instantiate an object for the product capture (input) parameters.
            //ProductCaptureParams productCaptureParams = new ProductCaptureParams();
            ////Assign the sandbox ID to the ProductCaptureParams.
            //productCaptureParams.SandboxId = sandboxId;
            ////Assign the customer's agreement to the new product.
            //productCaptureParams.AgreementId = newAgreement.Id;
            ////To assign an agreement-level Offer, pass in the
            ////OfferDefinitionId(s) here.
            ////To assign product-level Offers, pass their IDs in the
            ////AgreementDetailWithOfferDefinitions.ProductCaptureOfferInfos property.
            ////This line of code is commented out here because in this
            ////example, the Offer is assigned at the product-level.
            ////productCaptureParams.OfferDefinitions = new List<int>(31);
            ////If False, ICC creates shipping orders that have been configured for
            ////the TechnicalProducts that make up the selected CommercialProduct.
            //productCaptureParams.SkipShippingOrderGeneration = false;
            ////If False, ICC creates work orders that have been configured for
            ////the TechnicalProducts that make up the selected CommercialProduct.
            //productCaptureParams.SkipWorkOrderGeneration = false;
            ////Valid only when a distributor owns the existing devices at the
            ////customer's address. If True, the system attempts to find
            ////distributor-owned devices at the associated address, and
            ////automatically links the existing hardware to the DPADs
            ////created during the capture process.
            //productCaptureParams.DevicesOnHand = false;
            ////Here we instantiate an object for the reason associated with this
            ////product's capture.
            //productCaptureParams.CaptureReasons = new ProductCaptureReasons();
            ////In this example, we are creating a new product, so we assign a reason ID
            ////that is valid for ICC system event 120.
            //productCaptureParams.CaptureReasons.CreateReason = 0;
            ////The next set of values set up the customer's product and, optionally,
            ////product-level offers.
            ////NOTE: An AgreementDetail is an instance of a CommercialProduct that is linked to
            ////a specific customer. (CommercialProducts are the things you sell. When you sell one
            ////to a customer, the particular instance that is linked to a customer is called
            ////an AgreementDetail.)
            //productCaptureParams.AgreementDetailWithOfferDefinitions = new AgreementDetailWithOfferDefinitionsCollection
            //{
            //    new AgreementDetailWithOfferDefinitions
            //    {
            //        AgreementDetail = new AgreementDetail
            //        {
            //            //Not required but highly recommended.
            //            //This property is valid only when you create
            //            //a new AgreementDetail. If your users have configured a Default
            //            //Disconnection Setting in the Configuration Module, you
            //            //should set this property to
            //            //UseDefaultProductDisconnectionsSettingOnCreate.
            //            //This will ensure that ICC uses the configured Disconnection
            //            //Settings on the new product that you are creating.
            //            DisconnectionSetting = PayMedia.ApplicationServices.AgreementManagement.ServiceContracts.DisconnectionDateSettings.UseDefaultProductDisconnectionSettingOnCreate,
            //            //Replace this value with the ID of the address
            //            //where the product will be used.
            //            AddressId = newCustomer.DefaultAddress.Id,
            //            //Set this value to define whether a dealer can receive
            //            //commission for selling this product.
            //            CommissionOption=CommissionOptionTypes.NoCommission,
            //            //Replace this value with the customer's ID.
            //            CustomerId = newCustomer.Id,
            //            //This value is the Agreement to which the product
            //            //will be linked.
            //            AgreementId = productCaptureParams.AgreementId,
            //            //Choose the value that ICC will use when creating
            //            //charges for the product.
            //            ChargePeriod = ChargePeriods.Monthly,
            //            //ICC uses this property during an invoice run to determine
            //            //whether to create charges for the product.
            //            //If you don't specify this value, ICC defaults the value to
            //            //yesterday (which is the correct value to use when creating
            //            //a new product).
            //            ChargeUntilDate = DateTime.Now.AddDays(-1),
            //            //Replace this value with the ID of the CommercialProduct that
            //            //the customer is buying.
            //            CommercialProductId = 21,
            //            //You can use the agreement's contract period, or you can
            //            //enter a different value (depending on your company's business rules).
            //            ContractPeriodId = newAgreement.ContractPeriodId,
            //            //Replace this value with the start date of the contract.
            //            //It can be the same or different from the Agreement start date.
            //            ContractStartDate=newAgreement.ContractStartDate,
            //            //Replace this value with the end date of the contract.
            //            //It can be the same or different from the Agreement end date.
            //            ContractEndDate=newAgreement.ContractEndDate,
            //            //If True, indicates that the product includes a hardware
            //            //device, and ICC will create the necessary DPAD (Device Per Agreement
            //            //Detail) records for you.
            //            DeviceIncluded = true,
            //            //Used only when a customer is linked to a Distributor.
            //            //If True, ICC will attempt to find distributor-owned devices
            //            //at the customer's address, and will automatically link the
            //            //devices to the DPADs that are created during the transaction.
            //            DevicesOnHand = false,
            //            //Replace this value with the ID of a FinanceOption that is
            //            //valid in your environment. We are using the same Rent finance option
            //            //that we used when we called GetPricing().
            //            FinanceOptionId = pricingCriteria.FinanceOptionId,
            //            //Assign the customer's financial account ID to the new AgreementDetail.
            //            FinancialAccountId = newFinancialAccount.Id,
            //            //Replace this value with the number of this product (ID = 21) that
            //            //the customer wants to buy.
            //            Quantity = 1
            //        },
            //        //Use this section if you want to apply a product-level
            //        //offer to the new product.
            //        ProductCaptureOfferInfos = new ProductCaptureOfferInfoCollection
            //        {
            //            new ProductCaptureOfferInfo
            //            {
            //                //Replace this value with a valid Offer ID.
            //                AppliedOfferDefinitionId = 22
            //            }
            //        },
            //        //Use this section if you want to override the user-configured
            //        //price for this product.
            //        PriceOverrides = new PriceOverrideExCollection
            //        {
            //            new PriceOverrideEx
            //            {
            //                //Replace this value with the ID of the list price condition
            //                //that you want to override. It must be a valid condition ID
            //                //for the product you are creating.
            //                ListPriceConditionId = 61,
            //                //ICC uses this enum, together with Amount, to calculate
            //                //the override price.
            //                OverrideType = AgreementOverrideTypes.PercentOff,
            //                //ICC uses this value, together with OverrideType,
            //                //to calculate the override price. In this example,
            //                //the override price will be 12% off the regular list price.
            //                Amount = 12,
            //                //Replace this value with the ID of a valid reason
            //                //for event 5231, the Create Price Override event.
            //                CreationReason = 0,
            //                //This text appears on FTs and invoices to describe
            //                //the charge.
            //                OverrideInvoiceText = "Your Special Price",
            //                //Replace this value with a ledger account ID that is
            //                //valid in your environment.
            //                OverrideLedgerAccountId = 711
            //            }
            //        }
            //    }
            //};

            ////Call the method to create the AgreementDetail, device links, shipping order,
            ////and work order.
            //ProductCaptureResults newProduct = agreementManagementService.ManageProductCapture(productCaptureParams);
            //if (newProduct.AgreementDetails.Items.Count == 0)
            //{
            //    //Console.WriteLine("Something bad happened. Sandbox rolled back.");
            //    //Console.ReadLine();
            //    //The "false" value in this line of code rolls back the sandbox.
            //    sandboxService.FinalizeSandbox(sandboxId, false);
            //    //return;
            //}
            ////The "true" value in this line of code commits the sandbox.
            //sandboxService.FinalizeSandbox(sandboxId, true);
            ////foreach (var item in newProduct.AgreementDetails)
            ////{
            ////    Console.WriteLine("Customer purchased Commercial Product ID {0}; AgreementDetail ID = { 1}", item.CommercialProductId, item.Id);
            ////}
            #endregion Create product, device links, shipping order, and work order.
            //This keeps the console window open until the user presses Enter.
            //Console.ReadLine();


            //return newCustomer;

            if (newCustomer != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, newCustomer);
            }
            else
            {
                var message = string.Format("error");
                HttpError err = new HttpError(message);
                return Request.CreateResponse(HttpStatusCode.OK, message);
            }

        }
        static void addAddress(int address_type, ICC_customer customer_data_json, Customer customer_param, string postcode, string addr1, string addr2, int customerid, string mobile, string homephone, string workphone, string emergency_phone, string email, string landmark, string directions)
        {
            Authentication_class var_auth = new Authentication_class();
            AuthenticationHeader ah = var_auth.getAuthHeader(customer_data_json.username_ad, customer_data_json.password_ad);
            //This is the location of the ASM services.
            AsmRepository.SetServiceLocationUrl(var_auth.var_service_location_url);

            ICustomersService custService = null;
            ICustomersConfigurationService custcService = null;
            custService = AsmRepository.AllServices.GetCustomersService(ah);
            custcService = AsmRepository.AllServices.GetCustomersConfigurationService(ah);

            var va = custcService.GetValidAddresses(new BaseQueryRequest()
            {
                FilterCriteria = new CriteriaCollection() {
                    new Criteria() {
                        Key = "PostalCode",
                        Operator=Operator.Equal,
                         Value = postcode
                    }
                }

            });

            Address addr = new Address();
            addr.Street = va.Items[0].Street;
            addr.BigCity = va.Items[0].BigCity;
            addr.SmallCity = va.Items[0].SmallCity;
            addr.CustomerId = customerid;
            addr.Fax1 = emergency_phone;
            //addr.CustomFields = 
            if (address_type == 1)
            {
                // Billing address
                addr.Type = AddressTypes.Billing;
            }
            else if (address_type == 2)
            {
                // Billing address
                addr.Type = AddressTypes.WorkOrder;
            }
            else if (address_type == 3)
            {
                // Billing address
                addr.Type = AddressTypes.Agreement;
            }


            // Work Order Address
            //addr.Type = AddressTypes.WorkOrder;
            addr.FirstName = customer_param.DefaultAddress.FirstName;
            addr.Surname = customer_param.DefaultAddress.Surname;
            addr.MiddleName = customer_param.DefaultAddress.MiddleName;
            addr.Email = email;
            addr.CountryId = va.Items[0].CountryId;
            addr.ProvinceKey = va.Items[0].ProvinceId;
            addr.Extra = addr1;
            addr.ExtraExtra = addr2;
            addr.Fax2 = mobile;
            addr.HomePhone = homephone;
            addr.WorkPhone = workphone;
            addr.PropertyTypeId = 1;
            addr.TitleId = 1;
            addr.EntityId = 1;
            addr.ValidAddressId = va.Items[0].Id;
            addr.Directions = directions;
            addr.LandMark = landmark;

            custService.CreateAddress(addr, 0);

        }
        public class customFieldHandler
        {
            ICustomFieldsService custfService = null;
            ICustomersService custService = null;

            public customFieldHandler(AuthenticationHeader authHeader, ICC_customer customer_json_param)
            {
                Authentication_class var_auth = new Authentication_class();
                AuthenticationHeader ah = var_auth.getAuthHeader(customer_json_param.username_ad, customer_json_param.password_ad);
                //This is the location of the ASM services.
                AsmRepository.SetServiceLocationUrl(var_auth.var_service_location_url);

                custfService = AsmRepository.AllServices.GetCustomFieldsService(ah);
                custService = AsmRepository.AllServices.GetCustomersService(ah);
            }


            public void addAddressCustomField(string field_name, int address_id, string field_value)
            {

                var c = custfService.GetCustomFieldValues(address_id, 2);


                if (c.Find(c1 => (c1.Name == field_name && c1.Value != null)) != null)
                {
                    custfService.UpdateCustomFieldValues(address_id, 2, new CustomFieldValueCollection()
                    {
                        new CustomFieldValue()
                        {
                            Name = field_name,
                            Value = field_value
                        }
                    });
                }
                else
                {
                    custfService.CreateCustomFieldValues(address_id, 2, new CustomFieldValueCollection(){
                        new CustomFieldValue()
                            {
                                Name = field_name,
                                Value = field_value
                            }
                    });
                }
            }
        }
        //static int addCommercialProduct(int commercial_product_id, int customer_id, int financeoption)
        //{

        //    try
        //    {
        //        Authentication_class var_auth = new Authentication_class();
        //        AuthenticationHeader authHeader = var_auth.getAuthHeader();
        //        AsmRepository.SetServiceLocationUrl("http://192.168.177.4/asm/all/servicelocation.svc");


        //        IAgreementManagementService agService = null;
        //        IAgreementManagementConfigurationService agcService = null;
        //        IProductCatalogService productService = null;
        //        IProductCatalogConfigurationService productcService = null;
        //        ICustomersConfigurationService custcService = null;
        //        IFinanceConfigurationService fincService = null;
        //        ISandBoxManagerService sandboxService = null;
        //        ICustomersService custService = null;


        //        agService = AsmRepository.AllServices.GetAgreementManagementService(authHeader);
        //        agcService = AsmRepository.AllServices.GetAgreementManagementConfigurationService(authHeader);
        //        productService = AsmRepository.AllServices.GetProductCatalogService(authHeader);
        //        productcService = AsmRepository.AllServices.GetProductCatalogConfigurationService(authHeader);
        //        custcService = AsmRepository.AllServices.GetCustomersConfigurationService(authHeader);
        //        fincService = AsmRepository.AllServices.GetFinanceConfigurationService(authHeader);
        //        sandboxService = AsmRepository.AllServices.GetSandBoxManagerService(authHeader);
        //        custService = AsmRepository.AllServices.GetCustomersService(authHeader);

        //        int sandbox_id = sandboxService.CreateSandbox();
        //        var commercial_product = productcService.GetCommercialProduct(commercial_product_id);
        //        if (commercial_product == null)
        //        {
        //            //Console.WriteLine("Commercial Product with ID : " + commercial_product_id + " Not Exist!!!");
        //            return 0;
        //        }

        //        int business_unit_id = custService.GetCustomer(customer_id).BusinessUnitId.Value;

        //        int reason120 = 65;
        //        int agreement_id = 0;
        //        var agreements = agService.GetAgreementsForCustomerId(customer_id, 1);

        //        if (agreements.Items.Count > 0)
        //        {
        //            agreement_id = agreements.Items[0].Id.Value;

        //            ProductCaptureParams param = new ProductCaptureParams()
        //            {
        //                SandboxId = sandbox_id,
        //                AgreementId = agreement_id,
        //                CaptureReasons = new ProductCaptureReasons()
        //                {
        //                    CreateReason = reason120
        //                },
        //                OfferDefinitions = null,
        //                SkipWorkOrderGeneration = true,
        //                SkipShippingOrderGeneration = true,
        //                AgreementDetailWithOfferDefinitions = new AgreementDetailWithOfferDefinitionsCollection()
        //                {
        //                    new AgreementDetailWithOfferDefinitions()
        //                    {
        //                        AgreementDetail = new AgreementDetail()
        //                        {
        //                            CustomerId = customer_id,
        //                            AgreementId = agreement_id,
        //                            ChargePeriod = ChargePeriods.Monthly,
        //                            CommercialProductId = commercial_product_id,
        //                            // DevicesOnHand : For software product, put false, for hardware product, put true.
        //                            DevicesOnHand = false,
        //                            // Quantity : Please do not put number more than 1. It must be 1 only.
        //                            Quantity = 1,  
        //                            // FinanceOptionId : 2 is Subscription for software product, if you need add hardware product, you need use 1 (Sold) or 3 (Rent).
        //                            FinanceOptionId = financeoption,
        //                            BusinessUnitId = business_unit_id,
        //                            DisconnectionSetting = PayMedia.ApplicationServices.AgreementManagement.ServiceContracts.DisconnectionDateSettings.QuoteBased
        //                        },
        //                    }
        //                }

        //            };

        //            var result = agService.ManageProductCapture(param);

        //            if (result.AgreementDetails.Items.Count == 0)
        //            {
        //                //Console.WriteLine("Warnings : " + result.WarningMessage);

        //                //Console.WriteLine("Something bad happened. Sandbox rolled back.");

        //                //The "false" value in this line of code rolls back the sandbox.
        //                sandboxService.FinalizeSandbox(sandbox_id, false);
        //                return 0;

        //            }
        //            else
        //            {
        //                //The "true" value in this line of code commits the sandbox.
        //                sandboxService.FinalizeSandbox(sandbox_id, true);

        //                foreach (var item in result.AgreementDetails)
        //                {
        //                    //Console.WriteLine("Congratulations! New Product ID = {0}.", item.Id);
        //                }
        //                return result.AgreementDetails.Items[0].Id.Value;
        //            }
        //        }
        //        else
        //        {
        //            //Console.WriteLine("Can't find agreement for customer id : " + customer_id);
        //            return 0;
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        //Console.WriteLine("Errors : " + ex.Message);
        //        //Console.WriteLine("Exception Stack : " + ex.StackTrace);
        //        return 0;
        //    }

        //}
        static int addCommercialProductsSingleQuote(ICC_customer customer_data_json, int customer_id, ChargePeriods param_periods, List<int> offers)
        {
            try
            {
                Authentication_class var_auth = new Authentication_class();
                AuthenticationHeader authHeader = var_auth.getAuthHeader(customer_data_json.username_ad, customer_data_json.password_ad);
                AsmRepository.SetServiceLocationUrl(var_auth.var_service_location_url);
                
                IAgreementManagementService agService = null;
                IAgreementManagementConfigurationService agcService = null;
                IProductCatalogService productService = null;
                IProductCatalogConfigurationService productcService = null;
                ICustomersConfigurationService custcService = null;
                IFinanceConfigurationService fincService = null;
                ISandBoxManagerService sandboxService = null;
                ICustomersService custService = null;


                agService = AsmRepository.AllServices.GetAgreementManagementService(authHeader);
                agcService = AsmRepository.AllServices.GetAgreementManagementConfigurationService(authHeader);
                productService = AsmRepository.AllServices.GetProductCatalogService(authHeader);
                productcService = AsmRepository.AllServices.GetProductCatalogConfigurationService(authHeader);
                custcService = AsmRepository.AllServices.GetCustomersConfigurationService(authHeader);
                fincService = AsmRepository.AllServices.GetFinanceConfigurationService(authHeader);
                sandboxService = AsmRepository.AllServices.GetSandBoxManagerService(authHeader);
                custService = AsmRepository.AllServices.GetCustomersService(authHeader);

                int sandbox_id = sandboxService.CreateSandbox();
                
                
                //var commercial_product = productcService.GetCommercialProduct(commercial_product_id);
                //if (commercial_product == null)
                //{
                //    //Console.WriteLine("Commercial Product with ID : " + commercial_product_id + " Not Exist!!!");
                //    return 0;
                //}

                int business_unit_id = custService.GetCustomer(customer_id).BusinessUnitId.Value;

                int first_prod = 0;
                int reason120 = 65;
                int agreement_id = 0;
                var agreements = agService.GetAgreementsForCustomerId(customer_id, 1);

                if (agreements.Items.Count > 0)
                {
                    agreement_id = agreements.Items[0].Id.Value;

                    AgreementDetailWithOfferDefinitionsCollection productList = new AgreementDetailWithOfferDefinitionsCollection();
                    ProductCaptureOfferInfoCollection offerInfos = new ProductCaptureOfferInfoCollection();

                    for (int i = 0; i < customer_data_json.the_total_com_prod; i++)
                    {
                        // for product (hardware)
                        if (
                            customer_data_json.the_list_com_prod_id[i] == 1 || customer_data_json.the_list_com_prod_id[i] == 2 ||
                            customer_data_json.the_list_com_prod_id[i] == 199 || customer_data_json.the_list_com_prod_id[i] == 200 ||
                            customer_data_json.the_list_com_prod_id[i] == 308 || customer_data_json.the_list_com_prod_id[i] == 309
                          )
                        {

                            productList.Add(new AgreementDetailWithOfferDefinitions()
                            {
                                AgreementDetail = new AgreementDetail()
                                {
                                    CustomerId = customer_id,
                                    AgreementId = agreement_id,
                                    Segmentation = customer_data_json.the_segmentation_list[i],
                                    
                                    ChargePeriod = param_periods,

                                    CommercialProductId = customer_data_json.the_list_com_prod_id[i],    // comm_prod_id
                                    DeviceIncluded = true,
                                    DevicesOnHand = false,
                                    Quantity = 1,
                                    FinanceOptionId = customer_data_json.the_finance_option_id_list[i],
                                    //FinanceOptionId = 3,   // FinanceOptionId : 2 is Subscription for software product, if you need add hardware product, you need use 1 as sold, 3 as rent
                                    BusinessUnitId = business_unit_id,
                                    //DisconnectionSetting = PayMedia.ApplicationServices.AgreementManagement.ServiceContracts.DisconnectionDateSettings.QuoteBased
                                    DisconnectionSetting = PayMedia.ApplicationServices.AgreementManagement.ServiceContracts.DisconnectionDateSettings.AccountBased
                                },
                            });
                        }

                        // for product (software)
                        else
                        {
                            if (first_prod == 0)
                            {
                                foreach (var zz in offers)
                                {
                                    offerInfos.Add(new ProductCaptureOfferInfo() { AppliedOfferDefinitionId = zz });
                                }
                            }
                            else

                                offerInfos = null;
                            productList.Add(new AgreementDetailWithOfferDefinitions()
                            {

                                AgreementDetail = new AgreementDetail()
                                {
                                    CustomerId = customer_id,
                                    AgreementId = agreement_id,


                                    ChargePeriod = param_periods,
                                    Segmentation = customer_data_json.the_segmentation_list[i],
                                    CommercialProductId = customer_data_json.the_list_com_prod_id[i],   // comm_prod_id
                                    DeviceIncluded = true,
                                    DevicesOnHand = false,
                                    Quantity = 1,
                                    FinanceOptionId = 2,   // FinanceOptionId : 2 is Subscription for software product, if you need add hardware product, you need use 1 as sold, 3 as rent
                                    BusinessUnitId = business_unit_id,
                                    DisconnectionSetting = PayMedia.ApplicationServices.AgreementManagement.ServiceContracts.DisconnectionDateSettings.QuoteBased
                                },
                                ProductCaptureOfferInfos = offerInfos
                            });
                            first_prod = 1;
                        }
                    }


                    ProductCaptureParams param = new ProductCaptureParams()
                    {
                        SandboxId = sandbox_id,
                        AgreementId = agreement_id,
                        CaptureReasons = new ProductCaptureReasons()
                        {
                            CreateReason = reason120
                        },
                        //OfferDefinitions = offers,
                        SkipWorkOrderGeneration = true,
                        SkipShippingOrderGeneration = false,
                        AgreementDetailWithOfferDefinitions = productList
                        //AgreementDetailWithOfferDefinitions = new AgreementDetailWithOfferDefinitionsCollection()
                        //{
                        //    new AgreementDetailWithOfferDefinitions()
                        //    {
                        //        AgreementDetail = new AgreementDetail()
                        //        {
                                   
                        //            CustomerId = customer_id,
                        //            AgreementId = agreement_id,
                        //            ChargePeriod = ChargePeriods.Monthly,
                        //            CommercialProductId = commercial_product_id,
                        //            // DevicesOnHand : For software product, put false, for hardware product, put true.
                        //            DevicesOnHand = false,
                        //            // Quantity : Please do not put number more than 1. It must be 1 only.
                        //            Quantity = 1,  
                        //            // FinanceOptionId : 2 is Subscription for software product, if you need add hardware product, you need use 1 (Sold) or 3 (Rent).
                        //            FinanceOptionId = financeoption,
                        //            BusinessUnitId = business_unit_id,
                        //            DisconnectionSetting = PayMedia.ApplicationServices.AgreementManagement.ServiceContracts.DisconnectionDateSettings.QuoteBased
                        //        },
                        //    }
                        //}

                    };

                    

                    var result = agService.ManageProductCapture(param);
                    

                    if (result.AgreementDetails.Items.Count == 0)
                    {
                        //Console.WriteLine("Warnings : " + result.WarningMessage);

                        //Console.WriteLine("Something bad happened. Sandbox rolled back.");

                        //The "false" value in this line of code rolls back the sandbox.
                        sandboxService.FinalizeSandbox(sandbox_id, false);

                        return 0;

                    }
                    else
                    {
                        //The "true" value in this line of code commits the sandbox.
                        sandboxService.FinalizeSandbox(sandbox_id, true);
                        
                        foreach (var item in result.AgreementDetails)
                        {
                            //Console.WriteLine("Congratulations! New Product ID = {0}.", item.Id);
                        }
                        return result.AgreementDetails.Items[0].Id.Value;
                    }
                }
                else
                {
                    //Console.WriteLine("Can't find agreement for customer id : " + customer_id);
                    return 0;
                }

            }
            catch (Exception ex)
            {
                //Console.WriteLine("Errors : " + ex.Message);
                //Console.WriteLine("Exception Stack : " + ex.StackTrace);
                return 0;
            }

        }

        public int getBubyZipcode(string postcode, ICC_customer customer_data_json)
        {
            Authentication_class var_auth = new Authentication_class();
            AuthenticationHeader ah = var_auth.getAuthHeader(customer_data_json.username_ad, customer_data_json.password_ad);
            //This is the location of the ASM services.
            AsmRepository.SetServiceLocationUrl(var_auth.var_service_location_url);

            int buid = 0;
            var buService = AsmRepository.AllServices.GetBusinessUnitsService(ah);
            var bucService = AsmRepository.AllServices.GetBusinessUnitsConfigurationService(ah);
            var bus = buService.GetBusinessUnits(new AddressCriteria()
            {
                CountryId = 1,
                PostalCode = postcode
            });

            foreach (var bu in bus)
            {

                var businessunit = bucService.GetBusinessUnitById(Int32.Parse(bu.Key.ToString()));
                if (businessunit.Attributes.Items.Count == 4)
                {
                    buid = businessunit.Id.Value;
                    Console.WriteLine("Business Unit : " + businessunit.Description);
                    break;
                }

            }
            return buid;

        }

        [HttpPost]
        [Route("api/customer/AddNewhardwareproductadncreateshippingorder")] //JSON EXAMPLE method to add new customer
        public HttpResponseMessage AddNewhardwareproductadncreateshippingorder([FromBody]NewHw the_hw_param)
        {
            bool is_succes = false;

            List<int> offers = null;



            int customer_id = the_hw_param.customer_id; //500057120
            int agreement_id = 0;
            int fa_id = the_hw_param.fa_id;

            Authentication_class var_auth = new Authentication_class();
            AuthenticationHeader ah = var_auth.getAuthHeader(the_hw_param.username_ad, the_hw_param.password_ad);
            AsmRepository.SetServiceLocationUrl(var_auth.var_service_location_url);

            var customersService = AsmRepository.GetServiceProxyCachedOrDefault<ICustomersService>(ah);
            var customersConfigurationService = AsmRepository.GetServiceProxyCachedOrDefault<ICustomersConfigurationService>(ah);
            var financeService = AsmRepository.GetServiceProxyCachedOrDefault<IFinanceService>(ah);
            var financeConfigurationService = AsmRepository.GetServiceProxyCachedOrDefault<IFinanceConfigurationService>(ah);
            var invoiceRunService = AsmRepository.GetServiceProxyCachedOrDefault<IInvoiceRunService>(ah);
            var agreementManagementService = AsmRepository.GetServiceProxyCachedOrDefault<IAgreementManagementService>(ah);
            var pricingService = AsmRepository.GetServiceProxyCachedOrDefault<IPricingService>(ah);
            var productCatalogService = AsmRepository.GetServiceProxyCachedOrDefault<IProductCatalogService>(ah);
            var sandboxService = AsmRepository.GetServiceProxyCachedOrDefault<ISandBoxManagerService>(ah);

            var agService = AsmRepository.AllServices.GetAgreementManagementService(ah);
            var agcService = AsmRepository.AllServices.GetAgreementManagementConfigurationService(ah);
            var productService = AsmRepository.AllServices.GetProductCatalogService(ah);
            var productcService = AsmRepository.AllServices.GetProductCatalogConfigurationService(ah);
            var custcService = AsmRepository.AllServices.GetCustomersConfigurationService(ah);
            var fincService = AsmRepository.AllServices.GetFinanceConfigurationService(ah);
            var custService = AsmRepository.AllServices.GetCustomersService(ah);

            try
            {
                int sandbox_id = sandboxService.CreateSandbox();

                int business_unit_id = custService.GetCustomer(customer_id).BusinessUnitId.Value;

                int reason120 = 65;

                var agreements = agService.GetAgreementsForCustomerId(customer_id, 1);

                if (agreements.Items.Count > 0)
                {
                    agreement_id = agreements.Items[0].Id.Value;

                    AgreementDetailWithOfferDefinitionsCollection productList = new AgreementDetailWithOfferDefinitionsCollection();
                    for (int i = 0; i < the_hw_param.the_total_com_prod; i++) //total product
                    {
                        if (
                            the_hw_param.the_list_com_prod_id[i] == 1 || the_hw_param.the_list_com_prod_id[i] == 2 ||
                            the_hw_param.the_list_com_prod_id[i] == 199 || the_hw_param.the_list_com_prod_id[i] == 200 ||
                            the_hw_param.the_list_com_prod_id[i] == 308
                          ) // for product (hardware) with id 1
                        {
                            productList.Add(new AgreementDetailWithOfferDefinitions()
                            {
                                AgreementDetail = new AgreementDetail()
                                {
                                    CustomerId = customer_id,
                                    AgreementId = agreement_id,
                                    ChargePeriod = ChargePeriods.Monthly,
                                    Segmentation = the_hw_param.the_segmentation_list[i],
                                    CommercialProductId = the_hw_param.the_list_com_prod_id[i],    // comm_prod_id
                                    DeviceIncluded = true,
                                    DevicesOnHand = false,
                                    Quantity = 1,
                                    //FinanceOptionId = 3,   // FinanceOptionId : 2 is Subscription for software product, if you need add hardware product, you need use 1 as sold, 3 as rent
                                    FinanceOptionId = the_hw_param.the_finance_option_id_list[i],
                                    BusinessUnitId = business_unit_id,
                                    //DisconnectionSetting = PayMedia.ApplicationServices.AgreementManagement.ServiceContracts.DisconnectionDateSettings.QuoteBased
                                    DisconnectionSetting = PayMedia.ApplicationServices.AgreementManagement.ServiceContracts.DisconnectionDateSettings.AccountBased
                                },
                            });
                        }
                        else
                        {
                            productList.Add(new AgreementDetailWithOfferDefinitions()
                            {

                                AgreementDetail = new AgreementDetail()
                                {
                                    CustomerId = customer_id,
                                    AgreementId = agreement_id,
                                    ChargePeriod = ChargePeriods.Monthly,
                                    Segmentation = the_hw_param.the_segmentation_list[i],
                                    CommercialProductId = the_hw_param.the_list_com_prod_id[i],   // comm_prod_id
                                    DeviceIncluded = true,
                                    DevicesOnHand = false,
                                    Quantity = 1,
                                    FinanceOptionId = 2,   // FinanceOptionId : 2 is Subscription for software product, if you need add hardware product, you need use 1 as sold, 3 as rent
                                    BusinessUnitId = business_unit_id,
                                    DisconnectionSetting = PayMedia.ApplicationServices.AgreementManagement.ServiceContracts.DisconnectionDateSettings.QuoteBased
                                },
                            });
                        }
                    }


                    ProductCaptureParams param = new ProductCaptureParams()
                    {
                        SandboxId = sandbox_id,
                        AgreementId = agreement_id,
                        CaptureReasons = new ProductCaptureReasons()
                        {
                            CreateReason = reason120
                        },
                        OfferDefinitions = offers,
                        SkipWorkOrderGeneration = true,
                        SkipShippingOrderGeneration = false,
                        AgreementDetailWithOfferDefinitions = productList


                    };

                    var result = agService.ManageProductCapture(param);

                    if (result.AgreementDetails.Items.Count == 0)
                    {
                        //Console.WriteLine("Warnings : " + result.WarningMessage);

                        //Console.WriteLine("Something bad happened. Sandbox rolled back.");

                        //The "false" value in this line of code rolls back the sandbox.
                        sandboxService.FinalizeSandbox(sandbox_id, false);
                        is_succes = false;

                    }
                    else
                    {
                        //The "true" value in this line of code commits the sandbox.
                        sandboxService.FinalizeSandbox(sandbox_id, true);

                        foreach (var item in result.AgreementDetails)
                        {
                            //Console.WriteLine("Congratulations! New Product ID = {0}.", item.Id);
                        }
                        is_succes = true;
                    }
                }
                else
                {
                    //Console.WriteLine("Can't find agreement for customer id : " + customer_id);
                    is_succes = false;
                }

            }
            catch (Exception ex)
            {
                //Console.WriteLine("Errors : " + ex.Message);
                //Console.WriteLine("Exception Stack : " + ex.StackTrace);
                is_succes = false;
            }

            if (is_succes == true)
            {
                return Request.CreateResponse(HttpStatusCode.OK, "SUCCESS");
            }
            else
            {
                var message = string.Format("error");
                HttpError err = new HttpError(message);
                return Request.CreateResponse(HttpStatusCode.OK, "ERROR" );
            }

        }

        [Route("api/customer/post_data")]
        [HttpPost]
        public String Post_data(Customers m)
        {
            string a = m.Name1;
            string b = m.Name2;
            string total;

            total = "name1 = " + a + " ---- name2 = " + b;

            return total;
        }

        [HttpGet]
        [ActionName("GetAllCustomerByCountryId")]
        [Route("api/{username_ad}/{password_ad}/customer/GetAllCustomerByCountryId/{id}")]
        public HttpResponseMessage GetAllCustomerByCountryId(String username_ad, String password_ad, int id)
        {
            Authentication_class var_auth = new Authentication_class();
            AuthenticationHeader authHeader = var_auth.getAuthHeader(username_ad, password_ad);
            AsmRepository.SetServiceLocationUrl(var_auth.var_service_location_url);

            var customersService = AsmRepository.GetServiceProxyCachedOrDefault<ICustomersService>(authHeader);
            var financeService = AsmRepository.GetServiceProxyCachedOrDefault<IFinanceService>(authHeader);
            var financeConfigurationService = AsmRepository.GetServiceProxyCachedOrDefault<IFinanceConfigurationService>(authHeader);

            #region Find customer by phone number
            //Instantiate a BaseQueryRequest for your filter criteria.
            BaseQueryRequest request = new BaseQueryRequest();

            //Because the phone number is a string, the value entered
            //must exactly match the customer's phone number in the database.
            //For an example that searches for a matching address,
            //see the code sample called Code_FindCustomerByAddressDetails.pdf


            request.FilterCriteria = Op.Like("CountryId", id);

            //Call the method and display the results.
            //This method searches ALL the addresses for a matching phone number.
            FindCustomerCollection customerColl = customersService.GetCustomers(request);
            #endregion


            if (customerColl != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, customerColl);
            }
            else
            {
                var message = string.Format("error");
                HttpError err = new HttpError(message);
                return Request.CreateResponse(HttpStatusCode.OK, message);
            }
        }

        [HttpGet]
        //[ActionName("GetAllCustomerByHomePhoneLike")]
        [Route("api/{username_ad}/{password_ad}/customer/GetAllCustomerByHomePhoneLike")]
        public HttpResponseMessage GetAllCustomerByHomePhoneLike(String username_ad, String password_ad)
        {
            Authentication_class var_auth = new Authentication_class();
            AuthenticationHeader authHeader = var_auth.getAuthHeader(username_ad, password_ad);
            AsmRepository.SetServiceLocationUrl(var_auth.var_service_location_url);

            var customersService = AsmRepository.GetServiceProxyCachedOrDefault<ICustomersService>(authHeader);
            var financeService = AsmRepository.GetServiceProxyCachedOrDefault<IFinanceService>(authHeader);
            var financeConfigurationService = AsmRepository.GetServiceProxyCachedOrDefault<IFinanceConfigurationService>(authHeader);

            #region Find customer by phone number
            //Instantiate a BaseQueryRequest for your filter criteria.
            BaseQueryRequest request = new BaseQueryRequest();

            //Because the phone number is a string, the value entered
            //must exactly match the customer's phone number in the database.
            //For an example that searches for a matching address,
            //see the code sample called Code_FindCustomerByAddressDetails.pdf

            request.FilterCriteria = Op.Like("HomePhone", "%0%") ||
            Op.Like("WorkPhone", "%1%") ||
            Op.Like("Fax2", "%1%") ||
            Op.Like("Fax1", "%1%");

            //Call the method and display the results.
            //This method searches ALL the addresses for a matching phone number.
            FindCustomerCollection customerColl = customersService.GetCustomers(request);

            ArrayList list = new ArrayList();
            
            if (customerColl != null)
            {
                foreach (FindCustomer c in customerColl)
                {
                    //Console.WriteLine("Found Customer ID {0}, name {1} {2}", c.Id, c.FirstName, c.Surname);
                    list.Add(c.Id.ToString());
                    list.Add(c.CareOfName);
                    list.Add(c.MiddleName);
                    list.Add(c.FirstName);
                }
            }
            else
            {
                Console.WriteLine("Cannot find a customer with that phone number.");
            }

            #endregion
            //list.Add("deviceId"); list.Add(devices.Id.ToString());
            //list.Add("SerialNumber"); list.Add(devices.SerialNumber);
            //list.Add("Shidate"); list.Add(devices.ShipDate.ToString());
            //list.Add("StatusId"); list.Add(devices.StatusId.ToString());
            //list.Add("ModelId"); list.Add(devices.ModelId.ToString());
            //list.Add("BigCarReferenceNumber"); list.Add(devices.BigCAReferenceNumber);

            if (customerColl != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, customerColl);
            }
            else
            {
                var message = string.Format("error");
                HttpError err = new HttpError(message);
                return Request.CreateResponse(HttpStatusCode.OK, message);
            }

            //return customerColl;
        }


        //id1 = firstname
        //id2 = middlename
        //id3 = surname
        //id4 = homephone
        //id5 = workphone
        //id6 = mobilephone
        //id7 = emergencyphone
        //id8 = identificationId
        //id9 = pln id
        //id10 = address
        //id11 = postalcode
        
        //[ActionName("DuplicationCheckCustomer")]
        [Route("api/customer/DuplicationCheckCustomer")]
        [HttpPost]
        public HttpResponseMessage DuplicationCheckCustomer([FromBody]DuplicationCheckParams customer_data)
        {
            Authentication_class var_auth = new Authentication_class();
            AuthenticationHeader authHeader = var_auth.getAuthHeader(customer_data.username_ad, customer_data.password_ad);
            AsmRepository.SetServiceLocationUrl(var_auth.var_service_location_url);
            ICustomersService custService = null;
            ICustomersConfigurationService custcService = null;
            
            custService = AsmRepository.AllServices.GetCustomersService(authHeader);
            custcService = AsmRepository.AllServices.GetCustomersConfigurationService(authHeader);
            // Get Valid Address Info by postal code
            var ads = custcService.GetValidAddresses(new BaseQueryRequest()
            {
                FilterCriteria = new CriteriaCollection()
                {
                    new Criteria()
                    {
                        Key = "PostalCode",
                        Operator = Operator.Equal,
                        Value = customer_data.ZipCode     // Postal code of your potential customer. 
                    }
                }
            });

            // Do Duplicate Check
            var findCustomers = custService.GetDuplicateCustomers(new DuplicateCustomerCriteria()
            {
                ValidAddressId = ads.Items[0].Id.Value,
                CustomerCaptureCategory = CustomerCaptureCategory.ResidentialCustomers,
                FirstName = customer_data.FirstName,
                MiddleName = customer_data.MiddleName,
                Surname = customer_data.Lastname,

                //HouseNumberAlpha = "123",   
                Extra = "chongqing",      // Address Line 1
                ExtraExtra = "yuzhongqu", // Address Line 2
                HomePhone = customer_data.HomePhone,
                WorkPhone = customer_data.WorkPhone,

                Fax1 = customer_data.EmergencyPhone,   // emergency phone
                Fax2 = customer_data.MobilePhone,   // Mobile Phone
                //CustomField1 = "aaaaa",
                //CustomField2 = "bbbbb",
                ReferenceTypeKey = 1, // 
                ReferenceNumber = "RN123",
            });


            if (findCustomers != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, findCustomers);
            }
            else
            {
                var message = string.Format("error");
                HttpError err = new HttpError(message);
                return Request.CreateResponse(HttpStatusCode.OK, message);
            }

            //return findCustomers;
        }

        //rejoin process
        [HttpPost]
        [ActionName("doSendContacts")]
        [Route("api/{username_ad}/{password_ad}/customer/doSendContacts")]
        public HttpResponseMessage doSendContacts([FromBody] Request req)
        {
            #region Authentication

            Authentication_class var_auth = new Authentication_class();
            AuthenticationHeader authHeader = var_auth.getAuthHeader(req.username_ad, req.password_ad);
            AsmRepository.SetServiceLocationUrl(var_auth.var_service_location_url);
            IContactsService ctService = AsmRepository.AllServices.GetContactsService(authHeader);
            IContactsConfigurationService ctcService = AsmRepository.AllServices.GetContactsConfigurationService(authHeader);

            #endregion
            
            Contact ctt = new Contact()
            {
                CustomerId = req.CustId,
                CategoryKey = req.CategoryKey, // Contact category Id
                MethodKey = "T",
                WorkOrderId = null,
                OrderId = null,  // Shipping Order Id
                ProductId = null,   // Commercial Product Id
                CustomerProductId = null, // Agreement Detail Id
                ProblemDescription = req.ProblemDescription,
                
                StatusKey = "C",
                ActionTaken = "",
                DeviceId = null
            };
            // 42579 The reason to create contact.
            // 48861 reason for close
            int reasonid = 42579;  // The reason to create contact.
            Contact ct = ctService.CreateContact(ctt, 42579);

            if(req.direct_close == 1)
            {
                Contact ct1 = ctService.UpdateContact(ct, 50955);
                return Request.CreateResponse(HttpStatusCode.OK, ct1);
            }
            else
                return Request.CreateResponse(HttpStatusCode.OK, ct);



        }
        //end of rejoin process
        

        [HttpGet]
        //[ActionName("GetCustDetailByCustId")]
        [Route("api/{username_ad}/{password_ad}/customer/GetCategoryKey")]
        public HttpResponseMessage GetCategoryKey(String username_ad, String password_ad)
        {
            Authentication_class var_auth = new Authentication_class();
            AuthenticationHeader authHeader = var_auth.getAuthHeader(username_ad, password_ad);

            AsmRepository.SetServiceLocationUrl(var_auth.var_service_location_url);
            var ctService = AsmRepository.AllServices.GetContactsService(authHeader);
            var ctcService = AsmRepository.AllServices.GetContactsConfigurationService(authHeader);

            try
            {
                ContactCategoryCollection category = ctcService.GetAllContactCategories();
                return Request.CreateResponse(HttpStatusCode.OK, category);
            }
            catch
            {
                var message = string.Format("An Error Has Occured ");
                HttpError err = new HttpError(message);
                return Request.CreateResponse(HttpStatusCode.BadRequest, err);
            }


        }



        [HttpGet]
        //[ActionName("GetCustDetailByCustId")]
        [Route("api/{username_ad}/{password_ad}/customer/GetCustDetailByCustId/{id}")]
        public HttpResponseMessage GetCustDetailByCustId(String username_ad, String password_ad, int id)
        {
            Authentication_class var_auth = new Authentication_class();
            AuthenticationHeader ah = var_auth.getAuthHeader(username_ad, password_ad);

            AsmRepository.SetServiceLocationUrl(var_auth.var_service_location_url);
            var customersService = AsmRepository.GetServiceProxyCachedOrDefault<ICustomersService>(ah);
            
            var customers = customersService.GetCustomer(id);
            return Request.CreateResponse(HttpStatusCode.OK, customers);
                
            
                //var message = string.Format("customers with id = {0} not found", id);
                //HttpError err = new HttpError(message);
                //return Request.CreateResponse(HttpStatusCode.OK, err);
            


        }


        [HttpGet]
        [Route("api/{username_ad}/{password_ad}/customer/UpdateGeoLocationCustByCustId/{id}/{geolocation}")]
        public Customer UpdateGeoLocationCustByCustId(String username_ad, String password_ad, int id, string geolocation)
        {
            Authentication_class var_auth = new Authentication_class();
            AuthenticationHeader ah = var_auth.getAuthHeader(username_ad, password_ad);
            AsmRepository.SetServiceLocationUrl(var_auth.var_service_location_url);
            ICustomersService customerService = AsmRepository.GetServiceProxyCachedOrDefault<ICustomersService>(ah);
            Customer cust = new Customer();
            bool exists = customerService.CustomerExists(id);
            if (exists == true)
            {
                // Retrieve customer and display information.
                cust = customerService.GetCustomer(id);
                
                cust.DefaultAddress.Directions = "directions saya ganti";

                //cust.DefaultAddress.CustomFields[0].Value = "ini contact name diganti huda";
                //cust.DefaultAddress.CustomFields[1].Value = "Saudara";
                //cust.DefaultAddress.CustomFields[2].Value = "ini Emergency Contact Address diganti siapa";
                cust.DefaultAddress.CustomFields[3].Value = geolocation;

                //var cf = new customFieldHandler(ah);
                //cf.addAddressCustomField("Emergency Contact Name", cust.DefaultAddress.Id.Value, "ini contact name");
                //cf.addAddressCustomField("Emergency Contact Relationship", cust.DefaultAddress.Id.Value, "Saudara");
                //cf.addAddressCustomField("Emergency Contact Address", cust.DefaultAddress.Id.Value, "ini alamat emergency");
                //cf.addAddressCustomField("GPS latitude and longitude", cust.DefaultAddress.Id.Value, "-88888888888, -9999999999999");
                
                int reasonKey = 0;
                int reasonTypeKey = 0;
                int reasonStatusKey = 0;
                int reasonAddressKey = 0;
                customerService.UpdateCustomer(cust, reasonKey, reasonTypeKey, reasonStatusKey, reasonAddressKey);

                return cust;
            }
            else
            {
                return null;
            }

        }

        [HttpGet]
        [Route("api/{username_ad}/{password_ad}/customer/createRelationshipForCustomer/{parent_cust_id}/{child_cust_id}/{relationship_type_key}")]
        public HttpResponseMessage createRelationshipForCustomer(String username_ad, String password_ad, int child_cust_id, int parent_cust_id, string relationship_type_key)
        {
            Authentication_class var_auth = new Authentication_class();
            AuthenticationHeader ah = var_auth.getAuthHeader(username_ad, password_ad);
            AsmRepository.SetServiceLocationUrl(var_auth.var_service_location_url);

            ICustomersService custService = AsmRepository.AllServices.GetCustomersService(ah);

            //asasasas
            custService.CreateRelation(new Relation()
            {
                CustomerIdFrom = parent_cust_id,
                CustomerIdTo = child_cust_id,
                RelationTypeKey = relationship_type_key
            }, 0);

            if (custService != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, custService);
            }
            else
            {
                var message = string.Format("error");
                HttpError err = new HttpError(message);
                return Request.CreateResponse(HttpStatusCode.OK, message);
            }
            

        }

        [HttpGet]
        [Route("api/{username_ad}/{password_ad}/customer/updateRelatioshipForCustomer/{child_cust_id}/{parent_cust_id}/{relationship_type_key}")]
        public HttpResponseMessage updateRelatioshipForCustomer(String username_ad, String password_ad, int child_cust_id, int parent_cust_id, string relationship_type_key)
        {
            Authentication_class var_auth = new Authentication_class();
            AuthenticationHeader ah = var_auth.getAuthHeader(username_ad, password_ad);
            AsmRepository.SetServiceLocationUrl(var_auth.var_service_location_url);

            ICustomersService custService = AsmRepository.AllServices.GetCustomersService(ah);

            //asasasas
            RelationCollection relationships = custService.GetRelations(child_cust_id, new CriteriaCollection(){
                new Criteria()
                {
                    Key = "RelationTypeKey",
                    Operator = Operator.Equal,
                    Value = relationship_type_key
                }
            }, 0);

            if (relationships.Items.Count > 0)
            {
                var relationship = relationships.Items[0];
                relationship.CustomerIdFrom = parent_cust_id;
                relationship.CustomerIdTo = child_cust_id;
                relationship.RelationTypeKey = relationship_type_key;
                custService.UpdateRelation(relationship, 0);
                
                return Request.CreateResponse(HttpStatusCode.OK, relationship);
            }
            else
            {
                var message = string.Format("error");
                HttpError err = new HttpError(message);
                return Request.CreateResponse(HttpStatusCode.OK, message);
            }

            //if (relationships.Items.Count > 0)
            //{
                
            //    var relationship = relationships.Items[0];
            //    relationship.CustomerIdFrom = parent_cust_id;
            //    relationship.CustomerIdTo = child_cust_id; 
            //    relationship.RelationTypeKey = relationship_type_key;
            //    custService.UpdateRelation(relationship, 0);

            //    return relationships;
            //}
            //else
            //{
            //    return null;
            //    //Console.WriteLine("Can't find relationship type : " + relationship_type_key + " on customer : " + customer_id);
            //}

        }

        [HttpGet]
        [Route("api/{username_ad}/{password_ad}/customer/deleteRelationshipForCustomer/{cust_id}/{relationship_type_key}")]
        public HttpResponseMessage deleteRelationshipForCustomer(String username_ad, String password_ad, int cust_id, string relationship_type_key)
        {
            Authentication_class var_auth = new Authentication_class();
            AuthenticationHeader ah = var_auth.getAuthHeader(username_ad, password_ad);
            AsmRepository.SetServiceLocationUrl(var_auth.var_service_location_url);

            ICustomersService custService = AsmRepository.AllServices.GetCustomersService(ah);

            //asasasas
            int[] r_ids = new int[1];
            var relationships = custService.GetRelations(cust_id, new CriteriaCollection(){
                new Criteria()
                {
                    Key = "RelationTypeKey",
                    Operator = Operator.Equal,
                    Value = relationship_type_key
                }
            }, 0);

            if (relationships.Items.Count > 0)
            {
                r_ids[0] = relationships.Items[0].Id.Value;
                custService.DeleteRelations(r_ids, 0);
                //return custService;

                return Request.CreateResponse(HttpStatusCode.OK, custService);
            }
            else
            {
                var message = string.Format("error");
                HttpError err = new HttpError(message);
                return Request.CreateResponse(HttpStatusCode.OK, message);
            }

            //if (relationships.Items.Count > 0)
            //{
            //    r_ids[0] = relationships.Items[0].Id.Value;
            //    custService.DeleteRelations(r_ids, 0);
            //    return custService;
            //}
            //else
            //    return null;

        }

    }
}
