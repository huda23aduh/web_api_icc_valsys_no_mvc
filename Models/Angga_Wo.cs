using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace web_api_icc_valsys_no_mvc.Models
{
    public class Angga_Wo
    {
        public string username_ad { get; set; }
        public string password_ad { get; set; }

        public int wo_id_param { get; set; }
        public string wo_action_name_param { get; set; }

        public string work_desc_param { get; set; }
        public string action_taken_param { get; set; }
        public int reason_id { get; set; }
    }
}