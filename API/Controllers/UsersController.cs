using API.Exceptions;
using API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserContext db;
        private readonly ILogger<UsersController> logger;

        public UsersController(UserContext db, ILogger<UsersController> logger)
        {
            this.db = db;
            this.logger = logger;
        }

        // GET: api/<UsersController>
        [HttpGet]
        public async Task<IEnumerable<User>> GetAsync()
        {
            return await db.Users.AsNoTracking().ToListAsync();
        }

        // GET api/<UsersController>/page?page=0&length=10
        [HttpGet("page")]
        public async Task<IEnumerable<User>> GetAsync([FromQuery]int page, [FromQuery]int? length)
        {
            length = length ?? 20; 

            if (page < 0 || length < 1) throw new BadRequestException("not valid length or page number");

            var data = await db.Users.AsNoTracking().Skip(page*length.Value).Take(length.Value).ToListAsync();

            if (data == null) throw new NotFoundException("no data exists");

            return data;
        }

        // GET api/<UsersController>/search?Name=text&Email=text&Address=text&Page=text&Length
        [HttpGet("search")]
        public async Task<IEnumerable<User>> GetAsync([FromQuery]UserSearchModel search)
        {
            IQueryable<User> query = db.Users.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(search.Name))
                query = query.Where(user => user.Name.Contains(search.Name));

            if (!string.IsNullOrWhiteSpace(search.Address))
                query = query.Where(user => user.Address.Contains(search.Address));


            if (!string.IsNullOrWhiteSpace(search.Email))
                query = query.Where(user => user.Email.Contains(search.Email));

            // page and length exist
            if (search.Page.HasValue && search.Length.HasValue)
                query = query.Skip(search.Page.Value * search.Length.Value).Take(search.Length.Value);

            // length exists but page not exists.
            else if (!search.Page.HasValue && search.Length.HasValue)
                query = query.Take(search.Length.Value);


            var data = await query.ToListAsync();

            if (data == null) throw new NotFoundException("no data exists");

            return data;
        }



        // GET api/<UsersController>/5
        [HttpGet("{id}")]
        public User GetAsync(int id)
        {

            var data = db.Users.AsNoTracking().FirstOrDefault(user => user.Id == id);

            if (data == null) throw new NotFoundException("no user with that id exists");

            return data;
        }

        // POST api/<UsersController>
        [HttpPost]
        public async Task PostAsync([FromBody] User value)
        {
            if (!ModelState.IsValid)
                throw new ValidationExeption("Not Valid User Data");

            db.Users.Add(value);
            await db.SaveChangesAsync();

        }

        // PUT api/<UsersController>/5
        [HttpPut("{id}")]
        public async Task PutAsync(int id, [FromBody] User value)
        {
            if (id != value.Id) throw new BadRequestException("invalid data.");
            if (!ModelState.IsValid)
                throw new ValidationExeption("Not Valid User Data");

            db.Users.Update(value);
            await db.SaveChangesAsync();
        }

        // DELETE api/<UsersController>/5
        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {


            var user = db.Users.Find(id);
            if (user == null) throw new BadRequestException("invalid request, user not exist");

            db.Users.Remove(user);
            await db.SaveChangesAsync();

        }




        public  class UserSearchModel
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
                }.All(cond=> cond == true);
                    
            }

        }







    }
}
