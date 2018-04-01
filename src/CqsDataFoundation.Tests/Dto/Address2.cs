
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace CqsDataFoundation.Tests.Dto
{
    [DataContract(Namespace = "http://www.com/")]
    class Address2
    {
        [DataType(DataType.PostalCode, ErrorMessage = "Illegal Zip code")]
        [DataMember(Order = 1)]
        public int Zip { get; set; }

        [Required]
        [MaxLength(40, ErrorMessage = "City is longer than 40")]
        [DataMember(Order = 2)]
        public string City { get; set; }
        
        [Required]
        [MaxLength(60, ErrorMessage = "Street is longer than 60")]
        [DataMember(Order = 3)]
        public string Street { get; set; }
        
        [Required]        
        [DataMember(Order = 4)]
        [Range(1, int.MaxValue)]
        public int House { get; set; }

        [DataMember(Order = 5)]
        public string Building { get; set; }

        [Required]
        [DataMember(Order = 6)]
        [Range(1, int.MaxValue)]
        public int Flat { get; set; }
    }        
}
