using PayMedia.ApplicationServices.Workforce.ServiceContracts.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace web_api_icc_valsys_no_mvc.Models
{
    public class TimeSlotAvailabilityItem
    {
        //public DateTime The_date { get; set; }
        //public int The_time_slot_id { get; set; }
        //public int The_service_provider_id { get; set; }
        //public Decimal The_remain_slot { get; set; }
        //public Boolean The_isAvailable { get; set; }

        public SPServiceWithGeoInfo the_SPServiceWithGeoInfo { get; set; }
        public TimeSlotDescription[] the_timeslot { get; set; }

    }
}