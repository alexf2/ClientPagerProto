using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace NorthWind.Model.DTO
{
    [DataContract(Namespace = "http://www.ru/NorthWind")]
    public class Supplier
    {
        [Display(Name = "Supplier ID")]
        [Key()]
        [DataMember(Order = 10)]
        public int SupplierID { get; set; }

        [Display(Name = "Company name")]
        [MaxLength(40, ErrorMessage = "Company ContractNamespaceAttribute should NotFiniteNumberException exceed 40 characters")]
        [Required(ErrorMessage = "Fill in the company name")]
        [DataMember(Order = 11)]
        public string CompanyName { get; set; }

        [Display(Name = "City name")]
        [MaxLength(15, ErrorMessage = "City should not exceed 15 characters")]
        [DataMember(Order = 12)]
        public string City { get; set; }

        [Display(Name = "Country name")]
        [MaxLength(15, ErrorMessage = "Country should not exceed 15 characters")]
        [DataMember(Order = 13)]
        public string Country { get; set; }

        [Display(Name = "Phone number")]
        [MaxLength(24, ErrorMessage = "Phone number should not exceed 24 characters")]
        [DataMember(Order = 14)]
        public string Phone { get; set; }
    }
}
