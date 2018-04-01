using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using NorthWind.Model.DTO;
using CqsDataFoundation.Query;

namespace NorthWind.Model.Queries
{
    [DataContract(Namespace = "http://www.ru/NorthWind")]
    public class CustomersPageSearchQuery : QueryPaggedBase<Customer, DataPage<Customer>>
    {
        public CustomersPageSearchQuery(int pageNumber, int size = Constants.PageSizeDefault)
            : base(size, pageNumber)
        {            
        }

        private string _companyName;
        [MaxLength(40, ErrorMessage = "Company name should not exceed 40 characters")]
        [DataMember(Order = 10)]
        public string CompanyName
        {
            get { return _companyName;  }
            set
            {
                _companyName = value;
                AddFiltering(c => c.CompanyName, value, CriterionPredicate.Contains);
            }
        }

        private string _city;
        [MaxLength(15, ErrorMessage = "City name should not exceed 15 characters")]
        [DataMember(Order = 11)]
        public string City
        {
            get { return _city; }
            set
            {
                _city = value;
                AddFiltering(c => c.City, value, CriterionPredicate.Eq);
            }
        }

        private string _country;
        [MaxLength(15, ErrorMessage = "Country name should not exceed 15 characters")]
        [DataMember(Order = 12)]
        public string Country
        {
            get { return _country; }
            set
            {
                _country = value;
                AddFiltering(c => c.Country, value, CriterionPredicate.Eq);
            }
        }
    }
}
