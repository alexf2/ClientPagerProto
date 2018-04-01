using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using DataAnnotationsExtensions;

namespace NorthWind.Model.DTO
{
    [DataContract(Namespace = "http://www.ru/NorthWind")]
    public class ProductInfo
    {
        [Display(Name = "Category ID")]
        [Key()]
        [DataMember(Order = 1)]
        public int ProductID { get; set; }

        [Display(Name = "Category name")]
        [MaxLength(15, ErrorMessage = "Category name should not exceed 15 characters")]        
        [DataMember(Order = 2)]
        public string ProductName { get; set; }

        [Display(Name = "Category Name")]
        [MaxLength(15, ErrorMessage = "Category name should not exceed 15 symbols")]        
        [DataMember(Order = 3)]
        public string CategoryName { get; set; }

        [Display(Name = "Company name")]
        [MaxLength(40, ErrorMessage = "Company name shoud not exceed 40 characters")]        
        [DataMember(Order = 4)]
        public string CompanyName { get; set; }

        [Display(Name = "Bought number")]
        [DataMember(Order = 5)]
        [Min(1, ErrorMessage = "Bought number should be 1 or greater")]
        public int BoughtNumber { get; set; }
    }    
}
