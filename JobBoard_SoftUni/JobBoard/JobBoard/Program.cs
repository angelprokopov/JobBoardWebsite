using JobBoard.Data;
using JobBoard.Data.Models;
using JobBoard.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using JobBoard.Data.Interfaces;

namespace JobBoard
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var config = builder.Configuration;

            // Add services
            builder.Services.AddControllersWithViews();
            builder.Services.AddRazorPages();

            // Configure DbContext
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                                   ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            builder.Services.AddDbContext<JobBoardContext>(options =>
                options.UseSqlServer(connectionString));

            // builder.Services.AddDefaultIdentity<User>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<JobBoardContext>();

           // builder.Services.AddDefaultIdentity<User>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<JobBoardContext>();

            // Configure Identity
            builder.Services.AddIdentity<User, Role>(options =>
            {
                options.Password.RequireDigit = true;
                options.SignIn.RequireConfirmedAccount = true;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(8);
                options.Lockout.MaxFailedAccessAttempts = 6;
            })
            .AddRoles<Role>()
            .AddEntityFrameworkStores<JobBoardContext>()
            .AddDefaultTokenProviders();

            // Add external authentication providers
            builder.Services.AddAuthentication()
                .AddGoogle(options =>
                {
                    options.ClientId = config["Authentication:Google:ClientId"];
                    options.ClientSecret = config["Authentication:Google:ClientSecret"];
                });

            // Add authorization policies
            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
            });

            // Email sender and custom services
            builder.Services.AddTransient<IEmailSender, EmailSender>();
            builder.Services.Configure<AuthMessageSenderOptions>(builder.Configuration);
            builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

            // Configure authentication cookie
            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromDays(14);
                options.LoginPath = "/Account/Login";
                options.LogoutPath = "/Account/Logout";
                options.SlidingExpiration = true;
            });

            var app = builder.Build();

            // Seed the database
            await SeedDatabaseAsync(app);

            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Error/Error500");
                app.UseHsts();
            }

            app.UseStatusCodePagesWithReExecute("/Error/{0}");
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapRazorPages();

            await app.RunAsync();
        }

        private static async Task SeedDatabaseAsync(WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<JobBoardContext>();

                if (!context.JobCategories.Any())
                {
                    var categories = new[]
                    {
                        new JobCategory {Id = Guid.NewGuid(), Name = "Finance"},
                        new JobCategory {Id = Guid.NewGuid(), Name = "IT"}
                    };

                    context.JobCategories.AddRange(categories);
                    await context.SaveChangesAsync();
                }

                if (!context.Companies.Any())
                {
                    var companies = new[]
                    {
                        new Company {Id = Guid.NewGuid(), Name = "DXC Technology", Location = "Sofia", Description = "A leading tech company."},
                        new Company {Id = Guid.NewGuid(), Name = "myPOS", Location = "Varna", Description = "Fintech solutions."}
                    };

                    context.Companies.AddRange(companies);
                    await context.SaveChangesAsync();
                }

                if (!context.Jobs.Any())
                {
                    var jobCategories = context.JobCategories.ToList();
                    var companies = context.Companies.ToList();

                    var jobs = new[]
                    {
                        new Job
                        {
                            Id = Guid.NewGuid(),
                            Title = "Senior Network Specialist",
                            Salary = 2600,
                            PostDate = DateTime.Now,
                            ExperienceLevel = "",
                            EmploymentType = "Full-Time",
                            Location = "Remote",
                            CompanyId = companies[0].Id,
                            CategoryId = jobCategories[0].Id,
                        },
                        new Job
                        {
                            Id = Guid.NewGuid(),
                            Title = "Senior C# Developer",
                            Salary = 2100,
                            PostDate = DateTime.Now,
                            ExperienceLevel = "",
                            EmploymentType = "Full-Time",
                            Location = "Sofia",
                            CompanyId = companies[1].Id,
                            CategoryId = jobCategories[1].Id,
                        }
                    };

                    context.Jobs.AddRange(jobs);
                    await context.SaveChangesAsync();
                }
            }
        }
    }
}
