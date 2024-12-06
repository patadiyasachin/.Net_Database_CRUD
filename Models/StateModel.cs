using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Admin3.Models
{
    public class StateModel
    {
        public int? StateID { get; set; }
        [Required]
        [DisplayName("City Name")]
        public string StateName { get; set; }
        [Required]
        [DisplayName("Country Name")]
        public int CountryID { get; set; }
        [Required]
        [DisplayName("City Code")]
        public string StateCode { get; set; }
    }
}
