using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using NorthWind.Model.DTO;


namespace NorthWind.Model.Commands
{
    [DataContract(Namespace = "http://www.ru/NorthWind")]
    public class AddCategoryWithProductsCommand
    {
        public AddCategoryWithProductsCommand(string catName, string description)
        {
            CategoryName = catName;
            Description = description;
        }

        [DataMember(Order = 1)]
        public int CategoryID { get; set; }

        [MaxLength(15, ErrorMessage = "Category name should not exceed 15 characters")]
        [Required(ErrorMessage = "Fill in category name")]
        [DataMember(Order = 2)]
        public string CategoryName { get; set; }

        [Required(ErrorMessage = "Fill in category description")]
        [DataMember(Order = 3)]
        public string Description { get; set; }

        [DataMember(Order = 4)]
        public IList<Product> Products { get; set; }
    }    
}
