using PayMedia.ApplicationServices.SharedContracts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace web_api_icc_valsys_no_mvc.Models
{
    public class Authentication_class
    {
        static string path = @"c:\API_ACCESSING_LOG\";

        static string full_path_file = null;
        static string file_log_name = null;
        
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

       public string var_service_location_url = "http://mncsvasm.mskydev1.local/asm/all/servicelocation.svc";

        public void write_log(StringBuilder sb_param)
        {
            #region checking file log
            if (!Directory.Exists(path))  // if it doesn't exist, create
                Directory.CreateDirectory(path);

            file_log_name = DateTime.Now.ToString("dd_M_yyyy").ToString();

            full_path_file = path + "API_LOG_FILE_" + file_log_name + ".txt";

            if (!File.Exists(full_path_file))
            {
                File.Create(full_path_file).Dispose();
                using (TextWriter tw = new StreamWriter(full_path_file))
                {
                    tw.WriteLine("The very first line!" + Environment.NewLine);
                    tw.Close();
                }
            }

            #endregion checking file log

            File.AppendAllText(full_path_file, sb_param.ToString());
            sb_param.Clear();

        }

    }
}