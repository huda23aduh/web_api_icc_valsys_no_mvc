using PayMedia.ApplicationServices.ClientProxy;
using PayMedia.ApplicationServices.SharedContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using web_api_icc_valsys_no_mvc.Models;

using PayMedia.ApplicationServices.DocumentManagement.ServiceContracts;
using System.Xml.Linq;
using PayMedia.ApplicationServices.DocumentManagement.ServiceContracts.DataContracts;

namespace web_api_icc_valsys_no_mvc.Controllers
{
    public class DocumentController : ApiController
    {
        [HttpPost]
        [ActionName("linkdocument")]
        [Route("api/{username_ad}/{password_ad}/document/linkdocument")]
        public string linkdocument([FromBody]DocumentRequest req)
        {
            IDocumentManagementService docmService = null;

            Authentication_class var_auth = new Authentication_class();
            AuthenticationHeader authHeader = var_auth.getAuthHeader(req.username_ad, req.password_ad);
            AsmRepository.SetServiceLocationUrl(var_auth.var_service_location_url);

            docmService = AsmRepository.GetServiceProxy<IDocumentManagementService>(authHeader);

            var var_uri = new System.Uri(req.file_uri);

            
                Document doc = new Document
                {
                    Name = req.file_name,
                    Uri = req.file_uri, // File Path , must be correct. And it must be ICC configured file server
                    CreatedDate = DateTime.Now,
                    CreatedByUserId = 45, // ICC User ID
                    HostId = 23,      // Fixed Value 23
                    FileSize = req.file_size  // must be the exact file size in Bytes
                };
                var newDoc = docmService.CreateDocument(doc);

                var docLink = new DocumentLink
                {
                    DocumentId = newDoc.Id,
                    CustomerId = req.cust_id,
                    //DocumentType = 1
                };
                var linkReason = 0;
                DocumentLink newDocLink = docmService.CreateDocumentLink(docLink, linkReason);

                return req.file_uri;
            



        }
    }
}
