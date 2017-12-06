using Microsoft.AspNet.Identity.EntityFramework;
using System.Linq;

namespace RollerTest.WebUI.Models
{
    public class UserRolesViewModel
    {
        public IQueryable<IdentityUserRole> UserRoles { get; set; }
        public IQueryable<IdentityRole> Roles { get; set; }

    }
}