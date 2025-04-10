using System.ComponentModel.DataAnnotations;

namespace INDIACom.Models
{
    public class MembersModel
    {
        public long MemberID { get; set; }  // Primary Key (Auto-increment)
        [Required]
        public string Salutation { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string Country { get; set; }
        [Required]
        public string CountryID { get; set; }
        [Required]
        public string State { get; set; }
        public string StateID { get; set; }
        [Required]
        public string City { get; set; }
        public string CityID { get; set; }
        [Required]
        public string Pincode { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public long Mobile { get; set; }
        public string EventID { get; set; }
        [Required]
        public string Event { get; set; }
        public string CSI_No { get; set; }
        public string IEEE_No { get; set; }
        public string OrgID { get; set; }
        [Required]
        public string OrganisationName { get; set; }
        public string Biodata { get; set; }
        [Required]
        public string Category { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string ConfirmPassword { get; set; }


        public string OrgEmail { get; set; }
        public string OrgName { get; set; }
        public string OrgShortName { get; set; }
        public string OrgAddress { get; set; }
        public string OrgContactPerson { get; set; }
        public string OrgPhone { get; set;}

        public int UserTypeId { get; set; }






    }



}