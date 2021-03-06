﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using PayMedia.ApplicationServices.ClientProxy;
using PayMedia.ApplicationServices.SharedContracts;
using PayMedia.ApplicationServices.Devices.ServiceContracts.DataContracts;
using PayMedia.ApplicationServices.Devices.ServiceContracts;
using web_api_icc_valsys_no_mvc.Models;
using PayMedia.ApplicationServices.OrderManagement.ServiceContracts.DataContracts;
using PayMedia.ApplicationServices.ViewFacade.ServiceContracts.DataContracts;

namespace web_api_icc_valsys_no_mvc.Controllers
{
    public class DeviceController : ApiController
    {
        //string msg = null;
        string QuoteAccountTypeId = "1";
        //string WalletAccountTypeId = "2";
        const int SOPENDING = 1;
        const int SOMAYSHIP = 21;
        const int DECODER_TP_ID = 3;   // 3 Decoder SD, 2 : Decoder HD
        const int SCTPID = 7;
        const string DEVICE_STATUS_STOCK = "1";
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

        // GET api/values
        //public IEnumerable<string> Get()
        //{
        //    Authentication_class var_auth = new Authentication_class();
        //    AuthenticationHeader authHeader = var_auth.getAuthHeader(username_ad, password_ad);
        //    AsmRepository.SetServiceLocationUrl(var_auth.var_service_location_url);

        //    var custService = AsmRepository.AllServices.GetCustomersConfigurationService(authHeader);
        //    var devicesService = AsmRepository.GetServiceProxyCachedOrDefault<IDevicesService>(authHeader);

        //    FindDeviceCriteria criteria = new FindDeviceCriteria();

        //    var provinces = custService.GetProvinceByCountry(1);
        //    var devices = devicesService.GetType();


        //    List<string> list = new List<string>();
        //    //Provinces ok1 = new Provinces();

        //    // Loop through List with foreach.
        //    //foreach (var device in devices)
        //    //{
        //    //    var a = province.Id.ToString();
        //    //    list.Add(a);
        //    //    var b = province.Name.ToString();
        //    //    list.Add(b);
        //    //}

        //    //var provinces_listString = provinces.OrderBy(x => x.Id).Select(x => new Provinces { Id_nya = (int) x.Id, Name_nya = (string) x.Name }).ToList();
        //    //var result = ((IEnumerable<string>)provinces).Cast<object>().ToList();
        //    return list;
        //}

        [HttpGet]
        //[ActionName("GetDeviceById")]
        [Route("api/{username_ad}/{password_ad}/device/GetDeviceById/{id}")]
        public HttpResponseMessage GetDeviceById(int id, String username_ad, String password_ad)
        {
            Authentication_class var_auth = new Authentication_class();
            AuthenticationHeader authHeader = var_auth.getAuthHeader(username_ad, password_ad);
            AsmRepository.SetServiceLocationUrl(var_auth.var_service_location_url);

            var devicesService = AsmRepository.GetServiceProxyCachedOrDefault<IDevicesService>(authHeader);
            //var provinces = devicesService.GetDeviceById(id); //parameter id
            Device devices = devicesService.GetDeviceById(id);

            //List<string> list = new List<string>();
            //list.Add("deviceId"); list.Add(devices.Id.ToString());
            //list.Add("SerialNumber"); list.Add(devices.SerialNumber);
            //list.Add("Shidate"); list.Add(devices.ShipDate.ToString());
            //list.Add("StatusId"); list.Add(devices.StatusId.ToString());
            //list.Add("ModelId"); list.Add(devices.ModelId.ToString());
            //list.Add("BigCarReferenceNumber"); list.Add(devices.BigCAReferenceNumber);

            if (devices != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, devices);
            }
            else
            {
                var message = string.Format("error");
                HttpError err = new HttpError(message);
                return Request.CreateResponse(HttpStatusCode.OK, message);
            }

            //return devices;
        }

        [HttpGet]
        //[ActionName("GetDeviceBySerialNumber")]
        [Route("api/{username_ad}/{password_ad}/device/GetDeviceBySerialNumber/{id}")]
        public Device GetDeviceBySerialNumber(String username_ad, String password_ad, string id)
        {
            Authentication_class var_auth = new Authentication_class();
            AuthenticationHeader authHeader = var_auth.getAuthHeader(username_ad, password_ad);
            AsmRepository.SetServiceLocationUrl(var_auth.var_service_location_url);

            var devicesService = AsmRepository.GetServiceProxyCachedOrDefault<IDevicesService>(authHeader);
            
            //string serialNumber = "2000000000010";
            Device devices = devicesService.GetDeviceBySerialNumber(id);

            //List<string> list = new List<string>();
            //list.Add("deviceId"); list.Add(devices.Id.ToString());
            //list.Add("SerialNumber"); list.Add(devices.SerialNumber);
            //list.Add("Shidate"); list.Add(devices.ShipDate.ToString());
            //list.Add("StatusId"); list.Add(devices.StatusId.ToString());
            //list.Add("ModelId"); list.Add(devices.ModelId.ToString());
            //list.Add("BigCarReferenceNumber"); list.Add(devices.BigCAReferenceNumber);

            return devices;

            //if (devices != null)
            //{
            //    return Request.CreateResponse(HttpStatusCode.OK, devices);
            //}
            //else
            //{
            //    var message = string.Format("error");
            //    HttpError err = new HttpError(message);
            //    return Request.CreateResponse(HttpStatusCode.OK, message);
            //}

            //return devices;
        }

        //API FOR PRE ACTIVATION
        [HttpGet]
        [Route("api/{username_ad}/{password_ad}/device/SendCommandToDevice/{serial_number}/{id_action}")]
        public HttpResponseMessage SendCommandToDevice(string serial_number, int id_action, String username_ad, String password_ad)
        {
            Authentication_class var_auth = new Authentication_class();
            AuthenticationHeader authHeader = var_auth.getAuthHeader(username_ad, password_ad);
            AsmRepository.SetServiceLocationUrl(var_auth.var_service_location_url);

            var the_string = "";
            
            var deviceService = AsmRepository.AllServices.GetDevicesService(authHeader);
            var viewfService = AsmRepository.AllServices.GetViewFacadeService(authHeader);

            //int pre_act_7 = 99177; // pre-activation for 7 days.
            var device = deviceService.GetDeviceBySerialNumber(serial_number);
            if (device != null)
            {
                deviceService.SendCommandToDevice(device.Id.Value, id_action);

                
                return Request.CreateResponse(HttpStatusCode.OK, "SUCCESS");

            }
            else
            {
                var message = string.Format("An Error Has Occured on serial number", serial_number);
                HttpError err = new HttpError(message);
                return Request.CreateResponse(HttpStatusCode.BadRequest, err);
                //Console.WriteLine("Can't find device with SN :" + serial_number);
            }
        }

        
        [HttpGet]
        [Route("api/{username_ad}/{password_ad}/device/retrieveDevice/{serial_number}")]
        public Device retrieveDevice(String username_ad, String password_ad, string serial_number)
        {

            Authentication_class var_auth = new Authentication_class();
            AuthenticationHeader ah = var_auth.getAuthHeader(username_ad, password_ad);
            AsmRepository.SetServiceLocationUrl(var_auth.var_service_location_url);

            var devicesService = AsmRepository.GetServiceProxyCachedOrDefault<IDevicesService>(ah);

            string device_serial_number = serial_number;    //device serial number
            int reason156_RetriveDevice = 99146;
            int reason183_Returned = 99149;
            int reason156_InStock = 99152;
            int reason156_ReturnToManufactory = 99151;
            int reason156_RepairedStock = 99153;
            int reason156_RefurbishedStock = 99154;
            int reason156_Damaged = 99155;

            int to_stockhandler_id = 1015;   // the destination stocck handler of the device

            //var deviceh = new deviceHandler(authHeader);

            // First step - change to to be return
            updateDeviceStatus(username_ad, password_ad, device_serial_number, reason156_RetriveDevice);

            // Second step - change stock handler
            transferDevice(username_ad, password_ad, reason183_Returned, to_stockhandler_id, device_serial_number);


            // Third Step - option 1 : change to in stock
            updateDeviceStatus(username_ad, password_ad, device_serial_number, reason156_InStock);

            // Third Step - option 2 : change to repaired stock
            //var the_device = updateDeviceStatus(username_ad, password_ad, device_serial_number, reason156_RepairedStock);

            // Third Step - option 3 : change to refurbished stock
            //var the_device = updateDeviceStatus(username_ad, password_ad, device_serial_number, reason156_RefurbishedStock);

            // Third Step - option 4 : change to return to manufactory
            //var the_device = updateDeviceStatus(username_ad, password_ad, device_serial_number, reason156_ReturnToManufactory);

            // Third Step - option 5 : change to damaged
            //var the_device = updateDeviceStatus(username_ad, password_ad, device_serial_number, reason156_Damaged);


            //-----------       ---------------
            Device devices = devicesService.GetDeviceBySerialNumber(serial_number);

            return devices;

        }
        
        // Update device status(event 156)
        public void updateDeviceStatus(string username_ad, string password_ad, string serial_number, int reason_id)
        {
            Authentication_class var_auth = new Authentication_class();
            AuthenticationHeader authHeader = var_auth.getAuthHeader(username_ad, password_ad);
            AsmRepository.SetServiceLocationUrl(var_auth.var_service_location_url);

            var whService = AsmRepository.AllServices.GetLogisticsService(authHeader);
            var prodcService = AsmRepository.AllServices.GetProductCatalogConfigurationService(authHeader);
            IDevicesService deviceService = AsmRepository.AllServices.GetDevicesService(authHeader);

            int return_status = 0;

            try
            {
                Device device = deviceService.GetDeviceBySerialNumber(serial_number);
                if (device != null)
                {
                    deviceService.UpdateDeviceStatus(device.Id.Value, reason_id);
                    return_status = 1;
                    //Console.WriteLine("device id = " + device.Id, "serial number = " + device.SerialNumber);
                }
                else
                {
                    return_status = 0;
                    //Console.WriteLine("Can't find device with serial number : " + serial_number);
                }
            }
            catch (Exception ex)
            {
                //return null;
                //Console.WriteLine("Errors : " + ex.Message);
                //Console.WriteLine("Stack : " + ex.StackTrace);
            }

        }

        public void transferDevice(string username_ad, string password_ad, int reason183, int to_stockhandler_id, string sn)
        {
            Authentication_class var_auth = new Authentication_class();
            AuthenticationHeader authHeader = var_auth.getAuthHeader(username_ad, password_ad);
            AsmRepository.SetServiceLocationUrl(var_auth.var_service_location_url);

            var whService = AsmRepository.AllServices.GetLogisticsService(authHeader);
            var prodcService = AsmRepository.AllServices.GetProductCatalogConfigurationService(authHeader);
            var deviceService = AsmRepository.AllServices.GetDevicesService(authHeader);

            try
            {
                BuildList bl = new BuildList();
                bl.TransactionType = BuildListTransactionType.TransferToAnotherStockHandler;
                bl.Reason = reason183;
                bl.StockHandlerId = to_stockhandler_id;


                // Create build list
                var nbl = deviceService.CreateBuildList(bl);

                // Add device to build list
                deviceService.AddDeviceToBuildListManually(nbl.Id.Value, sn);

                // Perform build list
                var bl1 = deviceService.PerformBuildListAction(nbl.Id.Value);

            }
            catch (Exception ex)
            {
                Console.WriteLine("Errors : " + ex.Message);
                Console.WriteLine("Stack : " + ex.StackTrace);
            }
        }

        // import devices to stock handler
        public void importDevices(string username_ad, string password_ad)
        {
            Authentication_class var_auth = new Authentication_class();
            AuthenticationHeader authHeader = var_auth.getAuthHeader(username_ad, password_ad);
            AsmRepository.SetServiceLocationUrl(var_auth.var_service_location_url);

            var whService = AsmRepository.AllServices.GetLogisticsService(authHeader);
            var prodcService = AsmRepository.AllServices.GetProductCatalogConfigurationService(authHeader);
            var deviceService = AsmRepository.AllServices.GetDevicesService(authHeader);

            try
            {

                var stockhandlers = whService.GetStockHandlers(new BaseQueryRequest()
                {
                    FilterCriteria = Op.Eq("Active", true),
                    PageCriteria = new PageCriteria()
                    {
                        Page = 1,
                        PageSize = 100
                    }
                });

                var models = prodcService.GetHardwareModels(new BaseQueryRequest()
                {
                    FilterCriteria = Op.Like("Description", "%Decoder%"),
                    PageCriteria = new PageCriteria()
                    {
                        Page = 1,
                        PageSize = 100
                    }
                });

                //models.Items.Sort();

                foreach (var model in models.Items)
                {
                    string start_sn = "ID" + model.Id.Value + "YY";
                    int count = 100;

                    StockReceiveDetails stockReceiveDetails = new StockReceiveDetails();
                    stockReceiveDetails.FromStockHanderId = 1;
                    stockReceiveDetails.ToStockHanderId = 1;
                    stockReceiveDetails.Reason = 71197;
                    stockReceiveDetails = deviceService.CreateStockReceiveDetails(stockReceiveDetails);


                    BuildList bl = new BuildList();
                    bl.TransactionType = BuildListTransactionType.ReceiveNewStock;
                    bl.Reason = 71197;
                    bl.ModelId = model.Id.Value;
                    bl.StockReceiveDetailsId = stockReceiveDetails.Id.Value;

                    // Create build list
                    var nbl = deviceService.CreateBuildList(bl);

                    // Add device to build list
                    for (int i = 0; i < 300; i++)
                    {
                        string sn = start_sn + count.ToString().PadLeft(model.HardwareNumberLength.Value - start_sn.Length, '0');
                        Console.WriteLine(sn);
                        count++;
                        deviceService.AddDeviceToBuildListManually(nbl.Id.Value, sn);
                    }

                    // Perform build list
                    var bl1 = deviceService.PerformBuildListAction(nbl.Id.Value);

                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Errors : " + ex.Message);
                Console.WriteLine("Stack : " + ex.StackTrace);
            }
        }

    }
}
