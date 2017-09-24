using PayMedia.ApplicationServices.ClientProxy;
using PayMedia.ApplicationServices.Finance.ServiceContracts;
using PayMedia.ApplicationServices.Finance.ServiceContracts.DataContracts;
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
    public class LedgerAccountController : ApiController
    {

        [HttpGet]
        [Route("api/{username_ad}/{password_ad}/ledgeraccount/GetLedgerAccounts")]
        public HttpResponseMessage GetLedgerAccounts(String username_ad, String password_ad)
        {

            Authentication_class var_auth = new Authentication_class();
            AuthenticationHeader ah = var_auth.getAuthHeader(username_ad, password_ad);
            AsmRepository.SetServiceLocationUrl(var_auth.var_service_location_url);
            var financeConfigurationService = AsmRepository.GetServiceProxyCachedOrDefault<IFinanceConfigurationService>(ah);
            //Instantiate a request object and its FilterCriteria
            LedgerAccountQueryRequest request = new LedgerAccountQueryRequest();
            request.FilterCriteria = new CriteriaCollection();
            //Populate the FilterCriteria, using In operator to perform an "Or" query.
            //request.FilterCriteria.Add(new Criteria("LedgerAccountCode", "U2001", Operator.In));
            //request.FilterCriteria.Add(new Criteria("LedgerAccountCode", "RU2001", Operator.In));
            //request.FilterCriteria.Add(new Criteria("LedgerAccountCode", "P9500", Operator.In));
            //request.FilterCriteria.Add(new Criteria("LedgerAccountCode", "RP9500", Operator.In));
            request.FilterCriteria.Add(new Criteria("IconId", "", Operator.Like));
            //Call the method.
            LedgerAccountCollection collection = financeConfigurationService.GetLedgerAccounts(request);
            //Display the results.

            if (collection != null && collection.Items.Count > 0)
            {
                return Request.CreateResponse(HttpStatusCode.OK, collection);
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
