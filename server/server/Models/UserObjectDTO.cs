using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace server.Models
{
    public class UserObjectDTO
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Role { get; set; }
    }
}
