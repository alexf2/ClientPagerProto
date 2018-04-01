using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using DataAnnotationsExtensions;

namespace NorthWind.Model.Commands
{
    [DataContract(Namespace = "http://www.ru/NorthWind")]
    public class AddProductCommand
    {
        public AddProductCommand(string name, bool discounted)
        {            
            ProductName = name;
            Discontinued = discounted;
        }

        [DataMember(Order = 10)]
        public int ProductID { get; set; }
        
        [MaxLength(15, ErrorMessage = "Product name should not exceed 15 characters")]
        [Required(ErrorMessage = "Fill in product name")]
        [DataMember(Order = 11)]
        public string ProductName { get; set; }
        
        [Required(ErrorMessage = "Fill in Discontinued")]
        [DataMember(Order = 12)]
        public bool Discontinued { get; set; }

        [DataMember(Order = 13)]
        public int? CategoryID { get; set; }
        [DataMember(Order = 14)]
        public int? SupplierID { get; set; }
                
        [Numeric(ErrorMessage = "UnitPrice should be a number. For instance: 50 or 25.07")]
        [Min(0, ErrorMessage = "UnitPrice should not be negative")]
        [DataMember(Order = 15)]
        public decimal? UnitPrice { get; set; }
        
        [Integer]
        [Min(0, ErrorMessage = "UnitsInStock should be positive")]
        [DataMember(Order = 16)]
        public short? UnitsInStock { get; set; }
    }
}
