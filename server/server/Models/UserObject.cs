using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace server.Models
{
    public class UserObject
    {
        [Key]
        [Required]
        [Column(TypeName ="nvarchar(100)")]
        public string Email { get; set; }
        [Required]
        [Column(TypeName = "nvarchar(100)")]
        public string Name { get; set; }
        [Required]
        [Column(TypeName = "nvarchar(50)")]
        public string password { get; set; }
    }
}
