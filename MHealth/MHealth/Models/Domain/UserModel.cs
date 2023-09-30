using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MHealth.Models.Domain
{
    public class UserModel : IdentityUser
    {
        [Required]
        public string? Name { get; set; }
    }
}
