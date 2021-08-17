using System.Linq;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Models
{
    public class UserSearchModel
    {

        public string Name { get; set; }

        public string Email { get; set; }

        public string Address { get; set; }

        public string Country { get; set; }


        public int? Page { get; set; }

        public int? Length { get; set; }


        public bool IsEmptySearch()
        {
            return new[] {
                    string.IsNullOrWhiteSpace(Name),
                    string.IsNullOrWhiteSpace(Email),
                    string.IsNullOrWhiteSpace(Address),
                    string.IsNullOrWhiteSpace(Country)
                }.All(cond => cond == true);

        }

    }
}
