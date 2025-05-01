using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace INDIACom.Models
{
     public class PaperDetailsModel
    {
        public long PaperId { get; set; }
        public string Title { get; set; }
        public DateTime DateOfSubmission { get; set; }
        public string EventName { get; set; }
        public string TrackName { get; set; }
        public string SessionName { get; set; }
        public int? CorrespondanceId { get; set; }
        public string Status { get; set; } // This will be based on review_result
        public string PlagiarismPath { get; set; } // For Turnitin download column
    }


}
