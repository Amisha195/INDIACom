using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace INDIACom.Models
{
    public class SpecialSessionModel
    {
        public string SSName { get; set; }
        public long TrackID { get; set; }  // Stores Track ID
        public long MemberID { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string Organization { get; set; }
        public string Topic { get; set; }
        public long ReqNo { get; set; }
        public DateTime Request_Date { get; set; }
        public DateTime? Decision_Date { get; set; }
        public string Request_Status { get; set; }
        public int PaperCount { get; set; }
    }
}