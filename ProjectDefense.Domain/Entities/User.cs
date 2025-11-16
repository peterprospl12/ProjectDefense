using Microsoft.AspNetCore.Identity;
using ProjectDefense.Domain.Enums;

namespace ProjectDefense.Domain.Entities
{
    public class User : IdentityUser
    {
        public Role Role { get; set; }
    }
}
