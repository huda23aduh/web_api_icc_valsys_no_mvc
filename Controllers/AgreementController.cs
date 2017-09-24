using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using PayMedia.ApplicationServices.ViewFacade.ServiceContracts;
using PayMedia.ApplicationServices.ViewFacade.ServiceContracts.DataContracts;
using PayMedia.ApplicationServices.ClientProxy;
using PayMedia.ApplicationServices.ServiceLocation;
using PayMedia.ApplicationServices.SharedContracts;
using PayMedia.ApplicationServices.AgreementManagement.ServiceContracts;
using PayMedia.ApplicationServices.AgreementManagement.ServiceContracts.DataContracts;
using web_api_icc_valsys_no_mvc.Models;

namespace web_api_icc_valsys_no_mvc.Controllers
{
    public class AgreementController : ApiController
    {
        
        [HttpGet]
        [Route("api/{username_ad}/{password_ad}/agreement/GetAgreementByTypeId/{id}")]
        public HttpResponseMessage GetAgreementByTypeId(int id, String username_ad, String password_ad)
        {
            Authentication_class var_auth = new Authentication_class();
            AuthenticationHeader ah = var_auth.getAuthHeader(username_ad, password_ad);

            AsmRepository.SetServiceLocationUrl(var_auth.var_service_location_url);

            IViewFacadeService viewFacadeService = AsmRepository.GetServiceProxyCachedOrDefault<IViewFacadeService>(ah);

            BaseQueryRequest request = new BaseQueryRequest();
            request.FilterCriteria = new CriteriaCollection();

            //Agreement type is a user-configured property. The value in
            //this key-value pair is the agreement type ID. You can get this
            //value from the Configuration Module or from a call to
            //the AgreementManagementConfigurationService.
            request.FilterCriteria.Add(new Criteria("TypeId", id));
            AgreementViewCollection coll = viewFacadeService.GetAgreementView(request);

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
        [Route("api/{username_ad}/{password_ad}/agreement/GetAgreementByCustomerId/{id}")]
        public HttpResponseMessage GetAgreementByCustomerId(int id, String username_ad, String password_ad)
        {
            Authentication_class var_auth = new Authentication_class();
            AuthenticationHeader ah = var_auth.getAuthHeader(username_ad, password_ad);

            AsmRepository.SetServiceLocationUrl(var_auth.var_service_location_url);
            var agreementManagementService = AsmRepository.GetServiceProxyCachedOrDefault<IAgreementManagementService>(ah);
            //This value is the customer's ID number.
            int customerId = id;
            //In ICC, a Page can hold up to 20 records. Page = 0 returns ALL records.
            int page = 0;
            //Call the method and display the results.
            var agreements = agreementManagementService.GetAgreementsForCustomerId(customerId, page);

            if (agreements != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, agreements);
            }
            else
            {
                var message = string.Format("error");
                HttpError err = new HttpError(message);
                return Request.CreateResponse(HttpStatusCode.OK, message);
            }

            
        }

        [HttpGet]
        [Route("api/{username_ad}/{password_ad}/agreement/GetSoftwareProduct/{id}")]
        public HttpResponseMessage GetSoftwareProduct(int id, String username_ad, String password_ad)
        {
            Authentication_class var_auth = new Authentication_class();
            AuthenticationHeader ah = var_auth.getAuthHeader(username_ad, password_ad);

            AsmRepository.SetServiceLocationUrl(var_auth.var_service_location_url);
            var agreementManagementService = AsmRepository.GetServiceProxyCachedOrDefault<IAgreementManagementService>(ah);
            //This value is the customer's ID number.
            int customerId = id;
            //In ICC, a Page can hold up to 20 records. Page = 0 returns ALL records.
            int page = 0;
            //Call the method and display the results.
            var agreements = agreementManagementService.GetSoftwareForAgreementDetailById(2912043);
            if (agreements != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, agreements);
            }
            else
            {
                var message = string.Format("error");
                HttpError err = new HttpError(message);
                return Request.CreateResponse(HttpStatusCode.OK, message);
            }
        }

        [HttpGet]
        [Route("api/{username_ad}/{password_ad}/agreement/getAgreementDetailByCustId/{id}")]
        public HttpResponseMessage getAgreementDetailByCustId(int id, String username_ad, String password_ad)
        {
            Authentication_class var_auth = new Authentication_class();
            AuthenticationHeader ah = var_auth.getAuthHeader(username_ad, password_ad);

            AsmRepository.SetServiceLocationUrl(var_auth.var_service_location_url);
            var agreementManagementService = AsmRepository.GetServiceProxyCachedOrDefault<IAgreementManagementService>(ah);
            AgreementDetailCollection adc = agreementManagementService.GetAgreementDetailsForCustomer(id, 1);

            if (adc != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, adc);
            }
            else
            {
                var message = string.Format("error");
                HttpError err = new HttpError(message);
                return Request.CreateResponse(HttpStatusCode.OK, message);
            }

        }

        [HttpGet]
        [Route("api/{username_ad}/{password_ad}/agreement/getProductinAgreementDetailByCustId/{id}")]
        public HttpResponseMessage getProductinAgreementDetailByCustId(int id, String username_ad, String password_ad)
        {
            Authentication_class var_auth = new Authentication_class();
            AuthenticationHeader ah = var_auth.getAuthHeader(username_ad, password_ad);

            AsmRepository.SetServiceLocationUrl(var_auth.var_service_location_url);
            var agreementManagementService = AsmRepository.GetServiceProxyCachedOrDefault<IAgreementManagementService>(ah);
            var viewService = AsmRepository.GetServiceProxy<IViewFacadeService>(ah);

            var viewAg = viewService.GetAgreementDetailView(new BaseQueryRequest()
            {
                FilterCriteria = Op.Eq("CustomerId", id),
                PageCriteria = new PageCriteria()
                {
                    Page = 0,
                    PageSize = 100
                }
            });

            if (viewAg != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, viewAg);
            }
            else
            {
                var message = string.Format("error");
                HttpError err = new HttpError(message);
                return Request.CreateResponse(HttpStatusCode.OK, message);
            }
            


        }

        [HttpGet]
        [Route("api/{username_ad}/{password_ad}/agreement/getDeviceinAgreementDetailByCustId/{id}")]
        public HttpResponseMessage getDeviceinAgreementDetailByCustId(int id, String username_ad, String password_ad)
        {
            Authentication_class var_auth = new Authentication_class();
            AuthenticationHeader ah = var_auth.getAuthHeader(username_ad, password_ad);

            AsmRepository.SetServiceLocationUrl(var_auth.var_service_location_url);
            var agreementManagementService = AsmRepository.GetServiceProxyCachedOrDefault<IAgreementManagementService>(ah);
            var viewService = AsmRepository.GetServiceProxy<IViewFacadeService>(ah);

            var viewD = viewService.GetCustomerDeviceView(new BaseQueryRequest()
            {
                FilterCriteria = Op.Eq("CustomerId", id),
                PageCriteria = new PageCriteria()
                {
                    Page = 0,
                    PageSize = 100
                },
                DeepLoad = true

            });

            if (viewD != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, viewD);
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
