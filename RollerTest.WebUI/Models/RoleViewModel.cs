using Microsoft.AspNet.Identity.EntityFramework;
using System.Linq;

namespace RollerTest.WebUI.Models
{
    public class RoleViewModel
    {
        public IQueryable<IdentityRole> Roles { get; set; }
    }
}