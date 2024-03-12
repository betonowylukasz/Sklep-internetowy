using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace lista10.Data
{
    public class MyIdentityDataInitializer
    {
        public static async Task SeedData(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            SeedRoles(roleManager);
            SeedUsers(userManager);
        }
        // name - poprawny adres email
        // password - min 8 znaków, mała i duża litera, cyfra i znak specjalny
        public static void SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            if (!roleManager.RoleExistsAsync("Admin").Result)
            {
                IdentityRole role = new IdentityRole
                {
                    Name = "Admin",
                };
                IdentityResult roleResult = roleManager.CreateAsync(role).Result;
            }

            if (!roleManager.RoleExistsAsync("Pracownik").Result)
            {
                IdentityRole role = new IdentityRole
                {
                    Name = "Pracownik",
                };
                IdentityResult roleResult = roleManager.CreateAsync(role).Result;
            }
        }
        public static void SeedOneUser(UserManager<IdentityUser> userManager, string name, string password, string role = null)
        {
            if (userManager.FindByNameAsync(name).Result == null)
            {
                IdentityUser user = new IdentityUser
                {
                    UserName = name, // musi być taki sam jak email, inaczej nie zadziała
                    Email = name
                };
                IdentityResult result = userManager.CreateAsync(user, password).Result;
                if (result.Succeeded && role != null)
                {
                    userManager.AddToRoleAsync(user, role).Wait();
                }
            }
        }
        public static async Task SeedUsers(UserManager<IdentityUser> userManager)
        {
            SeedOneUser(userManager, "normaluser@localhost", "User123!");
            SeedOneUser(userManager, "adminuser@localhost", "Admin123!", "Admin");
            SeedOneUser(userManager, "workeruser@localhost", "Worker123!", "Pracownik");
        }

    }
}
