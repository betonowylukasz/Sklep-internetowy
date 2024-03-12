using lista10.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace lista10
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddDbContextPool<MyDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("MyDb")));
            services.AddDefaultIdentity<IdentityUser>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<MyDbContext>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            MyIdentityDataInitializer.SeedData(userManager, roleManager);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

                endpoints.MapRazorPages();

                endpoints.MapControllerRoute(
                    name: "articles",
                    pattern: "{controller=Articles}/{action=Index}");

                endpoints.MapControllerRoute(
                    name: "create",
                    pattern: "{controller=Articles}/{action=Create}");

                endpoints.MapControllerRoute(
                    name: "edit",
                    pattern: "{controller=Articles}/{action=Edit}/{id?}");

                endpoints.MapControllerRoute(
                    name: "detail",
                    pattern: "{controller=Articles}/{action=Details}/{id?}");

                endpoints.MapControllerRoute(
                    name: "delete",
                    pattern: "{controller=Articles}/{action=Delete}/{id?}");

                endpoints.MapControllerRoute(
                    name: "shop",
                    pattern: "{controller=Shop}/{action=Index}");

                endpoints.MapControllerRoute(
                    name: "shop2",
                    pattern: "{controlle=Shop}/{action=Index}/{id?}");
            });
        }
    }
}
