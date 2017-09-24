using PayMedia.ApplicationServices.ClientProxy;
using PayMedia.ApplicationServices.Customers.ServiceContracts.DataContracts;
using PayMedia.ApplicationServices.SharedContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using web_api_icc_valsys_no_mvc.Models;

namespace web_api_icc_valsys_no_mvc.Controllers
{
    public class ServiceTechnicianController : ApiController
    {
        [HttpGet]
        [Route("api/{username_ad}/{password_ad}/servicetechnician/GetServiceTechniciansByServProvId/{serv_prov_id_params}")]
        public HttpResponseMessage GetServiceTechnician(int serv_prov_id_params, String username_ad, String password_ad)
        {
            Authentication_class var_auth = new Authentication_class();
            AuthenticationHeader authHeader = var_auth.getAuthHeader(username_ad, password_ad);
            AsmRepository.SetServiceLocationUrl(var_auth.var_service_location_url);

            var custservice = AsmRepository.AllServices.GetCustomersService(authHeader);

            AssociateCollection aa = custservice.GetAssociates(serv_prov_id_params, 0);


            if (aa != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, aa);
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
