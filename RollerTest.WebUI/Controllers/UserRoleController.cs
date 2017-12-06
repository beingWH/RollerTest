using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using RollerTest.WebUI.Models;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace RollerTest.WebUI.Controllers
{
    public class UserRoleController : Controller
    {
        // GET: UserRole
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            return View();
        }
        //
        //Role
        [Authorize(Roles = "Admin")]
        public ActionResult RoleIndex()
        {
            var rm = new RoleManager<IdentityRole>(
            new RoleStore<IdentityRole>(new ApplicationDbContext()));
            RoleViewModel rvm = new RoleViewModel()
            {
                Roles = rm.Roles
            };
            return View(rvm);
        }
        [Authorize(Roles = "Admin")]
        public ActionResult CreateRoles()
        {
            var rolename = Request["rolename"];
            IdentityManager idm = new IdentityManager();
            idm.CreateRole(rolename);
            return RedirectToActionPermanent("RoleIndex", "UserRole");
        }
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteRoles(string roleid)
        {
            IdentityManager idm = new IdentityManager();
            idm.DeleteRole(roleid);
            return RedirectToActionPermanent("RoleIndex", "UserRole");
        }

        //
        //User
        [Authorize(Roles = "Admin")]
        public ActionResult UserIndex()
        {
            var um = new UserManager<ApplicationUser>(
                new UserStore<ApplicationUser>(new ApplicationDbContext()));
            UserViewModel uvm = new UserViewModel()
            {
                Users = um.Users.Include(x => x.Roles)
            };
            return View(uvm);
        }
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteUsers(string userid)
        {
            IdentityManager idm = new IdentityManager();
            idm.DeleteUser(userid);
            return RedirectToActionPermanent("UserIndex", "UserRole");
        }
        [Authorize(Roles = "Admin")]
        public ActionResult UserRolesIndex(string userId)
        {
            var um = new UserManager<ApplicationUser>(
            new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var rm = new RoleManager<IdentityRole>(
            new RoleStore<IdentityRole>(new ApplicationDbContext()));
            ViewData["userId"] = userId;
            ViewData["RoleList"] = rm.Roles.Select(a => new SelectListItem
            {
                Text = a.Name,
                Value = a.Name
            });
            UserRolesViewModel urvm = new UserRolesViewModel()
            {
                UserRoles = um.FindById(userId).Roles.AsQueryable(),
                Roles = rm.Roles
            };
            return View(urvm);
        }
        [Authorize(Roles = "Admin")]
        public ActionResult CreateUserRole()
        {
            var rolename = Request["rolename"];
            var userid = Request["userid"];
            IdentityManager idm = new IdentityManager();
            idm.AddUserToRole(userid, rolename);
            return RedirectToActionPermanent("UserRolesIndex", "UserRole", new { userId = userid });

        }
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteUserRoles(string userId, string roleName)
        {
            IdentityManager idm = new IdentityManager();
            idm.DeleteUserRole(userId, roleName);
            return RedirectToActionPermanent("UserRolesIndex", "UserRole", new { userId = userId });
        }
        [Authorize(Roles = "Admin")]
        public ActionResult ClearUserRoles(string userId)
        {
            IdentityManager idm = new IdentityManager();
            idm.ClearUserRoles(userId);
            return RedirectToActionPermanent("UserRolesIndex", "UserRole", new { userId = userId });
        }
        public PartialViewResult LoginName()
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            ApplicationUser user = userManager.FindByNameAsync(User.Identity.Name).Result;
            return PartialView(user);
        }

    }
}