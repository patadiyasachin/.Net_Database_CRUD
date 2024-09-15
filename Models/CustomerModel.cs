using System.ComponentModel.DataAnnotations;

namespace Admin3.Models
{
    public class CustomerModel
    {
        public int? CustomerID { get; set; }

        [Required(ErrorMessage = "please Enter Customer Name")]
        public string CustomerName { get; set; }

        [Required(ErrorMessage = "please Enter Home Address")]
        public string HomeAddress { get; set; }

        [Required(ErrorMessage = "please Enter Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "please Enter MobileNo")]
        public string MobileNo { get; set; }

        [Required(ErrorMessage = "please Enter GSR No")]
        public string GSTNO{ get; set; }

        [Required(ErrorMessage = "please Enter City Name")]
        public string CityName { get; set; }

        [Required(ErrorMessage = "please Enter Pin code")]
        public string PinCode { get; set; }

        [Required(ErrorMessage = "please Enter Net Amount")]
        public decimal NetAmount{ get; set; }

        [Required(ErrorMessage = "please select UserName")]
        public int UserID { get; set; }
    }

    public class Customer_DropDown
    {
        public int CustomerID { get; set; }
        public string CustomerName { get; set; }
    }
}
