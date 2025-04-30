namespace INDIACom.Models
{
    public class LoginUserModel
    {
        public long? UserID { get; set; }

        public string Email { get; set; }

        public string Name { get; set; }

        public string Password { get; set; }

        public MemberModel LoggedInUser { get; set; }



    }
}