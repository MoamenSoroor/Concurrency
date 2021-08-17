using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }


        [Required]
        [DataType(dataType:DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        public string Address { get; set; }


        [Required]
        public string Country { get; set; }
    }
}
