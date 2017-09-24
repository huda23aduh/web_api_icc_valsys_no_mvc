using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace web_api_icc_valsys_no_mvc.Models
{
    public class CreateNoteParams
    {
        public String username_ad { get; set; }
        public String password_ad { get; set; }

        public int the_customerId { get; set; }
        public int the_categoryId { get; set; }
        public int the_completionStageId { get; set; }
        public String the_body_note { get; set; }
    }
}