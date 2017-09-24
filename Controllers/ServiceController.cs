using PayMedia.ApplicationServices.ClientProxy;
using PayMedia.ApplicationServices.SharedContracts;
using PayMedia.ApplicationServices.Workforce.ServiceContracts;
using PayMedia.ApplicationServices.Workforce.ServiceContracts.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using web_api_icc_valsys_no_mvc.Models;

namespace web_api_icc_valsys_no_mvc.Controllers
{
    public class ServiceController : ApiController
    {
        [HttpGet]
        [Route("api/{username_ad}/{password_ad}/service/GetServiceTypes")]
        public HttpResponseMessage GetServiceTypes(String username_ad, String password_ad)
        {
            Authentication_class var_auth = new Authentication_class();
            AuthenticationHeader ah = var_auth.getAuthHeader(username_ad, password_ad);
            AsmRepository.SetServiceLocationUrl(var_auth.var_service_location_url);
            var TSService = AsmRepository.GetServiceProxyCachedOrDefault<IWorkforceConfigurationService>(ah);

            BaseQueryRequest request = new BaseQueryRequest();
            request.FilterCriteria = new CriteriaCollection();
            request.FilterCriteria.Add(new Criteria("Active", 1));

            ServiceCollection service= TSService.GetServices(request);

            if (service != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, service);
            }
            else
            {
                var message = string.Format("error");
                HttpError err = new HttpError(message);
                return Request.CreateResponse(HttpStatusCode.OK, message);
            }
            

        }
        [HttpGet]
        [Route("api/{username_ad}/{password_ad}/service/GetServicesByServiceTypeId/{id}")]
        public HttpResponseMessage GetServicesByServiceTypeId(String username_ad, String password_ad, int id)
        {
            Authentication_class var_auth = new Authentication_class();
            AuthenticationHeader ah = var_auth.getAuthHeader(username_ad, password_ad);
            AsmRepository.SetServiceLocationUrl(var_auth.var_service_location_url);
            var TSService = AsmRepository.GetServiceProxyCachedOrDefault<IWorkforceConfigurationService>(ah);

            BaseQueryRequest request = new BaseQueryRequest();
            request.FilterCriteria = new CriteriaCollection(); 
            request.FilterCriteria.Add(new Criteria("Active", 1));
            request.FilterCriteria.Add(new Criteria("ServiceTypeId", id));

            ServiceCollection service = TSService.GetServices(request);

            if (service != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, service);
            }
            else
            {
                var message = string.Format("error");
                HttpError err = new HttpError(message);
                return Request.CreateResponse(HttpStatusCode.OK, message);
            }
            

        }

        [HttpGet]
        [Route("api/{username_ad}/{password_ad}/service/GetServicesByServiceId/{id}")]
        public HttpResponseMessage GetServicesByServiceId(String username_ad, String password_ad, int id)
        {
            Authentication_class var_auth = new Authentication_class();
            AuthenticationHeader ah = var_auth.getAuthHeader(username_ad, password_ad);
            AsmRepository.SetServiceLocationUrl(var_auth.var_service_location_url);
            var TSService = AsmRepository.GetServiceProxyCachedOrDefault<IWorkforceConfigurationService>(ah);

            BaseQueryRequest request = new BaseQueryRequest();
            request.FilterCriteria = new CriteriaCollection();
            request.FilterCriteria.Add(new Criteria("Active", 1));
            request.FilterCriteria.Add(new Criteria("Id", id));

            ServiceCollection service = TSService.GetServices(request);

            if (service != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, service);
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
