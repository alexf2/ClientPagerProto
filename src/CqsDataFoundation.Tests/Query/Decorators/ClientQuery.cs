using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using CqsDataFoundation.Tests.Dto;
using CqsDataFoundation.Query;
using CqsDataFoundation.Validation;

namespace CqsDataFoundation.Tests.Query.Decorators
{
    [DataContract(Namespace = "http://www.com/")]
    class ClientQuery : QueryBase<Client>
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
        public ExtParams ExtParams { get; set; }

        public override string CacheKey
        {
            get { return string.Format("{0}_{1}", GivenName, SurName); }
        }
    }

    [DataContract(Namespace = "http://www.com/")]
    sealed class ExtParams
    {
        [Required]
        [DataMember(Order = 1)]
        [MaxLength(40)]
        public string RefererName { get; set; }


        [MaxLength(40, ErrorMessage = "City is longer than 40")]
        [DataMember(Order = 2)]
        public string RefererCity { get; set; }
    }
}
