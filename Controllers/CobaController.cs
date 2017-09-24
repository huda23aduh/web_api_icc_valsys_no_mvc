using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace web_api_icc_valsys_no_mvc.Controllers
{
    public class CobaController : ApiController
    {
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        /// <summary>
        /// Looks up some data by ID.
        /// </summary>
        /// <param name="id">The ID of the data.</param>
        public string Get(int id)
        {
            return "value";
        }

    }
}
