using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace NorthWind.Model.DTO
{
    [DataContract(Namespace = "http://www.ru/NorthWind")]
    public class Category
    {
        [Display(Name = "Category ID")]
        [Key()]
        [DataMember(Order = 10)]
        public int CategoryID { get; set; }

        [Display(Name = "Category Name")]
        [MaxLength(15, ErrorMessage = "Category name should not exceed 15 symbols")]
        [Required(ErrorMessage = "Fill in category name")]
        [DataMember(Order = 11)]
        public string CategoryName { get; set; }

        [Display(Name = "Category description")]
        [DataMember(Order = 12)]
        public string Description { get; set; }
    }
}
