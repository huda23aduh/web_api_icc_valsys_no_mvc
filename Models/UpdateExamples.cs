using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace web_api_icc_valsys_no_mvc.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class UpdateExamples
    {
        [Required]
        [MaxLength(140)]
        public string Status { get; set; }

        public DateTime Date { get; set; }
    }
}