namespace INDIACom.Models
{
    public class UserInfoModel
    {
        public long MemberId { get; set; }
        public string Salutation { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Category { get; set; }
        public int UserTypeId { get; set; }
        public string Address { get; set; }
        public string Country { get; set; }
        public string CountryID { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string Pincode { get; set; }
        public string Mobile { get; set; }
        public string Event { get; set; }
        public string CSINo { get; set; }
        public string IEEENo { get; set; }
        public string Organisation { get; set; }
        public string BioDataPath { get; set; }
    }
}