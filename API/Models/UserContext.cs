using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    public class UserContext : DbContext
    {
        private readonly IHostEnvironment env;

        public UserContext(DbContextOptions options, IHostEnvironment env) : base(options)
        {
            //this.Database.EnsureDeleted();
            //this.Database.EnsureCreated();
            this.env = env;
        }

        public virtual DbSet<User> Users { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var data = LoadSeedData();
            modelBuilder.Entity<User>().HasData(data);
            base.OnModelCreating(modelBuilder);
        }

        public List<User> LoadSeedData()
        {
            using StreamReader reader = File.OpenText(
                Path.Combine(env.ContentRootPath,"Data", "UsersMockData2.json"));
            string data = reader.ReadToEnd();

            return JsonConvert.DeserializeObject<List<User>>(data);
        }
    }
}
