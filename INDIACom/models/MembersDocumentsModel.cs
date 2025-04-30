using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace INDIACom.Models
{
    public class MembersDocumentsModel
    {
        public int MemberID { get; set; }  // Primary Key (Auto-increment)
        public string EventID { get; set; } // Associated Event ID
        public string InstitutionCard { get; set; } // Institution Card Document
        public string ProfBodyIDCard { get; set; } // Professional Body ID Card
        public string Passport { get; set; } // Passport Document
        public bool MemberCatVerificationStatus { get; set; } // Verification Status for Member Category
        public bool ProfBodyVerificationStatus { get; set; } // Verification Status for Professional Body
        public bool PassportVerification { get; set; } // Passport Verification Status
        public DateTime RequestDate { get; set; } = DateTime.Now; // Date of Request Submission
        public DateTime? DecisionDate { get; set; } // Date of Decision (Nullable)
        public string Comment { get; set; } // Admin Comments
        public string InstitutionCardPath { get; set; }
        public string ProfBodyIDCardPath { get; set; }
        public string PassportPath { get; set; }
    }
}
