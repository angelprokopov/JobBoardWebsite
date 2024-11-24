using JobBoard.Data;
using JobBoard.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace JobBoard
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var config = builder.Configuration;

            // Add services to the container.
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            builder.Services.AddDbContext<JobBoardContext>(options =>
                options.UseSqlServer(connectionString));
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            builder.Services.AddIdentity<User, Role>()
                .AddEntityFrameworkStores<JobBoardContext>()
                .AddDefaultTokenProviders();

            builder.Services.AddDefaultIdentity<User>(options => {
                options.Password.RequireDigit = true;
                options.SignIn.RequireConfirmedAccount = true;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(8); //
                options.Lockout.MaxFailedAccessAttempts = 6;
            })
                .AddRoles<Role>()
                 .AddEntityFrameworkStores<JobBoardContext>();
            builder.Services.AddControllersWithViews();

            builder.Services.AddAuthentication()
                .AddGoogle(options =>
                {
                    options.ClientId = config["Authentication:Google:ClientId"];
                    options.ClientSecret = config["Authentication:Google:ClientSecret"];
                });

            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
            });

            builder.WebHost.ConfigureKestrel(options =>
            {
                options.ListenLocalhost(7136, listenOptions =>
                {
                    listenOptions.UseHttps();
                });
            });

            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Account/Login";
                options.LogoutPath = "/Account/Logout";
            });


            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }


            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();    
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapRazorPages();

            app.Run();
        }
    }
}
