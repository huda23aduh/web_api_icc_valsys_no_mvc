using PayMedia.ApplicationServices.Workforce.ServiceContracts.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace web_api_icc_valsys_no_mvc.Models
{
    public class ServProvTimeSlot_response
    {
        public ServiceProviderService the_service_provider { get; set; }
        public TimeSlotDescription[] the_timeslot { get; set; }
    }
}