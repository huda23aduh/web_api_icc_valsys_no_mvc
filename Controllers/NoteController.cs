using PayMedia.ApplicationServices.ClientProxy;
using PayMedia.ApplicationServices.Customers.ServiceContracts;
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
    public class NoteController : ApiController
    {
        
        [HttpPost]
        [Route("api/note/CreateNote")]
        public HttpResponseMessage CreateNote([FromBody]CreateNoteParams created_data)
        {
            Authentication_class var_auth = new Authentication_class();
            AuthenticationHeader ah = var_auth.getAuthHeader(created_data.username_ad, created_data.password_ad);
            AsmRepository.SetServiceLocationUrl(var_auth.var_service_location_url);
            var customersConfiguration = AsmRepository.GetServiceProxyCachedOrDefault<ICustomersService>(ah);
            //Instanstiate and initialize a ValidAddress object.
            //For a description of each property, see the API Reference Library CHM.
            Note new_note = new Note();
            new_note.CategoryId = created_data.the_categoryId; //1
            //new_note.CategoryKey = "ADMINPOS";
            new_note.CompletionStageId = created_data.the_completionStageId; //1
            //new_note.CompletionStageKey = NoteCompletionStage.ENTITY_ID.ToString();

            new_note.Body = created_data.the_body_note;

            new_note.CustomerId = created_data.the_customerId;


            Note the_new_note = customersConfiguration.CreateNote(new_note, 0);

            if (the_new_note != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, the_new_note);
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
