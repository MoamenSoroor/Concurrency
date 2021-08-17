using ConcurrencyWithWPF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConcurrencyWithWPF.Services
{

    public interface IUserService
    {
        public Task<IList<User>> AllUsersAsync();
        public Task<IList<User>> GetPageAsync(int page, int length);
        public Task<IList<User>> SearchAsync(UserSearchModel search);
        public Task<IList<User>> SearchByCountry(string country);
        


    }

    public class UserService : IUserService
    {
        private readonly IClient client;

        public UserService(IClient client)
        {
            this.client = client;
        }

        public async Task<IList<User>> AllUsersAsync()
        {
            var result = await client.UsersAllAsync();
            return result.ToList();
        }

        public async Task<IList<User>> GetPageAsync(int page, int length)
        {
            var result = await client.GetPageAsync(page,length);
            return result.ToList();
        }

        public async Task<IList<User>> SearchAsync(UserSearchModel search)
        {
            var result = await client.SearchAsync(search.Name, search.Email, search.Address, search.Country, search.Page, search.Length);
            return result.ToList();
        }

        public Task<IList<User>> SearchByCountry(string country)
        {
            return SearchAsync(new UserSearchModel() { Country = country });
        }
    }
}
