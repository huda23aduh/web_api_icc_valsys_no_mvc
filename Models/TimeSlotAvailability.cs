using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using PayMedia.ApplicationServices.Workforce.ServiceContracts;
using PayMedia.ApplicationServices.Workforce.ServiceContracts.DataContracts;

namespace web_api_icc_valsys_no_mvc.Models
{
    public class TimeSlotAvailability
    {
        //public int The_service_provider_id { get; set; }
        //public String The_description { get; set; }
        //public int The_service_type_id { get; set; }
        //public int The_geo_group_id { get; set; }
        public List<TimeSlotAvailabilityItem> TimeItemsData { get; set; }
    }
}