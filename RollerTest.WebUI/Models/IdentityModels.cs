using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RollerTest.WebUI.Models
{
    // 可以通过向 ApplicationUser 类添加更多属性来为用户添加配置文件数据。若要了解详细信息，请访问 http://go.microsoft.com/fwlink/?LinkID=317594。
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public string Name { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // 请注意，authenticationType 必须与 CookieAuthenticationOptions.AuthenticationType 中定义的相应项匹配
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // 在此处添加自定义用户声明
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }

    public class IdentityManager
    {
        //判斷角色是否已在存在
      public bool RoleExists(string name)
        {
            var rm = new RoleManager<IdentityRole>(
                new RoleStore<IdentityRole>(new ApplicationDbContext()));
            return rm.RoleExists(name);
        }
        //新增角色
      public bool CreateRole(string name)
        {
            var rm = new RoleManager<IdentityRole>(
                new RoleStore<IdentityRole>(new ApplicationDbContext()));
            var idResult = rm.Create(new IdentityRole(name));
            return idResult.Succeeded;
        }
        //删除角色
        public bool DeleteRole(string roleId)
        {
            var rm = new RoleManager<IdentityRole>(
            new RoleStore<IdentityRole>(new ApplicationDbContext()));
            var idResult = rm.Delete(rm.FindById(roleId));
            return idResult.Succeeded;
        }
        //新增用户
      public bool CreateUser(ApplicationUser user, string password)
        {
            var um = new UserManager<ApplicationUser>(
                new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var idResult = um.Create(user, password);
            return idResult.Succeeded;
        }
        //删除用户
        public bool DeleteUser(string userid)
        {
            var um = new UserManager<ApplicationUser>(
                new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var idResult = um.Delete(um.FindById(userid));
            return idResult.Succeeded;
        }

        //將使用者加入角色中
        public bool AddUserToRole(string userId, string roleName)
        {
            var um = new UserManager<ApplicationUser>(
                new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var idResult = um.AddToRole(userId, roleName);
            return idResult.Succeeded;
        }
        //清除使用者的角色設定
      public void ClearUserRoles(string userId)
        {
            var um = new UserManager<ApplicationUser>(
                new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var rm = new RoleManager<IdentityRole>(
                new RoleStore<IdentityRole>(new ApplicationDbContext()));
            var user = um.FindById(userId);
            var currentRoles = new List<IdentityUserRole>();
            currentRoles.AddRange(user.Roles);
            foreach (var role in currentRoles)
            {
                
                um.RemoveFromRole(userId, rm.FindById(role.RoleId).Name);
            }
        }
        public void DeleteUserRole(string userId,string roleName)
        {
            var um = new UserManager<ApplicationUser>(
               new UserStore<ApplicationUser>(new ApplicationDbContext()));
            um.RemoveFromRole(userId, roleName);
        }
    }



}