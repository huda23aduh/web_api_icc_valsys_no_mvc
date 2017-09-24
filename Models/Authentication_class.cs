using PayMedia.ApplicationServices.SharedContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace web_api_icc_valsys_no_mvc.Models
{
    public class Authentication_class
    {
        public AuthenticationHeader getAuthHeader(String username_param, String password_param)
        {
            //Create the authentication header, initialize with credentials, and
            //create the service proxy.
            //To avoid passing in a clear password, use Token Authentication.
            //For details, see the API Developer's Guide.
            //Pass in a value for ExternalAgent to track the external user who
            //accessed the API. For details, see the API Reference Library CHM.
            AuthenticationHeader ah = new AuthenticationHeader
            {
                UserName = username_param,
                Proof = password_param,
                Dsn = "MSKY-TRA"
            };

            return ah;
        }
        //COMMAND http://mncsvasm.mskydev1.local/asm/all/servicelocation.svc http://192.168.176.112/asm/all/servicelocation.svc

       // public string var_service_location_url = "http://192.168.177.4/asm/all/servicelocation.svc";
        public string var_service_location_url = "http://mncsvasm.mskydev1.local/asm/all/servicelocation.svc";

    }
}