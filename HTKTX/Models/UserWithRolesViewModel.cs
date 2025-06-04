using Microsoft.AspNetCore.Identity;

namespace HTKTX.Models
{
    public class UserWithRolesViewModel
    {
        public IdentityUser User { get; set; }
        public List<string> Roles { get; set; }
    }
}
