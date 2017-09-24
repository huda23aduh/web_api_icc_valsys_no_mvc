using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using PayMedia.ApplicationServices.SharedContracts;
using PayMedia.ApplicationServices.ClientProxy;
using PayMedia.ApplicationServices.Customers.ServiceContracts;
using PayMedia.ApplicationServices.Customers.ServiceContracts.DataContracts;
using PayMedia.ApplicationServices.OrderManagement.ServiceContracts;
using PayMedia.ApplicationServices.OrderManagement.ServiceContracts.DataContracts;
using web_api_icc_valsys_no_mvc.Models;
using PayMedia.ApplicationServices.ViewFacade.ServiceContracts.DataContracts;
using PayMedia.ApplicationServices.Devices.ServiceContracts.DataContracts;
using PayMedia.ApplicationServices.Devices.ServiceContracts;
using PayMedia.ApplicationServices.SandBoxManager.ServiceContracts;
using PayMedia.ApplicationServices.Finance.ServiceContracts;
using PayMedia.ApplicationServices.ViewFacade.ServiceContracts;
using PayMedia.ApplicationServices.AgreementManagement.ServiceContracts;
using PayMedia.ApplicationServices.AgreementManagement.ServiceContracts.DataContracts;

using web_api_icc_valsys_no_mvc.Models;
using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace web_api_icc_valsys_no_mvc.Controllers
{
    public class ShippingOrderController : ApiController
    {
        IOrderManagementService soService = null;
        ISandBoxManagerService sbService = null;
        IFinanceService faService = null;
        IAgreementManagementService agService = null;
        IDevicesService deviceService = null;
        IViewFacadeService viewfService = null;
        //string msg = null;
        string QuoteAccountTypeId = "1";
        //string WalletAccountTypeId = "2";
        const int SOPENDING = 1;
        const int SOMAYSHIP = 21;
        const int DECODER_TP_ID = 3;   // 3 Decoder SD, 2 : Decoder HD
        const int DECODER_HD_TP_ID = 2;
        const int SCTPID = 7;
        const string DEVICE_STATUS_STOCK = "1";
        const string DEVICE_STATUS_REPAIREDSTOCK = "21";
        const string DEVICE_STATUS_REFURSTOCK = "22";
        const int DECODER_MODEL_ID = 34;   // Put the real decoder model id here
        const int SC_MODEL_ID = 48;
        const int FORENT = 3;
        const int mayship_reason = 59331;
        const int shipso_reason = 99122;
        const int internet_router_tp_id = 10;
        const int xl_sim_tp_id = 12;
        const int indosat_sim_tp_id = 13;
        const int xl_sim_model_id = 55;
        const int indosat_sim_model_id = 56;
        const int router_model_id = 53;
        const int lnb_tp_id = 4;
        const int antenna_tp_id = 30;
        const int DECODER_HD_MODEL_ID = 1;


        [HttpGet]
        [Route("api/{username_ad}/{password_ad}/shippingorder/GetShippingOrderByStatusId/{id}")]
        public HttpResponseMessage GetShippingOrderByStatusId(String username_ad, String password_ad, int id)
        {

            Authentication_class var_auth = new Authentication_class();
            AuthenticationHeader ah = var_auth.getAuthHeader(username_ad, password_ad);
            AsmRepository.SetServiceLocationUrl(var_auth.var_service_location_url);
            var TSService = AsmRepository.GetServiceProxyCachedOrDefault<IOrderManagementService>(ah);

            BaseQueryRequest request = new BaseQueryRequest();
            request.FilterCriteria = new CriteriaCollection();
            request.FilterCriteria.Add(new Criteria("StatusId", 103));

            var servicetype = TSService.GetShippingOrders(request);

            if (servicetype != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, servicetype);
            }
            else
            {
                var message = string.Format("error");
                HttpError err = new HttpError(message);
                return Request.CreateResponse(HttpStatusCode.OK, message);
            }
            


        }

        [HttpGet]
        [Route("api/{username_ad}/{password_ad}/shippingorder/GetShippingOrderById/{id}")]
        public HttpResponseMessage GetShippingOrderById(String username_ad, String password_ad, int id)
        {

            Authentication_class var_auth = new Authentication_class();
            AuthenticationHeader ah = var_auth.getAuthHeader(username_ad, password_ad);
            AsmRepository.SetServiceLocationUrl(var_auth.var_service_location_url);
            var TSService = AsmRepository.GetServiceProxyCachedOrDefault<IOrderManagementService>(ah);

            BaseQueryRequest request = new BaseQueryRequest();
            request.FilterCriteria = new CriteriaCollection();
            request.FilterCriteria.Add(new Criteria("Id", id));

            ShippingOrder servicetype = TSService.GetShippingOrder(id);
            if (servicetype != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, servicetype);
            }
            else
            {
                var message = string.Format("error");
                HttpError err = new HttpError(message);
                return Request.CreateResponse(HttpStatusCode.OK, message);
            }


        }

        [HttpGet]
        [Route("api/{username_ad}/{password_ad}/shippingorder/GetFormatHWByShippingOrderId/{so_id}")]
        public HttpResponseMessage GetFormatHWByShippingOrderId(String username_ad, String password_ad, int so_id)
        {
            StringBuilder sb = new StringBuilder();

            string the_json = get_request_to_api("http://192.168.177.186:1111/api/hdkartika/ICCMsky2016/shippingorder/GetShippingOrderById/" + so_id);
            dynamic parsedJson = JsonConvert.DeserializeObject(the_json);

            for (int i = 0; i < parsedJson.ShippingOrderLines.Count; i++)
            {
                sb.Append(parsedJson.ShippingOrderLines[i].TechnicalProductId);

                if (i != parsedJson.ShippingOrderLines.Count - 1)
                    sb.Append("-");
            }

            sb.ToString();

            if (parsedJson != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, sb);
            }
            else
            {
                var message = string.Format("error");
                HttpError err = new HttpError(message);
                return Request.CreateResponse(HttpStatusCode.OK, message);
            }


        }
        public string get_request_to_api(string url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            try
            {
                WebResponse response = request.GetResponse();
                using (Stream responseStream = response.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                    return reader.ReadToEnd();
                }
            }
            catch (WebException ex)
            {
                WebResponse errorResponse = ex.Response;
                using (Stream responseStream = errorResponse.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(responseStream, Encoding.GetEncoding("utf-8"));
                    String errorText = reader.ReadToEnd();
                    // log errorText

                }
                throw;
            }
        }

        [HttpGet]
        [Route("api/{username_ad}/{password_ad}/shippingorder/GetShippingOrderByCustomerId/{id}")]
        public HttpResponseMessage GetShippingOrderByCustomerId(String username_ad, String password_ad, int id)
        {

            Authentication_class var_auth = new Authentication_class();
            AuthenticationHeader ah = var_auth.getAuthHeader(username_ad, password_ad);
            AsmRepository.SetServiceLocationUrl(var_auth.var_service_location_url);
            var TSService = AsmRepository.GetServiceProxyCachedOrDefault<IOrderManagementService>(ah);

            

            var servicetype = TSService.GetShippedOrders(id, 1);

            if (servicetype != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, servicetype);
            }
            else
            {
                var message = string.Format("error");
                HttpError err = new HttpError(message);
                return Request.CreateResponse(HttpStatusCode.OK, message);
            }
            


        }

        [HttpGet]
        [Route("api/{username_ad}/{password_ad}/shippingorder/LinkDeviceThenSipped/{cust_id}/{decode_sn}/{smartcard_sn}/{is_inet}/{lnb_sn}/{antenna_sn}/{shipping_order_id}/{is_hd}")]
        public AgreementDetailCollection LinkDeviceThenSipped(String username_ad, String password_ad, int cust_id, String decode_sn, String smartcard_sn, int is_inet, string lnb_sn, string antenna_sn, int shipping_order_id, int is_hd)
        {

            Authentication_class var_auth = new Authentication_class();
            AuthenticationHeader ah = var_auth.getAuthHeader(username_ad, password_ad);
            AsmRepository.SetServiceLocationUrl(var_auth.var_service_location_url);

            bool is_inet_bool = false;
            bool is_hd_bool = false;

            string lnb_sn_string;
            string antenna_sn_string;


            if (is_inet == 1)
                is_inet_bool = true;
            
            else if (is_inet == 0)
                is_inet_bool = false;


            if (is_hd == 1)
                is_hd_bool = true;

            else if (is_hd == 0)
                is_hd_bool = false;



            if (lnb_sn == "0")
                lnb_sn_string = "";
            else
                lnb_sn_string = lnb_sn;

            if (antenna_sn == "0")
                antenna_sn_string = "";
            else
                antenna_sn_string = antenna_sn;

            MayShipSO(username_ad, password_ad, cust_id);
            
            ShippingOrder var_shipping_order = ShipShippingOrder_ver2(username_ad, password_ad, cust_id, decode_sn, smartcard_sn, is_inet_bool, lnb_sn_string, antenna_sn_string, shipping_order_id, is_hd_bool);

            //return var_shipping_order;

            if (var_shipping_order != null)
            {

                var agreementManagementService = AsmRepository.GetServiceProxyCachedOrDefault<IAgreementManagementService>(ah);
                AgreementDetailCollection adc = agreementManagementService.GetAgreementDetailsForCustomer(cust_id, 1);
                return adc;
            }
            else
                return null;



        }
        public ShippingOrder ShipShippingOrder_ver2(String username_ad, String password_ad, int custid, string decoder_serial_number, string smartcard_serial_number, bool isInternet, string lnb_sn, string antenna_sn, int shippingorder_id, bool isHD)
        {
            Authentication_class var_auth = new Authentication_class();
            AuthenticationHeader authHeader = var_auth.getAuthHeader(username_ad, password_ad);
            AsmRepository.SetServiceLocationUrl(var_auth.var_service_location_url);

            //var docmService = AsmRepository.GetServiceProxy<IDocumentManagementService>(authHeader);
            var soService = AsmRepository.AllServices.GetOrderManagementService(authHeader);
            var faService = AsmRepository.AllServices.GetFinanceService(authHeader);
            var sbService = AsmRepository.AllServices.GetSandBoxManagerService(authHeader);
            var agService = AsmRepository.AllServices.GetAgreementManagementService(authHeader);
            var deviceService = AsmRepository.AllServices.GetDevicesService(authHeader);
            var viewfService = AsmRepository.AllServices.GetViewFacadeService(authHeader);

            //msg = "Ship the shipping order...";
            //Console.WriteLine(msg);
            //logger.Info(msg);

            int d_tp = 0;
            int s_tp = 0;
            int d_model = 0;
            int s_model = 0;

            int lnb_model = 0;
            int antenna_model = 0;

            if (isInternet)
            {
                d_tp = internet_router_tp_id;
                s_tp = xl_sim_tp_id;
                d_model = router_model_id;
                s_model = xl_sim_model_id;
            }
            else
            {
                d_tp = DECODER_TP_ID;
                s_tp = SCTPID;
                d_model = DECODER_MODEL_ID;
                s_model = SC_MODEL_ID;
            }

            if (isHD)
            {
                //d_model = DECODER_HD_TP_ID;
                d_tp = DECODER_HD_TP_ID;
                d_model = DECODER_HD_MODEL_ID;
            }

            try
            {
                // Get the hardware agreement detail of customer
                AgreementDetailView hardwaread = null;
                var hardwareads = viewfService.GetAgreementDetailView(new BaseQueryRequest()
                {
                    FilterCriteria = Op.Eq("DeviceIncluded", true) & Op.Eq("CustomerId", custid) & Op.Gt("Id", 0) & Op.IsNull("ProvisionedDevices"),
                    PageCriteria = new PageCriteria(1),
                    SortCriteria = new SortCriteriaCollection(){
                        new SortCriteria()
                        {
                            Key = "Id",
                            SortDirection = SortDirections.Descending
                        }
                    }
                });

                if (hardwareads.TotalCount == 0)
                {
                    //Console.WriteLine("Hardware product is not captured, can't ship the shipping order for customer id : " + custid);
                    return null;
                }
                else
                {
                    hardwaread = hardwareads.Items[0];
                }

                // If not input the shipping order id then find the shipping order id by the random hardware product which don't assign device info yet.
                if (shippingorder_id == 0)
                {
                    shippingorder_id = getShippingOrderByHardware(username_ad, password_ad , hardwaread.Id.Value);
                    //Console.WriteLine("Ship the Shipping Order Id is : " + shippingorder_id);
                }
                // get the May shipping order and shipping order line( with smartcard or Sim card in it).
                var soc = soService.GetShippedOrders(custid, 0).Items.Find(t => (t.StatusId == SOMAYSHIP && t.Id.Value == shippingorder_id));


                if (soc == null)
                {
                    //Console.WriteLine("No Shipping Order with status May Ship, please check customer id : " + custid);
                    return null;
                }



                // Find the shipping order lines for decoder and smartcard

                var scsoline = soc.ShippingOrderLines.Items.Find(t => t.TechnicalProductId == s_tp);


                // Get random decoder and smartcard which are in stock, you should not use this since you have real device information. 
                Device decoder = null;
                Device smartcard = null;
                if (decoder_serial_number == "")
                {
                    var decoders = deviceService.GetDevices(
                        new BaseQueryRequest()
                        {
                            FilterCriteria = new CriteriaCollection()
                            {
                                new Criteria()
                                {
                                    Key="ModelId",
                                    Operator = Operator.Equal,
                                    Value = d_model.ToString()
                                },
                                new Criteria()
                                {
                                    Key="StatusId",
                                    Operator = Operator.Equal,
                                    Value = DEVICE_STATUS_STOCK
                                }
                            }
                        });
                    if (decoders.Items.Count == 0)
                    {
                        //Console.WriteLine("There are no decoder avaiable to use!");
                        return null;
                    }
                    else
                    {
                        decoder_serial_number = decoders.Items[0].SerialNumber;
                    }

                }
                else
                {
                    decoder = deviceService.GetDeviceBySerialNumber(decoder_serial_number);
                    if (decoder.StatusId.Value == Int32.Parse(DEVICE_STATUS_STOCK) || decoder.StatusId.Value == Int32.Parse(DEVICE_STATUS_REFURSTOCK)
                        || decoder.StatusId.Value == Int32.Parse(DEVICE_STATUS_REPAIREDSTOCK)
                       )
                    {

                    }
                    else
                    {
                        //Console.WriteLine(" Decoder with serial number " + decoder_serial_number + " is not in allowed capture status!");
                        return null;
                    }
                }


                if (smartcard_serial_number == "")
                {
                    var smartcards = deviceService.GetDevices(new BaseQueryRequest()
                    {
                        FilterCriteria = new CriteriaCollection()
                        {
                            new Criteria()
                            {
                                Key="ModelId",
                                Operator = Operator.Equal,
                                Value = s_model.ToString()
                            },
                            new Criteria()
                            {
                                Key="StatusId",
                                Operator = Operator.Equal,
                                Value = DEVICE_STATUS_STOCK
                            }
                        }
                    });
                    if (smartcards.Items.Count == 0)
                    {
                        //Console.WriteLine("There are no smartcard avaiable to use!");
                        return null;
                    }
                    else
                    {
                        smartcard_serial_number = smartcards.Items[0].SerialNumber;
                    }

                }
                else
                {
                    smartcard = deviceService.GetDeviceBySerialNumber(smartcard_serial_number);
                    if (smartcard.StatusId.Value == Int32.Parse(DEVICE_STATUS_STOCK) || smartcard.StatusId.Value == Int32.Parse(DEVICE_STATUS_REFURSTOCK)
                        || smartcard.StatusId.Value == Int32.Parse(DEVICE_STATUS_REPAIREDSTOCK)
                       )
                    {

                    }
                    else
                    {
                        //Console.WriteLine(" Smartcard with serial number " + smartcard_serial_number + " is not in allowed capture status!");
                        return null;
                    }
                }

                // Identify device info to the shipping order lines
                var dd = identifyDevice(username_ad, password_ad, soc, decoder_serial_number, d_tp, d_model);
                var sc = identifyDevice(username_ad, password_ad, soc, smartcard_serial_number, s_tp, s_model);
                if (lnb_sn.Length > 0)
                {
                    var lnb = deviceService.GetDeviceBySerialNumber(lnb_sn);
                    if (lnb.StatusId.Value == Int32.Parse(DEVICE_STATUS_STOCK) || lnb.StatusId.Value == Int32.Parse(DEVICE_STATUS_REFURSTOCK)
                        || lnb.StatusId.Value == Int32.Parse(DEVICE_STATUS_REPAIREDSTOCK)
                       )
                    {
                        lnb_model = lnb.ModelId.Value;
                    }
                    else
                    {
                        //Console.WriteLine(" LNB with serial number " + lnb_sn + " is not in allowed capture status!");
                        //return null;
                    }
                    identifyDevice(username_ad, password_ad, soc, lnb_sn, lnb_tp_id, lnb.ModelId.Value);
                }
                if (antenna_sn.Length > 0)
                {
                    var antenna = deviceService.GetDeviceBySerialNumber(lnb_sn);
                    if (antenna.StatusId.Value == Int32.Parse(DEVICE_STATUS_STOCK) || antenna.StatusId.Value == Int32.Parse(DEVICE_STATUS_REFURSTOCK)
                        || antenna.StatusId.Value == Int32.Parse(DEVICE_STATUS_REPAIREDSTOCK)
                       )
                    {
                        antenna_model = antenna.ModelId.Value;
                    }
                    else
                    {
                        //Console.WriteLine(" Antenna with serial number " + antenna_sn + " is not in allowed capture status!");
                        //return null;
                    }
                    identifyDevice(username_ad, password_ad, soc, antenna_sn, antenna_tp_id, antenna.ModelId.Value);
                }

                if (dd == null || sc == null)
                {
                    return null;
                }

                //msg = "Starting perform build list";
                //Console.WriteLine(msg);
                //logger.Debug(msg);


                // Ship the Shipping Order
                //msg = "Starting link the device to customer";
                //Console.WriteLine(msg);
                //logger.Debug(msg);

                // Fill the agreement detail id on the shipping order line to link the device to customer
                //soc.ShippingOrderLines.Items.Find(t => t.TechnicalProductId == d_tp).AgreementDetailId = hardwaread.Id.Value;
                //soc.ShippingOrderLines.Items.Find(t => t.TechnicalProductId == s_tp).AgreementDetailId = hardwaread.Id.Value;

                // Fill the correct model id
                soc.ShippingOrderLines.Items.Find(t => t.TechnicalProductId == d_tp).HardwareModelId = d_model;
                soc.ShippingOrderLines.Items.Find(t => t.TechnicalProductId == s_tp).HardwareModelId = s_model;
                soc.ShippingOrderLines.Items.Find(t => t.TechnicalProductId == lnb_tp_id).HardwareModelId = lnb_model;
                soc.ShippingOrderLines.Items.Find(t => t.TechnicalProductId == antenna_tp_id).HardwareModelId = antenna_model;

                soc.ShippingOrderLines.Items.Find(t => t.TechnicalProductId == d_tp).ReceivedQuantity = 1;
                soc.ShippingOrderLines.Items.Find(t => t.TechnicalProductId == s_tp).ReceivedQuantity = 1;

                soService.ShipOrder(soc, shipso_reason, null);
                //msg = "Shipping Order :" + soc.Id.Value + " has been shipped!";
                //logger.Info(msg);


                return soc;
            }
            catch (Exception ex)
            {
                //Console.WriteLine("Inner Exception : " + ex.InnerException.Message);
                //msg = "Errors : " + ex.Message + "  ------  Exception Stack : " + ex.StackTrace;
                //Console.WriteLine("Errors : " + ex.Message);
                //Console.WriteLine("Stack : " + ex.StackTrace);
                //logger.Error(msg);

                return null;
            }
        }

        
        //  Get Shipping Order by Hardware Product Agreeemntdetail ID
        public int getShippingOrderByHardware(String username_ad, String password_ad, int hw_agreementdetail_id)
        {
            Authentication_class var_auth = new Authentication_class();
            AuthenticationHeader authHeader = var_auth.getAuthHeader(username_ad, password_ad);
            AsmRepository.SetServiceLocationUrl(var_auth.var_service_location_url);

            //var docmService = AsmRepository.GetServiceProxy<IDocumentManagementService>(authHeader);
            var soService = AsmRepository.AllServices.GetOrderManagementService(authHeader);
            var faService = AsmRepository.AllServices.GetFinanceService(authHeader);
            var sbService = AsmRepository.AllServices.GetSandBoxManagerService(authHeader);
            var agService = AsmRepository.AllServices.GetAgreementManagementService(authHeader);
            var deviceService = AsmRepository.AllServices.GetDevicesService(authHeader);
            var viewfService = AsmRepository.AllServices.GetViewFacadeService(authHeader);

            int so = 0;
            try
            {
                var sol = soService.FindShippingOrderLines(new BaseQueryRequest()
                {
                    FilterCriteria = Op.Eq("AgreementDetailId", hw_agreementdetail_id),
                    PageCriteria = new PageCriteria() { Page = 0, PageSize = 100 }
                });

                if (sol.Items.Count > 0)
                {
                    so = sol.Items[0].ShippingOrderId.Value;
                }
                else
                {
                    return 0;
                }

                return so;
            }
            catch (Exception ex)
            {
                //msg = "Errors : " + ex.Message + "  ------  Exception Stack : " + ex.StackTrace;
                //Console.WriteLine("Errors : " + ex.Message);
                //Console.WriteLine("Stack : " + ex.StackTrace);
                //logger.Error(msg);
                return 0;
            }
        }

        // Identify device to shipping order
        public ShippingOrder identifyDevice(String username_ad, String password_ad, ShippingOrder soc, string serial_number, int tp_id, int model_id)
        {
            Authentication_class var_auth = new Authentication_class();
            AuthenticationHeader authHeader = var_auth.getAuthHeader(username_ad, password_ad);
            AsmRepository.SetServiceLocationUrl(var_auth.var_service_location_url);

            //var docmService = AsmRepository.GetServiceProxy<IDocumentManagementService>(authHeader);
            var soService = AsmRepository.AllServices.GetOrderManagementService(authHeader);
            var faService = AsmRepository.AllServices.GetFinanceService(authHeader);
            var sbService = AsmRepository.AllServices.GetSandBoxManagerService(authHeader);
            var agService = AsmRepository.AllServices.GetAgreementManagementService(authHeader);
            var deviceService = AsmRepository.AllServices.GetDevicesService(authHeader);

            var viewfService = AsmRepository.AllServices.GetViewFacadeService(authHeader);
            try
            {
                var so_lineitem = soc.ShippingOrderLines.Items.Find(t => t.TechnicalProductId == tp_id);

                if (so_lineitem == null)
                {
                    //Console.WriteLine("Can't find techinical product with id : " + tp_id + " in shipping order with id : " + soc.Id.Value);
                    return null;
                }

                var buildlist = new BuildList
                {
                    OrderLineId = so_lineitem.Id,
                    OrderId = soc.Id,
                    ModelId = model_id,
                    StockHandlerId = soc.ShipFromStockHandlerId,
                    TransactionType = BuildListTransactionType.ShippingSerializedProducts
                };

                var b_build_list = deviceService.CreateBuildList(buildlist);

                var d_a_list = deviceService.AddDeviceToBuildListManually(b_build_list.Id.Value, serial_number);

                if (d_a_list.Accepted.Value)
                {
                    var dblist = deviceService.PerformBuildListAction(b_build_list.Id.Value);
                }
                else
                {
                    //Console.WriteLine("Failed to attach decoder with SN : " + serial_number + " to Shipping Order : Error : " + d_a_list.Error);
                    return null;
                }

                return soc;
            }
            catch (Exception ex)
            {
                //msg = "Errors : " + ex.Message + "  ------  Exception Stack : " + ex.StackTrace;
                //Console.WriteLine("Errors : " + ex.Message);
                //Console.WriteLine("Stack : " + ex.StackTrace);
                //logger.Error(msg);
                return null;
            }

        }
        
        public ShippingOrder ShipShippingOrder_ver1(string username_ad, string password_ad, int custid, string decoder_serial_number, string smartcard_serial_number, bool isInternet = false)
        {
            Authentication_class var_auth = new Authentication_class();
            AuthenticationHeader authHeader = var_auth.getAuthHeader(username_ad, password_ad);
            AsmRepository.SetServiceLocationUrl(var_auth.var_service_location_url);

            var soService = AsmRepository.AllServices.GetOrderManagementService(authHeader);
            var faService = AsmRepository.AllServices.GetFinanceService(authHeader);
            var sbService = AsmRepository.AllServices.GetSandBoxManagerService(authHeader);
            var agService = AsmRepository.AllServices.GetAgreementManagementService(authHeader);
            var deviceService = AsmRepository.AllServices.GetDevicesService(authHeader);
            var viewfService = AsmRepository.AllServices.GetViewFacadeService(authHeader);

            //msg = "Ship the shipping order...";
            //Console.WriteLine(msg);
            //Console.WriteLine("decoder sn : " + decoder_serial_number);
            //Console.WriteLine("smartcard sn : " + smartcard_serial_number);
            //logger.Info(msg);

            int d_tp = 0;
            int s_tp = 0;
            int d_model = 0;
            int s_model = 0;

            if (isInternet)
            {
                d_tp = internet_router_tp_id;
                s_tp = xl_sim_tp_id;
                d_model = router_model_id;
                s_model = xl_sim_model_id;
            }
            else
            {
                d_tp = DECODER_TP_ID;
                s_tp = SCTPID;
                d_model = DECODER_MODEL_ID;
                s_model = SC_MODEL_ID;
            }

            try
            {
                // get the pending shipping order and shipping order line
                ShippingOrder soc = soService.GetShippedOrders(custid, 0).Items.Find(t => t.StatusId == SOMAYSHIP);


                if (soc == null)
                {
                    Console.WriteLine("No Shipping Order with status May Ship, please check customer id : " + custid);
                    return null;
                }

                // Get the hardware agreement detail of customer
                AgreementDetailView hardwaread = null;
                var hardwareads = viewfService.GetAgreementDetailView(new BaseQueryRequest()
                {
                    FilterCriteria = Op.Eq("DeviceIncluded", true) & Op.Eq("CustomerId", custid) & Op.Gt("Id", 0) & Op.IsNull("ProvisionedDevices"),
                    PageCriteria = new PageCriteria(1),
                    SortCriteria = new SortCriteriaCollection(){
                        new SortCriteria()
                        {
                            Key = "Id",
                            SortDirection = SortDirections.Descending
                        }
                    }
                });

                if (hardwareads.TotalCount == 0)
                {
                    //Console.WriteLine("Hardware product is not captured, can't ship the shipping order for customer id : " + custid);
                    return null;
                }
                else
                {
                    hardwaread = hardwareads.Items[0];
                }

                // Find the shipping order lines for decoder and smartcard
                var decodersoline = soc.ShippingOrderLines.Items.Find(t => t.TechnicalProductId == d_tp);
                var scsoline = soc.ShippingOrderLines.Items.Find(t => t.TechnicalProductId == s_tp);


                // Get random decoder and smartcard which are in stock, you should not use this since you have real device information.
                if (decoder_serial_number == "")
                {
                    decoder_serial_number = deviceService.GetDevices(
                        new BaseQueryRequest()
                        {
                            FilterCriteria = new CriteriaCollection()
                            {
                                new Criteria()
                                {
                                    Key="ModelId",
                                    Operator = Operator.Equal,
                                    Value = d_model.ToString()
                                },
                                new Criteria()
                                {
                                    Key="StatusId",
                                    Operator = Operator.Equal,
                                    Value = DEVICE_STATUS_STOCK
                                }
                            }
                        }).Items[0].SerialNumber;
                }
                else
                {
                    d_model = deviceService.GetDeviceBySerialNumber(decoder_serial_number).ModelId.Value;
                }


                if (smartcard_serial_number == "")
                {
                    smartcard_serial_number = deviceService.GetDevices(new BaseQueryRequest()
                    {
                        FilterCriteria = new CriteriaCollection()
                        {
                            new Criteria()
                            {
                                Key="ModelId",
                                Operator = Operator.Equal,
                                Value = s_model.ToString()
                            },
                            new Criteria()
                            {
                                Key="StatusId",
                                Operator = Operator.Equal,
                                Value = DEVICE_STATUS_STOCK
                            }
                       }
                    }).Items[0].SerialNumber;
                }
                else
                {
                    s_model = deviceService.GetDeviceBySerialNumber(smartcard_serial_number).ModelId.Value;
                }

                // Identify device info to the shipping order lines
                BuildList buildListdecoder = new BuildList
                {
                    OrderLineId = decodersoline.Id,
                    OrderId = soc.Id,
                    ModelId = decodersoline.HardwareModelId.GetValueOrDefault(),
                    StockHandlerId = soc.ShipFromStockHandlerId,
                    TransactionType = BuildListTransactionType.ShippingSerializedProducts
                };

                BuildList bldecoder = deviceService.CreateBuildList(buildListdecoder);

                BuildList buildListsc = new BuildList
                {
                    OrderLineId = scsoline.Id,
                    OrderId = soc.Id,
                    ModelId = scsoline.HardwareModelId.GetValueOrDefault(),
                    StockHandlerId = soc.ShipFromStockHandlerId,
                    TransactionType = BuildListTransactionType.ShippingSerializedProducts
                };

                BuildList blsc = deviceService.CreateBuildList(buildListsc);

                deviceService.AddDeviceToBuildListManually(bldecoder.Id.Value, decoder_serial_number);
                deviceService.AddDeviceToBuildListManually(blsc.Id.Value, smartcard_serial_number);

                //msg = "Starting perform build list";
                //Console.WriteLine(msg);
                //logger.Debug(msg);

                deviceService.PerformBuildListAction(bldecoder.Id.Value);
                deviceService.PerformBuildListAction(blsc.Id.Value);


                // Ship the Shipping Order
                //msg = "Starting link the device to customer";
                //Console.WriteLine(msg);
                //logger.Debug(msg);

                // Fill the agreement detail id on the shipping order line to link the device to customer
                soc.ShippingOrderLines.Items.Find(t => t.TechnicalProductId == d_tp).AgreementDetailId = hardwaread.Id.Value;
                soc.ShippingOrderLines.Items.Find(t => t.TechnicalProductId == s_tp).AgreementDetailId = hardwaread.Id.Value;

                soc.ShippingOrderLines.Items.Find(t => t.TechnicalProductId == d_tp).ReceivedQuantity = 1;
                soc.ShippingOrderLines.Items.Find(t => t.TechnicalProductId == s_tp).ReceivedQuantity = 1;

                soService.ShipOrder(soc, shipso_reason, null);
                //msg = "Shipping Order :" + soc.Id.Value + " has been shipped!";
                //logger.Info(msg);


                return soc;
            }
            catch (Exception ex)
            {
                //Console.WriteLine("Inner Exception : " + ex.InnerException.Message);
                //msg = "Errors : " + ex.Message + "  ------  Exception Stack : " + ex.StackTrace;
                //Console.WriteLine("Errors : " + ex.Message);
                //Console.WriteLine("Stack : " + ex.StackTrace);
                //logger.Error(msg);

                return null;
            }


        }

        public void MayShipSO(string username_ad, string password_ad, int custid)
        {
            Authentication_class var_auth = new Authentication_class();
            AuthenticationHeader authHeader = var_auth.getAuthHeader(username_ad, password_ad);
            AsmRepository.SetServiceLocationUrl(var_auth.var_service_location_url);

            var soService = AsmRepository.AllServices.GetOrderManagementService(authHeader);
            var faService = AsmRepository.AllServices.GetFinanceService(authHeader);
            var sbService = AsmRepository.AllServices.GetSandBoxManagerService(authHeader);
            var agService = AsmRepository.AllServices.GetAgreementManagementService(authHeader);
            var deviceService = AsmRepository.AllServices.GetDevicesService(authHeader);
            var viewfService = AsmRepository.AllServices.GetViewFacadeService(authHeader);

            string msg = "Update SO status to May Ship...";
            //Console.WriteLine(msg);
            //Logger.Info(msg);
            try
            {
                // get the pending shipping order and shipping order line
                var soc = soService.GetShippedOrders(custid, 0).Items.Find(t => t.StatusId == SOPENDING);

                if (soc == null)
                {
                    //Console.WriteLine("No pending shipping order please check!");
                    return;
                }

                soService.UpdateShippingOrderStatus(soc.Id.Value, mayship_reason);
            }
            catch (Exception ex)
            {

                //msg = "Errors : " + ex.Message + "  ------  Exception Stack : " + ex.StackTrace;
                //Console.WriteLine("Errors : " + ex.Message);
                //Console.WriteLine("Stack : " + ex.StackTrace);
                //logger.Error(msg);

            }

        }


        [HttpGet]
        [Route("api/{username_ad}/{password_ad}/shippingorder/GetPending_mayship_shippingordersByCustomerId/{cust_id}")]
        public IEnumerable<ShippingOrder> GetPending_mayship_shippingordersByCustomerId(String username_ad, String password_ad, int cust_id)
        {
            Authentication_class var_auth = new Authentication_class();
            AuthenticationHeader authHeader = var_auth.getAuthHeader(username_ad, password_ad);
            AsmRepository.SetServiceLocationUrl(var_auth.var_service_location_url);

            var soService = AsmRepository.AllServices.GetOrderManagementService(authHeader);
            var faService = AsmRepository.AllServices.GetFinanceService(authHeader);
            var sbService = AsmRepository.AllServices.GetSandBoxManagerService(authHeader);
            var agService = AsmRepository.AllServices.GetAgreementManagementService(authHeader);
            var deviceService = AsmRepository.AllServices.GetDevicesService(authHeader);
            var viewfService = AsmRepository.AllServices.GetViewFacadeService(authHeader);
            var prodcService = AsmRepository.AllServices.GetProductCatalogConfigurationService(authHeader);

            try
            {
                int so_shipped = 3;
                int so_pending = 1;
                int so_mayship = 21;
                int so_reject = 5;
                int so_approved = 2;
                string sn = "";

                var sos = soService.GetShippedOrders(cust_id, 0);

                List<ShippingOrder> shipped_so = sos.Items.FindAll(T => T.StatusId == so_shipped);

                List<ShippingOrder> pending_so = sos.Items.FindAll(T => T.StatusId == so_pending);

                List<ShippingOrder> mayship_so = sos.Items.FindAll(T => T.StatusId == so_mayship);

                List<ShippingOrder> mayship_pending = null;

                // get shipped ship order info.
                //foreach (var so in shipped_so)
                //{
                //    Console.WriteLine("Shipped Shipping Order ID : " + so.Id.Value);
                //    foreach (var item in so.ShippingOrderLines.Items)
                //    {
                //        var tp = prodcService.GetTechnicalProduct(item.TechnicalProductId.Value);
                //        var device = viewfService.GetDeviceView(new BaseQueryRequest()
                //        {
                //            FilterCriteria = Op.Eq("ShipOrderId", so.Id.Value) && Op.Eq("TechnicalProductName", tp.Name),
                //            PageCriteria = new PageCriteria()
                //            {
                //                Page = 0,
                //                PageSize = 5
                //            }

                //        });

                //        if (device.TotalCount > 0)
                //        {
                //            sn = device.Items[0].SerialNumber;
                //        }

                //        Console.WriteLine("Item Hardware Model ID : " + item.HardwareModelId.Value + " Serial Number : " + sn);
                //        sn = "";
                //    }
                //}


                // get pending ship order

                //foreach (var so in pending_so)
                //{
                //    Console.WriteLine("Please update shipping order : " + so.Id.Value + " to May Ship status");
                //}


                // get may ship ship order
                //foreach (var so in mayship_so)
                //{
                //    Console.WriteLine("You can input the device info to the May Ship shippinng order : " + so.Id.Value);

                //    foreach (var item in so.ShippingOrderLines.Items)
                //    {
                //        var tp = prodcService.GetTechnicalProduct(item.TechnicalProductId.Value);

                //        Console.WriteLine("Techinical Product : " + tp.Description);

                //    }
                //}
                IEnumerable<ShippingOrder> union = pending_so.Union(mayship_so);

                return union;

            }
            catch (Exception ex)
            {
                return null;
                //msg = "Errors : " + ex.Message + "  ------  Exception Stack : " + ex.StackTrace;
                //Console.WriteLine("Errors : " + ex.Message);
                //Console.WriteLine("Stack : " + ex.StackTrace);
                //logger.Error(msg);
            }
        }

        [HttpGet]
        [Route("api/{username_ad}/{password_ad}/shippingorder/GetSerialNumberDevices_shippingordersByCustomerId/{cust_id}")]
        public List<Hardware_list> GetSerialNumberDevices_shippingordersByCustomerId(String username_ad, String password_ad, int cust_id)
        {
            List<Hardware_list> list1 = new List<Hardware_list>();


            Authentication_class var_auth = new Authentication_class();
            AuthenticationHeader authHeader = var_auth.getAuthHeader(username_ad, password_ad);
            AsmRepository.SetServiceLocationUrl(var_auth.var_service_location_url);

            var soService = AsmRepository.AllServices.GetOrderManagementService(authHeader);
            var faService = AsmRepository.AllServices.GetFinanceService(authHeader);
            var sbService = AsmRepository.AllServices.GetSandBoxManagerService(authHeader);
            var agService = AsmRepository.AllServices.GetAgreementManagementService(authHeader);
            var deviceService = AsmRepository.AllServices.GetDevicesService(authHeader);
            var viewfService = AsmRepository.AllServices.GetViewFacadeService(authHeader);
            var prodcService = AsmRepository.AllServices.GetProductCatalogConfigurationService(authHeader);

            try
            {
                int so_shipped = 3;
                int so_pending = 1;
                int so_mayship = 21;
                int so_reject = 5;
                int so_approved = 2;
                string sn = "";

                var sos = soService.GetShippedOrders(cust_id, 0);

                List<ShippingOrder> shipped_so = sos.Items.FindAll(T => T.StatusId == so_shipped);

                List<ShippingOrder> pending_so = sos.Items.FindAll(T => T.StatusId == so_pending);

                List<ShippingOrder> mayship_so = sos.Items.FindAll(T => T.StatusId == so_mayship);

                List<ShippingOrder> mayship_pending = null;

                // get shipped ship order info.
                foreach (var so in shipped_so)
                {
                    //Console.WriteLine("Shipped Shipping Order ID : " + so.Id.Value);
                    foreach (var item in so.ShippingOrderLines.Items)
                    {
                        var tp = prodcService.GetTechnicalProduct(item.TechnicalProductId.Value);
                        var device = viewfService.GetDeviceView(new BaseQueryRequest()
                        {
                            FilterCriteria = Op.Eq("ShipOrderId", so.Id.Value) && Op.Eq("TechnicalProductName", tp.Name),
                            PageCriteria = new PageCriteria()
                            {
                                Page = 0,
                                PageSize = 5
                            }

                        });

                        if (device.TotalCount > 0)
                        {
                            sn = device.Items[0].SerialNumber;
                        }
                        list1.Add(new Hardware_list() {description = tp.Description, serial_number = sn, hardware_model_id = item.HardwareModelId.Value });
                        //Console.WriteLine("Item Hardware Model ID : " + item.HardwareModelId.Value + " Serial Number : " + sn);
                        sn = "";
                    }
                }

                

                return list1;

            }
            catch (Exception ex)
            {
                return null;
                //msg = "Errors : " + ex.Message + "  ------  Exception Stack : " + ex.StackTrace;
                //Console.WriteLine("Errors : " + ex.Message);
                //Console.WriteLine("Stack : " + ex.StackTrace);
                //logger.Error(msg);
            }
        }
    }
}
