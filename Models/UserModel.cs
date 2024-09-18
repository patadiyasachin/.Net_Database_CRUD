using System.ComponentModel.DataAnnotations;

namespace Admin3.Models
{
    public class UserModel
    {
        public int? UserID{ get; set; }

        [Required(ErrorMessage = "please Enter UserName")]
        public string UserName{ get; set; }

        [Required(ErrorMessage = "please Enter Email")]
        public string Email{ get; set; }

        [Required(ErrorMessage = "please Enter Password")]
        public string Password{ get; set; }

        [Required(ErrorMessage = "please Enter Mobile No")]
        public string MobileNumber{ get; set; }

        [Required(ErrorMessage = "please Enter Address")]
        public string Address{ get; set; }

        [Required(ErrorMessage = "please fill this field")]
        public bool? IsActive{ get; set; }
    }

    public class User_dropDown
    {
        public int UserID { get; set; }
        public string UserName { get; set; }
    }


    public class UserRegisterModel
    {
        public int? UserID { get; set; }

        [Required(ErrorMessage = "please Enter UserName")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "please Enter Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "please Enter Password")]
        public string Password { get; set; }

        [Required(ErrorMessage = "please Enter Mobile No")]
        public string MobileNumber { get; set; }

        [Required(ErrorMessage = "please Enter Address")]
        public string Address { get; set; }
    }
}
