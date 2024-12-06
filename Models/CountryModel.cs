using System.ComponentModel.DataAnnotations;

namespace Admin3.Models
{
    public class CountryModel
    {
        public int? CountryID { get; set; }

        [Required(ErrorMessage = " Country Name Is Not Enter")]
        public string CountryName { get; set; }

        [Required(ErrorMessage = "Country Code is Not Enter")]
        public string CountryCode { get; set; }
    }
}
