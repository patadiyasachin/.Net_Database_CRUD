using System.ComponentModel.DataAnnotations;

namespace Admin3.Models
{
    public class BillsModel
    {
        public int? BillID { get; set; }

        [Required(ErrorMessage = "please Enter Bill Number")]
        public string BillNumber { get; set; }

        [Required(ErrorMessage = "please select bill date")]
        public DateTime BillDate { get; set; }

        [Required(ErrorMessage = "please Enter Order Id")]
        public int OrderID{ get; set; }

        [Required(ErrorMessage = "please Enter Total Amount")]
        public decimal TotalAmount { get; set; }

        [Required(ErrorMessage = "please Enter Discount")]
        public decimal? Discount { get; set; }

        [Required(ErrorMessage = "please Enter netamount")]
        public decimal NetAmount { get; set; }

        [Required(ErrorMessage = "please select user")]
        public int UserID { get; set; }
    }
}
