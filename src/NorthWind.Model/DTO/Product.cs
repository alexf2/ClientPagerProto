using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using DataAnnotationsExtensions;
using CqsDataFoundation.Validation;

namespace NorthWind.Model.DTO
{
    [DataContract(Namespace = "http://www.ru/NorthWind")]
    public class Product
    {
        [Display(Name = "Category ID")]
        [Key()]
        [DataMember(Order = 10)]
        public int ProductID { get; set; }

        [Display(Name = "Category name")]
        [MaxLength(15, ErrorMessage = "Category name should not exceed 15 characters")]
        [Required(ErrorMessage = "Fill in category name")]
        [DataMember(Order = 11)]
        public string ProductName { get; set; }

        [Display(Name = "Category description")]
        [DataMember(Order = 12)]
        public string Description { get; set; }

        [Display(Name = "Product category")]
        [ValidateObject]
        [DataMember(Order = 13)]
        public Category Category { get; set; }

        [Display(Name = "Product supplier")]
        [ValidateObject]
        [DataMember(Order = 14)]
        public Supplier Supplier { get; set; }

        [Display(Name = "Price")]        
        [Numeric(ErrorMessage = "UnitPrice should be a number. For instance: 50 or 25.07")]
        [Min(0, ErrorMessage = "UnitPrice should not be negative")]
        [DataMember(Order = 15)]
        public decimal? UnitPrice { get; set; }

        [Display(Name = "The rest in stock")]
        //[Range(0, short.MaxValue, ErrorMessage = "UnitsInStock должно быть положительным")]
        [Integer]
        [Min(0, ErrorMessage = "UnitsInStock should be positive")]
        [DataMember(Order = 16)]
        public short? UnitsInStock { get; set; }

        [Display(Name = "Supplying has stopped")]
        [Required(ErrorMessage = "Fill in Discontinued")]
        [DataMember(Order = 17)]
        public bool Discontinued { get; set; }
    }
}
