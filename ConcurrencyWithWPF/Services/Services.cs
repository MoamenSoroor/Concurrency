using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConcurrencyWithWPF.Services
{
    public class Services
    {
        const string DataPath = "UsersMockData.json";

        public static List<User> Users;

        static Services()
        {
        }

        public static async Task<List<User>> LoadDataAsync()
        {
            using StreamReader reader = File.OpenText(DataPath);
            string data = await reader.ReadToEndAsync();

            return JsonConvert.DeserializeObject<List<User>>(data);
        }






    }

    public class User
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Address { get; set; }
    }
}
