using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using CqsDataFoundation.Tests.Dto;
using CqsDataFoundation.Validation;

namespace CqsDataFoundation.Tests.Command.Decorators
{
    [DataContract(Namespace = "http://www.com/")]
    class AddClientCommand
    {
        [Required]
        [DataMember(Order = 1)]
        [MaxLength(40)]
        public string GivenName { get; set; }

        [DataMember(Order = 2)]
        [MaxLength(40)]
        public string MiddleName { get; set; }

        [Required]
        [DataMember(Order = 3)]
        [MaxLength(40)]
        public string SurName { get; set; }

        [DataMember(Order = 4)]
        [ValidateObject]
        public Address2 Address { get; set; }
    }
}
