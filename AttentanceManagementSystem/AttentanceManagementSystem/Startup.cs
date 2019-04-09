using AttentanceManagementSystem.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AttentanceManagementSystem.Startup))]
namespace AttentanceManagementSystem
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
           // AddRoles();
        }


        private void AddRoles()
        {

            ApplicationDbContext db = new ApplicationDbContext();
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            if (!roleManager.RoleExists("Admin"))
            {
                // Add New Role
                var role = new IdentityRole();
                role.Name = "Admin";
                roleManager.Create(role);
                // Create User
                var user = new ApplicationUser() {
                    UserName = "Admin",
            };
                
                var result = userManager.Create(user, "itiN@123");// Take user and Password
                if (result.Succeeded)
                {
                    userManager.AddToRole(user.Id, role.Name);
                }
            }//Basic Admin

            if (!roleManager.RoleExists("Employee"))
            {
                var role = new IdentityRole();
                role.Name = "Employee";
                roleManager.Create(role);
            }
            

        }
    }
}
