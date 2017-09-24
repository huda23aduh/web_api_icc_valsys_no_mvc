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
    public class ServiceTypeController : ApiController
    {

        [HttpGet]
        [Route("api/{username_ad}/{password_ad}/servicetype/GetServiceTypes")]
        public HttpResponseMessage GetServiceTypes(String username_ad, String password_ad)
        {
            Authentication_class var_auth = new Authentication_class();
            AuthenticationHeader ah = var_auth.getAuthHeader(username_ad, password_ad);
            AsmRepository.SetServiceLocationUrl(var_auth.var_service_location_url);
            var TSService = AsmRepository.GetServiceProxyCachedOrDefault<IWorkforceConfigurationService>(ah);

            BaseQueryRequest request = new BaseQueryRequest();
            request.FilterCriteria = new CriteriaCollection();
            request.FilterCriteria.Add(new Criteria("IconId", 0));

            ServiceTypeCollection servicetype = TSService.GetServiceTypeList(request);

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
        [Route("api/{username_ad}/{password_ad}/servicetype/GetServiceTypeById/{id}")]
        public HttpResponseMessage GetServiceTypeById(String username_ad, String password_ad, int id)
        {
            Authentication_class var_auth = new Authentication_class();
            AuthenticationHeader ah = var_auth.getAuthHeader(username_ad, password_ad);
            AsmRepository.SetServiceLocationUrl(var_auth.var_service_location_url);
            var TSService = AsmRepository.GetServiceProxyCachedOrDefault<IWorkforceConfigurationService>(ah);

            BaseQueryRequest request = new BaseQueryRequest();
            request.FilterCriteria = new CriteriaCollection();
            request.FilterCriteria.Add(new Criteria("Id", id));

            ServiceTypeCollection servicetype = TSService.GetServiceTypeList(request);

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
    }
}
