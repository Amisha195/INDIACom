using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace INDIACom.Models
{
    public class PaperSubmissionModel
    {
        public string Title { get; set; }
        public DateTime DateOfSubmission { get; set; }
        public int Event_Id { get; set; }
        public string Event_Name { get; set; }
        public int Track_Id { get; set; }
        public string Track_Name { get; set; }
        public int Session_Id { get; set; }
        public string Session_Name { get; set; }
        public int Member_Id { get; set; }
        public string PaperPath { get; set; }
        public string PlagiarismPath { get; set; }
        public int Correspondence_Id { get; set; }
        public string Co_Authors_Id { get; set; }
    }
}
