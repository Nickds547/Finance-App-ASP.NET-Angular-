using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace server.Models
{
    public class UserObject
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Column(TypeName ="nvarchar(100)")]
        public string Email { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        public string Name { get; set; }
        [Required]
        [Column(TypeName = "nvarchar(255)")]
        public string Password { get; set; }
        [Required]
        [Column(TypeName = "nvarchar(50)")]
        public string Role { get; set; }
    }

}
