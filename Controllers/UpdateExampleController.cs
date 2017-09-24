using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace web_api_icc_valsys_no_mvc.Controllers
{
    using web_api_icc_valsys_no_mvc.Models;
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using System.Web;
    using System.Web.Http;
    public class UpdateExampleController : ApiController
    {

        static readonly Dictionary<Guid, UpdateExamples> updates = new Dictionary<Guid, UpdateExamples>();

        [HttpPost]
        [ActionName("UpdateExamplesTest")]
        public HttpResponseMessage PostComplex(UpdateExamples update)
        {
            if (ModelState.IsValid && update != null)
            {
                // Convert any HTML markup in the status text.
                update.Status = HttpUtility.HtmlEncode(update.Status);

                // Assign a new ID.
                var id = Guid.NewGuid();
                updates[id] = update;

                // Create a 201 response.
                var response = new HttpResponseMessage(HttpStatusCode.Created)
                {
                    Content = new StringContent(update.Status)
                };
                response.Headers.Location =
                    new Uri(Url.Link("DefaultApi", new { action = "status", id = id+"asasasas" }));
                return response;
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        [HttpGet]
        public UpdateExamples Status(Guid id)
        {
            UpdateExamples update;
            if (updates.TryGetValue(id, out update))
            {
                return update;
            }
            else
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
        }

    }
}
