using PayMedia.ApplicationServices.ClientProxy;
using PayMedia.ApplicationServices.SharedContracts;
using PayMedia.ApplicationServices.ViewFacade.ServiceContracts;
using PayMedia.ApplicationServices.ViewFacade.ServiceContracts.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using web_api_icc_valsys_no_mvc.Models;

namespace web_api_icc_valsys_no_mvc.Controllers
{
    public class AddressController : ApiController
    {
        //static IViewFacadeService m_IViewFacadeService;

        //[HttpGet]
        //[Route("api/address/GetaddressEntityViewCollection/{username_ad}/{password_ad}/{cust_id}")]
        //public AddressEntityViewCollection GetaddressEntityViewCollection(String username_ad, String password_ad, int cust_id)
        //{

        //    Authentication_class var_auth = new Authentication_class();
        //    AuthenticationHeader ah = var_auth.getAuthHeader(username_ad, password_ad);
        //    AsmRepository.SetServiceLocationUrl(var_auth.var_service_location_url);

        //    var m_IViewFacadeService = AsmRepository.GetServiceProxyCachedOrDefault<IViewFacadeService>(ah);

        //    BaseQueryRequest viewRequest = new BaseQueryRequest();
        //    viewRequest.PageCriteria = new PageCriteria { Page = 0 };
        //    CriteriaCollection viewCriteria = new CriteriaCollection();
        //    viewCriteria.Add("Customerid", cust_id);
        //    viewRequest.FilterCriteria = viewCriteria;
        //    {
        //        AddressEntityViewCollection address = m_IViewFacadeService.GetAddressCollection(viewRequest);
        //        if (address != null && address.Items.Count > 0)
        //        {
        //            return address;
        //        }
        //        else
        //        {
        //            return null;
        //        }
        //    }


        //}
        //static IViewFacadeService ViewFacade
        //{
        //    get
        //    {
        //        if (m_IViewFacadeService == null)
        //        {
        //            //TO DO: Change the authentication header parameters
        //            Authentication_class var_auth = new Authentication_class();
        //            AuthenticationHeader authHeader = var_auth.getAuthHeader();

        //            //TO DO: Change the Service Location
        //            AsmRepository.SetServiceLocationUrl("http://mncsvasm.mskydev1.local/asm/all/servicelocation.svc");
        //            AsmRepository.AsmOperationTimeout = 360;
        //            m_IViewFacadeService = AsmRepository.GetServiceProxyCachedOrDefault<IViewFacadeService>(authHeader);
        //        }
        //        return m_IViewFacadeService;
        //    }
        //}
        //static AddressEntityViewCollection GetAddressCollection(BaseQueryRequest baseQueryRequest)
        //{
        //    AddressEntityViewCollection addressCollection = null;
        //    IViewFacadeService viewFacadeService = AddressController.ViewFacade;
        //    addressCollection = viewFacadeService.GetAddressEntityViewCollection(baseQueryRequest);
        //    return addressCollection;

        //}

        [HttpGet]
        [Route("api/{username_ad}/{password_ad}/address/GetaddressEntityViewCollectionByCustId/{cust_id}")]
        public HttpResponseMessage GetaddressEntityViewCollectionByCustId(String username_ad, String password_ad, int cust_id)
        {

            Authentication_class var_auth = new Authentication_class();
            AuthenticationHeader ah = var_auth.getAuthHeader(username_ad, password_ad);
            AsmRepository.SetServiceLocationUrl(var_auth.var_service_location_url);
            var TSService = AsmRepository.GetServiceProxyCachedOrDefault<IViewFacadeService>(ah);

            BaseQueryRequest request = new BaseQueryRequest();
            request.FilterCriteria = new CriteriaCollection();
            request.FilterCriteria.Add(new Criteria("CustomerId", cust_id));

            var servicetype = TSService.GetAddressEntityViewCollection(request);

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
