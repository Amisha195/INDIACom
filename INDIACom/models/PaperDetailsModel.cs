using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace INDIACom.Models
{
    public class PaperDetailsModel
    {
        public int PaperID { get; set; }
        public string Title { get; set; }
        public string EventName { get; set; }
        public string TrackName { get; set; }
        public string SessionName { get; set; }
        public string ChairpersonTitle { get; set; }
        public string ReviewStatus { get; set; }
    }

}