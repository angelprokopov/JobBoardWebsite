using JobBoard.Data;
using JobBoard.Data.Models;
using JobBoard.Services;
using JobBoard.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using static JobBoard.Areas.Identity.Pages.Account.RegisterModel;

namespace JobBoard
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var config = builder.Configuration;
            
            builder.Services.AddControllersWithViews();
            builder.Services.AddRazorPages();
            
            
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
           
            builder.Services.AddDbContext<JobBoardContext>(options =>
                options.UseSqlServer(connectionString));
            
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();


            builder.Services.AddIdentity<User, Role>(options =>
            {
                options.Password.RequireDigit = true;
                options.SignIn.RequireConfirmedAccount = true;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(8); // The time that the user have before he's lockout
                options.Lockout.MaxFailedAccessAttempts = 6; // Maximum attepts for entering the password
            })
                 .AddRoles<Role>()
                 .AddEntityFrameworkStores<JobBoardContext>()
                 .AddDefaultTokenProviders();
            
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

            builder.Services.AddTransient<IEmailSender, EmailSender>();

            builder.WebHost.ConfigureKestrel(options =>
            {
                options.ListenLocalhost(5001, listenOptions =>
                {
                    listenOptions.UseHttps();
                });
            });

            builder.Services.Configure<AuthMessageSenderOptions>(builder.Configuration);

            builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Account/Login";
                options.LogoutPath = "/Account/Logout";
            });


            var app = builder.Build();

            //Dynamically seeding the database
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
                        new JobCategory {Id =  Guid.NewGuid(), Name = "Финанси"},
                        new JobCategory {Id = Guid.NewGuid(), Name = "IT" }
                    };

                    context.JobCategories.AddRange(categories);
                    await context.SaveChangesAsync();
                }

                if (!context.Companies.Any())
                {
                    var companies = new[]
                    {
                        new Company {Id = Guid.NewGuid(),Name = "DXC Technology / DXC Bulgaria EOOD\r\n", Location = "София", Description = "DXC Technology is a Fortune 500 global IT services leader. Our more than 130,000 people in 70-plus countries are entrusted by our customers to deliver what matters most. "},
                        new Company {Id = Guid.NewGuid(), Name = "myPOS Technologies EAD\r\n", Location = "Варна", Description = "myPOS is an international fintech company. Our team of 750+ people is located in 18 offices all over Europe. The company works in 30+ markets in the continent and has more than 200,000 business customers." }
                    };

                    context.Companies.AddRange(companies);
                    await context.SaveChangesAsync();
                }

                if (context.Jobs.Any())
                {
                    var jobCategories = context.JobCategories.ToList();
                    var companies = context.Companies.ToList();

                    var jobs = new[]
                    {
                        new Job
                        {
                            Id = Guid.NewGuid(),
                            Title = "Senior Network Infrastructure Specialist/ Remote/ Hybrid\r\n",
                            Salary = 2600,
                            PostDate = DateTime.Now,
                            Location = "София",
                            CompanyId = companies[0].Id,
                            CategoryId = jobCategories[0].Id,
                        },
                        new Job
                        {
                            Id = Guid.NewGuid(),
                            Title = "Senior C# (.NET) Developer\r\n",
                            Salary = 2100,
                            PostDate = DateTime.Now,
                            Location = "София",
                            CompanyId = companies[1].Id,
                            CategoryId = jobCategories[1].Id,
                        }
                    };
                }
            }
        }
    }
}
