using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Globalization;

using PayMedia.ApplicationServices.SharedContracts;
using PayMedia.ApplicationServices.ServiceLocation.ServiceContracts;
using PayMedia.ApplicationServices.ClientProxy;
using PayMedia.ApplicationServices.Customers.ServiceContracts;
using PayMedia.ApplicationServices.Customers.ServiceContracts.DataContracts;
using PayMedia.ApplicationServices.Workforce.ServiceContracts;
using PayMedia.ApplicationServices.Workforce.ServiceContracts.DataContracts;

using web_api_icc_valsys_no_mvc.Models;

namespace web_api_icc_valsys_no_mvc.Controllers
{
    public class TimeSlotController : ApiController
    {
       
        [HttpPost]
        [Route("api/{username_ad}/{password_ad}/timeslot/GetTimeSlotInfo")] 
        public TimeSlotAvailability GetTimeSlotInfo([FromBody]TimeSlotParams time_slot_param)
        {
            Authentication_class var_auth = new Authentication_class();
            AuthenticationHeader ah = var_auth.getAuthHeader(time_slot_param.username_ad, time_slot_param.password_ad);
            AsmRepository.SetServiceLocationUrl(var_auth.var_service_location_url);

            IWorkforceService woService = AsmRepository.AllServices.GetWorkforceService(ah);
            ICustomersService customerService = AsmRepository.GetServiceProxyCachedOrDefault<ICustomersService>(ah);

            Customer cust = new Customer();
            cust = customerService.GetCustomer(time_slot_param.customer_id);

            var sp = woService.FindSPServiceWithGeoInfo(new FindSPServiceCriteria()
            {
                AddressId = time_slot_param.serv_addr_id,
                ServiceTypeId = time_slot_param.serv_type_id,
                Active = true,
                PageSize = 100
            },
            1);

            TimeSlotAvailability var_timeslotAvailability = new TimeSlotAvailability();
            var var_timeslotAvailabilityItem = new List<TimeSlotAvailabilityItem>();

            foreach (SPServiceWithGeoInfo serviceprovider in sp.Items)
            {
                
                var sps = woService.GetServiceProviderServiceByServiceTypeGeoDefGroupIdandProviderId(serviceprovider.ServiceTypeId.Value, serviceprovider.GeoDefinitionGroupId.Value, serviceprovider.ServiceProviderId.Value);
                TimeSlotDescription[] timeslot = woService.GetTimeSlotsByServiceProviderServiceId(sps.Id.Value, DateTime.Now);
                // print the timeslot for this service

                var_timeslotAvailabilityItem.Add(new TimeSlotAvailabilityItem()
                {
                    the_SPServiceWithGeoInfo = serviceprovider,
                    the_timeslot = timeslot
                });
            }
            var_timeslotAvailability.TimeItemsData = var_timeslotAvailabilityItem;

            return var_timeslotAvailability;
        }

        [HttpPost]
        [Route("api/{username_ad}/{password_ad}/timeslot/GetTimeSlotInfoWithServProvIdParams")] 
        public TimeSlotAvailability GetTimeSlotInfoWithServProvIdParams([FromBody]TimeSlotParams time_slot_param)
        {
            Authentication_class var_auth = new Authentication_class();
            AuthenticationHeader ah = var_auth.getAuthHeader(time_slot_param.username_ad, time_slot_param.password_ad);
            AsmRepository.SetServiceLocationUrl(var_auth.var_service_location_url);

            IWorkforceService woService = AsmRepository.AllServices.GetWorkforceService(ah);
            ICustomersService customerService = AsmRepository.GetServiceProxyCachedOrDefault<ICustomersService>(ah);

            DateTime dt = DateTime.ParseExact(time_slot_param.Date, "yyyy-MM-dd", CultureInfo.InvariantCulture);

            Customer cust = new Customer();
            cust = customerService.GetCustomer(time_slot_param.customer_id);

            var sp = woService.FindSPServiceWithGeoInfo(new FindSPServiceCriteria()
            {
                AddressId = time_slot_param.serv_addr_id,
                ServiceTypeId = time_slot_param.serv_type_id,
                ServiceProviderId = time_slot_param.serv_prov_id,
                Active = true,
                PageSize = 100
            },
            1);

            TimeSlotAvailability var_timeslotAvailability = new TimeSlotAvailability();
            var var_timeslotAvailabilityItem = new List<TimeSlotAvailabilityItem>();

            foreach (SPServiceWithGeoInfo serviceprovider in sp.Items)
            {

                var sps = woService.GetServiceProviderServiceByServiceTypeGeoDefGroupIdandProviderId(serviceprovider.ServiceTypeId.Value, serviceprovider.GeoDefinitionGroupId.Value, serviceprovider.ServiceProviderId.Value);
                TimeSlotDescription[] timeslot = woService.GetTimeSlotsByServiceProviderServiceId(sps.Id.Value, dt);
                // print the timeslot for this service

                var_timeslotAvailabilityItem.Add(new TimeSlotAvailabilityItem()
                {
                    the_SPServiceWithGeoInfo = serviceprovider,
                    the_timeslot = timeslot
                });
            }
            var_timeslotAvailability.TimeItemsData = var_timeslotAvailabilityItem;

            return var_timeslotAvailability;
        }

        




    }
}
