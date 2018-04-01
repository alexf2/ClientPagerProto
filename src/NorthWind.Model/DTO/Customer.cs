using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace NorthWind.Model.DTO
{
    [DataContract(Namespace = "http://www.ru/NorthWind")]
    public class Customer
    {
        [Display(Name = "Client code")]
        [Key()]
        [MaxLength(5, ErrorMessage = "Client code should be 5 characters in size")]
        [MinLength(5, ErrorMessage = "Client code should be 5 characters in size")]
        [DataMember(Order = 10)]
        public string CustomerId { get; set; }

        [Display(Name = "Company name")]
        [MaxLength(40, ErrorMessage = "Company name shoud not exceed 40 characters")]
        [Required(ErrorMessage = "Fill in company name")]
        [DataMember(Order = 11)]
        public string CompanyName { get; set; }

        [Display(Name = "Company address")]
        [MaxLength(60, ErrorMessage = "Company address should not exceed 60 characters")]
        [DataMember(Order = 12)]
        public string Address { get; set; }

        [Display(Name = "City name")]
        [MaxLength(15, ErrorMessage = "City name should not exceed 15 characters")]
        [DataMember(Order = 13)]
        public string City { get; set; }

        [Display(Name = "Country name")]
        [MaxLength(15, ErrorMessage = "Country name should not exceed 15 characters")]
        [DataMember(Order = 14)]
        public string Country { get; set; }

        [Display(Name = "Postal index")]
        [DataType(DataType.PostalCode, ErrorMessage = "Error postal index")]
        [DataMember(Order = 15)]
        public string PostalCode { get; set; }

        [Display(Name = "Customer Name")]
        [MaxLength(30, ErrorMessage = "Customer name should not exceed 30 characters")]
        [DataMember(Order = 16)]
        public string ContactName { get; set; }
    }
}
