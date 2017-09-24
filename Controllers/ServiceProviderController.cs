using PayMedia.ApplicationServices.ClientProxy;
using PayMedia.ApplicationServices.SharedContracts;
using PayMedia.ApplicationServices.Workforce.ServiceContracts;
using PayMedia.ApplicationServices.Workforce.ServiceContracts.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using web_api_icc_valsys_no_mvc.Models;

namespace web_api_icc_valsys_no_mvc.Controllers
{
    public class ServiceProviderController : ApiController
    {
        [HttpGet]
        [Route("api/{username_ad}/{password_ad}/serviceprovider/GetServiceProviders")]
        public List<ServiceProviderCollection> GetServiceProviders(String username_ad, String password_ad)
        {
            //HttpContext.Current.Server.ScriptTimeout = 300;
            Authentication_class var_auth = new Authentication_class();
            AuthenticationHeader authHeader = var_auth.getAuthHeader(username_ad, password_ad);
            AsmRepository.SetServiceLocationUrl(var_auth.var_service_location_url);

            var sp_Service = AsmRepository.GetServiceProxyCachedOrDefault<IWorkforceService>(authHeader);
            int i = 0;
            int ada = 0;
            int total_data = 0;
            List<ServiceProviderCollection> initialList = new List<ServiceProviderCollection>();

            #region get service provider

            do
            {
                var serv_providers = sp_Service.GetServiceProviders(i);
                if (serv_providers.Items.Capacity > 0)
                {
                    ada = 1;
                    initialList.Add(serv_providers);

                    total_data = total_data + serv_providers.Count();
                    i++;
                    //Console.WriteLine("total sp = {0} ----", serv_providers.Count());
                }
                else if (serv_providers.Items.Capacity == 0)
                {
                    ada = 0;
                    //Console.WriteLine("end of program");
                    break;
                }
                else
                {
                    return null;
                    //Console.WriteLine("Cannot find a sp.");
                }


            } while (ada != 0);

            return initialList;
            //Console.WriteLine("total data = {0} ----", total_data);
            #endregion

        }

        [HttpGet]
        [Route("api/{username_ad}/{password_ad}/serviceprovider/GetServiceProviderByServiceProviderId/{id}")]
        public HttpResponseMessage GetServiceProviderByServiceProviderId(String username_ad, String password_ad, int id)
        {
            Authentication_class var_auth = new Authentication_class();
            AuthenticationHeader authHeader = var_auth.getAuthHeader(username_ad, password_ad);
            AsmRepository.SetServiceLocationUrl(var_auth.var_service_location_url);

            var sp_Service = AsmRepository.GetServiceProxyCachedOrDefault<IWorkforceService>(authHeader);
            int i = 0;
            int ada = 0;
            int total_data = 0;
            ServiceProvider datanya = null;

            #region get service provider

            //do
            //{

            //    var serv_providers = sp_Service.GetServiceProviders(i);

            //    if (serv_providers.Items.Capacity > 0)
            //    {
            //        ada = 1;
            //        foreach (var c in serv_providers)
            //        {
            //            if(c.Id == id)
            //            {
            //                datanya = c;
            //                break;
            //            }
                        
            //        }

            //        total_data = total_data + serv_providers.Count();
            //        i++;
            //        //Console.WriteLine("total sp = {0} ----", serv_providers.Count());
            //    }
            //    else if (serv_providers.Items.Capacity == 0)
            //    {
            //        ada = 0;
            //        //Console.WriteLine("end of program");
            //        break;
            //    }
            //    else
            //    {
            //        return null;
            //        //Console.WriteLine("Cannot find a sp.");
            //    }


            //} while (ada != 0);

            //return datanya;
            //Console.WriteLine("total data = {0} ----", total_data);
            #endregion


            #region get_service_provider2
            var serv_providers = sp_Service.GetServiceProvider(id);

            if (serv_providers != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, serv_providers);
            }
            else
            {
                var message = string.Format("error");
                HttpError err = new HttpError(message);
                return Request.CreateResponse(HttpStatusCode.OK, message);
            }
            


            #endregion get_service_provider2

        }

        [HttpGet]
        [Route("api/{username_ad}/{password_ad}/serviceprovider/GetServiceProvAndTimeSlot/{address_id}/{service_type_id}")]
        public List<ServProvTimeSlot_response> GetServiceProvAndTimeSlot(String username_ad, String password_ad, int address_id, int service_type_id)
        {
            #region Authenticate and create proxies
            Authentication_class var_auth = new Authentication_class();
            AuthenticationHeader ah = var_auth.getAuthHeader(username_ad, password_ad);
            AsmRepository.SetServiceLocationUrl(var_auth.var_service_location_url);
            var custService = AsmRepository.AllServices.GetCustomersService(ah);
            var custcService = AsmRepository.AllServices.GetCustomersConfigurationService(ah);
            var woService = AsmRepository.AllServices.GetWorkforceService(ah);


            #endregion

            var sp = woService.FindSPServiceWithGeoInfo(new FindSPServiceCriteria()
            {
                AddressId = address_id,
                ServiceTypeId = service_type_id,
                Active = true,
                PageSize = 100
            },
            1);

            //return sp.Items;

            List<ServProvTimeSlot_response> var_sp_and_timeslot = new List<ServProvTimeSlot_response>();

            foreach (var serviceprovider in sp.Items)
            {
                Console.WriteLine("Service Provider Id : " + serviceprovider.ServiceProviderId + "  Name : " + serviceprovider.ServiceProviderName + "  Service Type Id : " + serviceprovider.ServiceTypeId + " Service ID : " + serviceprovider.ServiceId + " Geo Group Id :" + serviceprovider.GeoDefinitionGroupId);

                ServiceProviderService sps = woService.GetServiceProviderServiceByServiceTypeGeoDefGroupIdandProviderId(serviceprovider.ServiceTypeId.Value, serviceprovider.GeoDefinitionGroupId.Value, serviceprovider.ServiceProviderId.Value);
                TimeSlotDescription[] timeslot = woService.GetTimeSlotsByServiceProviderServiceId(sps.Id.Value, DateTime.Now);
                // print the timeslot for this service
                for (int i = 0; i < timeslot.Length; i++)
                {
                    Console.WriteLine("Date :" + timeslot[i].Date + "  Time Slot ID : " + timeslot[i].TimeSlotId + " Service Provider Service Id : " + timeslot[i].ServiceProviderServiceId + "Remain Slot : " + timeslot[i].RemainingCapacity + " Is Avaiable : " + timeslot[i].IsAvailable);
                }

                var_sp_and_timeslot.Add(new ServProvTimeSlot_response
                {
                    the_timeslot = timeslot,
                    the_service_provider = sps
                });
            }
            return var_sp_and_timeslot;

        }
    }
}
