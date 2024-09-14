using System.ComponentModel.DataAnnotations;

namespace Admin3.Models
{
    public class ProductModel
    {
        public int? ProductID { get; set; }
        [Required(ErrorMessage ="please Enter ProductName")]
        public string? ProductName { get; set; }
        [Required(ErrorMessage = "please Enter ProductPrice")]
        public decimal ProductPrice { get; set; }
        [Required(ErrorMessage = "please Enter ProductCode")]
        public string ProductCode { get; set; }
        [Required(ErrorMessage ="please Enter Description")]
        public string Description { get; set; }
        public int UserID { get; set; }
    }

    public class product_dropdown
    {
        public int? ProductID { get; set; }
        public string ProductName { get; set; }
    }
}
