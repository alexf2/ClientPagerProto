
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using CqsDataFoundation.Validation;

namespace CqsDataFoundation.Tests.Dto {
    [DataContract(Namespace = "http://www.com/")]
    class Client
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

        [DataMember(Order = 5)]
        [ValidateObject]
        public Referer Referer { get; set; }
    }
}
