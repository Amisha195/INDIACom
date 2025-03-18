using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace INDIACom.Models
{
    public class FeedbackModel
    {
        public int FeedbackID { get; set; }  // Primary Key (Auto-increment)
        public string Name { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string Organization { get; set; }
        public string ConferenceInfo { get; set; }
        public string PaperSubmission { get; set; }
        public string Registration { get; set; }
        public string ConferenceOrg { get; set; }
        public string Proceedings { get; set; }
        public string Comments { get; set; }
    }



}