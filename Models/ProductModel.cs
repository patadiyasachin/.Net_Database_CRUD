using System.ComponentModel.DataAnnotations;

namespace Admin3.Models
{
    public class ProductModel
    {
        public int? ProductID { get; set; }
        [Required]
        public string ProductName { get; set; }
        [Required]
        public decimal ProductPrice { get; set; }
        [Required]
        public string ProductCode { get; set; }
        [Required]
        public string Description { get; set; }
        public int UserID { get; set; }
    }

    public class product_dropdown
    {
        public int? ProductID { get; set; }
        public string ProductName { get; set; }
    }
}
