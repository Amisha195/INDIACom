using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace INDIACom.Models
{
    public class Member
    {
        [Key]
        public string MemberID { get; set; }

        public string Salutation { get; set; }

        [Required]
        public string Name { get; set; }

        public string Address { get; set; }

        public string Country { get; set; }
        public int? CountryID { get; set; }

        public string State { get; set; }
        public string City { get; set; }
        public string Pincode { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        public string Mobile { get; set; }
        public string Event { get; set; }
        public string CSI_No { get; set; }
        public string IEEE_No { get; set; }

        public string Organisation { get; set; }
        public string Category { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }

        public string BioDataPath { get; set; }
    }
}