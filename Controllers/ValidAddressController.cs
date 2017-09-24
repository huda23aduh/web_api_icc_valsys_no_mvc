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
using PayMedia.ApplicationServices.Customers.ServiceContracts.DataContracts;
using PayMedia.ApplicationServices.Customers.ServiceContracts;
using web_api_icc_valsys_no_mvc.Models;

namespace web_api_icc_valsys_no_mvc.Controllers
{
    public class ValidAddressController : ApiController
    {
        
        [HttpGet]
        [ActionName("GetValidAddressById")]
        [Route("api/{username_ad}/{password_ad}/validaddress/GetValidAddressById/{id}")]
        public HttpResponseMessage GetValidAddressById(int id, String username_ad, String password_ad)
        {
            Authentication_class var_auth = new Authentication_class();
            AuthenticationHeader authHeader = var_auth.getAuthHeader(username_ad, password_ad);
            AsmRepository.SetServiceLocationUrl(var_auth.var_service_location_url);

            var customersConfiguration = AsmRepository.GetServiceProxyCachedOrDefault<ICustomersConfigurationService>(authHeader);
            int val_addr_id = id;
            ValidAddress var_valid_addr = customersConfiguration.GetValidAddress(val_addr_id);

            if (var_valid_addr != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, var_valid_addr);
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
