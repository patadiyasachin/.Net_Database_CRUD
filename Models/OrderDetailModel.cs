using System;
using System.ComponentModel.DataAnnotations;

namespace Admin3.Models
{
    public class OrderDetailModel
    {
        public int? OrderDetailID { get; set; }

        [Required(ErrorMessage = "please select shipping address")]
        public int OrderID { get; set; }

        [Required(ErrorMessage = "please select product name")]
        public int ProductID{ get; set; }

        [Required(ErrorMessage = "please Enter quantity")]
        public int Quantity{ get; set; }

        [Required(ErrorMessage = "please Enter amount")]
        public decimal Amount{ get; set; }

        [Required(ErrorMessage = "please Enter total amount")]
        public decimal TotalAmount{ get; set; }

        [Required(ErrorMessage = "please select username")]
        public int UserID{ get; set; }
    }
}
