namespace INDIACom.Models
{
    public class MemberModel
    {
        public long MemberID { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string UserPassword { get; set; }
        public string url { get; set; }
        public string UserType { get; set; }
        public short UserTypeId { get; set; }
        public string Category { get; set; }
        public string Event { get; set; }
        public string Bio_data_path { get; set; }
        public string CSI_No { get; set; }
        public string IEEE_No { get; set; }
        public string Mobile { get; set; }
        public string Salutation { get; set; }
        public string Address { get; set; }
        public string Country { get; set; }
        public string CountryID { get; set; }
        public string State { get; set; }
        public string StateID { get; set; }
        public string City { get; set; }
        public string CityID { get; set; }
        public string Pincode { get; set; }
        public string EventID { get; set; }
        public string OrgID { get; set; }
        public string OrganisationName { get; set; }
        public string Biodata { get; set; }
        public string ConfirmPassword { get; set; }

        public string OrgEmail { get; set; }
        public string OrgName { get; set; }
        public string OrgShortName { get; set; }
        public string OrgAddress { get; set; }
        public string OrgContactPerson { get; set; }
        public string OrgPhone { get; set; }


    }
}