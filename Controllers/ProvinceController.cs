using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using PayMedia.ApplicationServices.ClientProxy;
using PayMedia.ApplicationServices.SharedContracts;

using System.Web.Script.Serialization;

using web_api_icc_valsys_no_mvc.Models;
using System.Collections;
using PayMedia.ApplicationServices.Customers.ServiceContracts.DataContracts;

namespace web_api_icc_valsys_no_mvc.Controllers
{
    
    public class ProvinceController : ApiController
    {
        
        [HttpGet]
        //[ActionName("GetAllProvinces")]
        [Route("api/{username_ad}/{password_ad}/province/GetAllProvinces")]
        public ProvinceCollection GetAllProvinces(String username_ad, String password_ad)
        {
            Authentication_class var_auth = new Authentication_class();
            AuthenticationHeader authHeader = var_auth.getAuthHeader(username_ad, password_ad);
            AsmRepository.SetServiceLocationUrl(var_auth.var_service_location_url);

            var custService = AsmRepository.AllServices.GetCustomersConfigurationService(authHeader);
            ProvinceCollection provinces = custService.GetProvinceByCountry(1);
            
            //IEnumerable<string> enumerable = (IEnumerable<string>)provinces;

            List<string> list = new List<string>();
            //Provinces ok1 = new Provinces();

            // Loop through List with foreach.
            foreach (var prime in provinces)
            {
                var a = prime.Id.ToString();
                list.Add(a);
                var b = prime.Name.ToString();
                list.Add(b);
            }
            
            //var provinces_listString = provinces.OrderBy(x => x.Id).Select(x => new Provinces { Id_nya = (int) x.Id, Name_nya = (string) x.Name }).ToList();
            //var result = ((IEnumerable<string>)provinces).Cast<object>().ToList();
            return provinces;
        }

        [HttpGet]
        [Route("api/{username_ad}/{password_ad}/province/GetProvinceByCountryId/{id}")]
        public ProvinceCollection GetProvinceByCountryId(String username_ad, String password_ad, int id)
        {
            Authentication_class var_auth = new Authentication_class();
            AuthenticationHeader authHeader = var_auth.getAuthHeader(username_ad, password_ad);
            AsmRepository.SetServiceLocationUrl(var_auth.var_service_location_url);

            var custService = AsmRepository.AllServices.GetCustomersConfigurationService(authHeader);
            ProvinceCollection provinces = custService.GetProvinceByCountry(id); //parameter id
           
            return provinces;
        }

        // POST api/values
        public void Post([FromBody]string value)
        {

        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        
        // DELETE api/values/5
        public void Delete(int id)
        {
        }



    }
}
