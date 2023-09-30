using Microsoft.AspNetCore.Authentication;
using System.ComponentModel.DataAnnotations;

namespace MHealth.Models.DTO
{
    public class LoginModel
    {
        [Required]
        public string? Username { get; set; }
        [Required]
        public string? Password { get; set; }
        public string? ReturnUrl { get; set; }

        public IList<AuthenticationScheme>? ExternalLogins { get; set; }
    }
}
