using System.Linq;

namespace RollerTest.WebUI.Models
{
    public class UserViewModel
    {
        public IQueryable<ApplicationUser> Users { get; set; }
    }
}