using System.ComponentModel.DataAnnotations;

namespace Admin3.Models
{
    public class OrderModel
    {
        public int? OrderID { get; set; }

        [Required(ErrorMessage = "please select OrderDate")]
        public DateTime OrderDate { get; set; }

        [Required(ErrorMessage = "please select customer Name")]
        public int CustomerID { get; set; }

        [Required(ErrorMessage = "please Enter payment mode")]
        public string? PaymentMode { get; set; }

        [Required(ErrorMessage = "please Enter total amount")]
        public decimal? TotalAmount { get; set; }

        [Required(ErrorMessage = "please Enter ShippingAddress")]
        public string ShippingAddress { get; set; }

        [Required(ErrorMessage = "please select username")]
        public int UserID { get; set; }
    }

    public class order_dropdown
    {
        public int? OrderID { get; set; }
        public string ShippingAddress { get; set; }
    }
}
