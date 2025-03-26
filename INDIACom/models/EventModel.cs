using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace INDIACom.Models
{
    public class EventModel
    {
        public string Event_Id { get; set; }
        public string Event_Name { get; set; }
        public string Event_Description { get; set; }
        public DateTime Event_Creation_date { get; set; }
        public DateTime Event_Opening_date { get; set; }
        public DateTime Event_Closing_date { get; set; }
        public string Event_Type { get; set; }
    }
}