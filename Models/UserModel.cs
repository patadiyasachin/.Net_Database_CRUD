namespace Admin3.Models
{
    public class UserModel
    {
        public int? UserID{ get; set; }
        public string UserName{ get; set; }
        public string Email{ get; set; }
        public string Password{ get; set; }
        public string MobileNumber{ get; set; }
        public string Address{ get; set; }
        public bool IsActive{ get; set; }
    }

    public class User_dropDown
    {
        public int UserID { get; set; }
        public string UserName { get; set; }
    }
}
