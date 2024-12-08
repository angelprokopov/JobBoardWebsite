using JobBoard.Data;
using JobBoard.Data.Models;
using JobBoard.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using JobBoard.Data.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;

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

            // Configure Identity
            builder.Services.AddIdentity<User, Role>(options =>
            {
                options.Password.RequireDigit = true;
                options.SignIn.RequireConfirmedAccount = true;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(8);
                options.Lockout.MaxFailedAccessAttempts = 6;
                options.Tokens.EmailConfirmationTokenProvider = "Default";
            })
            .AddRoles<Role>()
            .AddEntityFrameworkStores<JobBoardContext>()
            .AddDefaultTokenProviders();

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
                options.LoginPath = "/Identity/Account/Login";
                options.AccessDeniedPath = "/Identity/Account/AccessDenied";
                options.LogoutPath = "/Identity/Account/Logout";
                options.SlidingExpiration = true;
            });

            builder.Services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new[] {new System.Globalization.CultureInfo("bg-BG")};
                options.DefaultRequestCulture = new Microsoft.AspNetCore.Localization.RequestCulture("bg-BG");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
            });

            var app = builder.Build();

            var localizationOptions = new RequestLocalizationOptions()
                .SetDefaultCulture("bg-BG")
                .AddSupportedCultures("bg-BG")
                .AddSupportedUICultures("bg-BG");

            app.UseRequestLocalization(localizationOptions);

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

            app.Use(async (context, next) =>
            {
                context.Response.ContentType = "text/html; charset=utf-8";
                await next();
            });

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

                if (!await context.Database.CanConnectAsync())
                {
                    Console.WriteLine("Unable to connect to the database.");
                    return;
                }

                try
                {
                    Random rnd = new Random();

                    // Deleting existing records to reseed database (optional)
                    context.Jobs.RemoveRange(context.Jobs);
                    context.Companies.RemoveRange(context.Companies);
                    context.JobCategories.RemoveRange(context.JobCategories);
                    await context.SaveChangesAsync();

                    Console.WriteLine("Old records deleted.");

                    // Seed Job Categories
                    if (!context.JobCategories.Any())
                    {
                        var categories = new[]
                        {
                    new JobCategory {Id = Guid.NewGuid(), Name = "IT"},
                    new JobCategory {Id = Guid.NewGuid(), Name = "Finance"},
                    new JobCategory {Id = Guid.NewGuid(), Name = "Marketing"},
                    new JobCategory {Id = Guid.NewGuid(), Name = "Human Resources"}
                };
                        context.JobCategories.AddRange(categories);
                        await context.SaveChangesAsync();
                        Console.WriteLine("Job categories seeded.");
                    }

                    // Seed Companies
                    if (!context.Companies.Any())
                    {
                        var companies = new[]
                        {
                    new Company {Id = Guid.NewGuid(), Name = "DXC Technology", Location = "Sofia", Description = "A leading tech company."},
                    new Company {Id = Guid.NewGuid(), Name = "myPOS", Location = "Varna", Description = "Fintech solutions."},
                    new Company {Id = Guid.NewGuid(), Name = "IBM Bulgaria", Location = "Plovdiv", Description = "A leading provider of IT services."},
                    new Company {Id = Guid.NewGuid(), Name = "SAP Labs", Location = "Burgas", Description = "An enterprise application software company."}
                };
                        context.Companies.AddRange(companies);
                        await context.SaveChangesAsync();
                        Console.WriteLine("Companies seeded.");
                    }

                    // Seed Jobs
                    if (!context.Jobs.Any())
                    {
                        var jobCategories = context.JobCategories.ToList();
                        var companies = context.Companies.ToList();

                        var jobTitles = new List<string>
                {
                    "Junior Software Developer",
                    "Senior Network Specialist",
                    "Marketing Specialist",
                    "Finance Analyst",
                    "HR Specialist",
                    "IT Support Technician"
                };

                        var employmentTypes = new[] { "Full-Time", "Part-Time", "Contract" };
                        var experienceLevels = new[] { "Junior", "Mid", "Senior" };
                        var locations = new[] { "Remote", "On-site", "Hybrid" };
                        var descriptions = new List<string>
                {
                    "This position requires a passionate individual ready to grow and learn.",
                    "We are looking for a motivated person to join our growing team.",
                    "The ideal candidate will be responsible for designing and developing scalable solutions.",
                    "The job involves building and maintaining web applications using the latest technologies."
                };
                        var responsibilitiesList = new List<string>
                {
                    "- Write clean and efficient code.\n- Debug and maintain applications.\n- Collaborate with team members.",
                    "- Implement high-quality software according to requirements.\n- Participate in code reviews.\n- Assist junior developers.",
                    "- Troubleshoot and resolve customer issues.\n- Manage software configurations.\n- Ensure optimal system performance.",
                    "- Lead a small team of developers.\n- Manage project timelines.\n- Interface with clients to gather project requirements."
                };
                        var requirementsList = new List<string>
                {
                    "- Bachelor's degree in Computer Science or related field.\n- 1+ years of experience with C# or Java.",
                    "- Excellent problem-solving skills.\n- Experience working in an Agile environment.",
                    "- Strong understanding of REST APIs.\n- Ability to work independently.\n- Knowledge of cloud platforms.",
                    "- Proficiency in one or more programming languages.\n- Excellent teamwork skills.\n- Experience with Git."
                };
                        var benefitsList = new List<string>
                {
                    "- Competitive salary.\n- Health and dental insurance.\n- Flexible working hours.",
                    "- Annual performance bonus.\n- Work from home opportunities.\n- Paid time off and holidays.",
                    "- Company-sponsored health programs.\n- Free parking.\n- Gym membership discounts.",
                    "- Relocation package for qualified candidates.\n- Free snacks and coffee.\n- Collaborative work environment."
                };

                        var jobs = new List<Job>();

                        for (int i = 0; i < 10; i++) // Seeding 10 jobs as an example
                        {
                            var job = new Job
                            {
                                Id = Guid.NewGuid(),
                                Title = jobTitles[rnd.Next(jobTitles.Count)],
                                Salary = rnd.Next(2000, 6000), // Random salary between 2000 and 6000
                                PostDate = DateTime.Now.AddDays(-rnd.Next(0, 30)), // Random post date within the last 30 days
                                ExperienceLevel = experienceLevels[rnd.Next(experienceLevels.Length)],
                                EmploymentType = employmentTypes[rnd.Next(employmentTypes.Length)],
                                Location = locations[rnd.Next(locations.Length)],
                                CompanyId = companies[rnd.Next(companies.Count)].Id,
                                CategoryId = jobCategories[rnd.Next(jobCategories.Count)].Id,
                                Description = descriptions[rnd.Next(descriptions.Count)],
                                Responsibilities = responsibilitiesList[rnd.Next(responsibilitiesList.Count)],
                                Requirements = requirementsList[rnd.Next(requirementsList.Count)],
                                Benefits = benefitsList[rnd.Next(benefitsList.Count)]
                            };

                            jobs.Add(job);
                        }

                        context.Jobs.AddRange(jobs);
                        await context.SaveChangesAsync();
                        Console.WriteLine("Jobs seeded.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error during seeding: {ex.Message}");
                }
            }
        }


    }
}
