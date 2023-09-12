using Microsoft.AspNetCore.Identity;

namespace MHealth.Models.Domain
{
    public class User : IdentityUser
    {
        public string Name { get; set; }
    }
}
