using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Quartz;
using System;
using voteCollector.Data;
using voteCollector.ModelsHttpSender;

namespace voteCollector
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // ����������� ������� (�������� �����������), ������� ����� ������������������ ��� ������ ����������
        public void ConfigureServices(IServiceCollection services)
        {
            //Add
            // ��������� ������������ �����������
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options => //CookieAuthenticationOptions
                {
                    options.LoginPath = new Microsoft.AspNetCore.Http.PathString("/Account/login");
                    options.AccessDeniedPath = new Microsoft.AspNetCore.Http.PathString("/Account/login");
                });
            //Add
            services.AddDbContext<VoterCollectorContext>();
            //Add
            services.AddHttpClient();

            services.AddControllersWithViews();
            //Add
            services.Configure<IdentityOptions>(options =>
            {
                // Password settings.
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 8;
                options.Password.RequiredUniqueChars = 1;

                // Lockout settings.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);
                options.Lockout.MaxFailedAccessAttempts = 7;
                options.Lockout.AllowedForNewUsers = true;

                // User settings.
                options.User.AllowedUserNameCharacters =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = false;
                
            });

            //Add ???
            services.ConfigureApplicationCookie(options =>
            {
                // Cookie settings
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(7);

                options.LoginPath = "/Account/login";
                options.AccessDeniedPath = "/Account/login";
                options.SlidingExpiration = true;
            });

            //Add
            services.AddControllers().AddNewtonsoftJson();

            // Add
            //services.AddQuartz(q =>
            //{
            //    q.UseMicrosoftDependencyInjectionJobFactory();

            //    // Create a "key" for the job
            //    var jobKey = new JobKey("RequestJob");

            //    // Register the job with the DI container
            //    q.AddJob<RequestJob>(opts => opts.WithIdentity(jobKey));

            //    // Create a trigger for the job
            //    q.AddTrigger(opts => opts
            //        .ForJob(jobKey) // link to the RequestJob
            //        .WithIdentity("RequestJob-trigger") // give the trigger a unique name
            //        .StartNow()
            //        .WithSimpleSchedule(x =>
            //            x.WithIntervalInMinutes(20)
            //            .RepeatForever()));
            //});
            //services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            //Add
            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();

            //Add
            //����� app.UseAuthentication() ���������� � �������� ��������� AuthenticationMiddleware, ������� ��������� ���������������.
            //��� ����� ��������� ���������� �������� ��� �������� HttpContext.User.
            //�������������� �������� �� ������, ��� ������������.
            app.UseAuthentication();

            ////Add ???
            //app.UseMvc();

            //����� app.UseAuthorization() ���������� � �������� ��������� AuthorizationMiddleware,
            //������� ��������� ������������ ������������� � �������������� ������ � ��������.
            //����������� �������� �� ������, ����� ����� � ������� ����� ������������, ��������� ������������ ������ � �������� ����������.
            app.UseAuthorization();            

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            //Add ???
            var cookiePolicyOptions = new CookiePolicyOptions
            {
                MinimumSameSitePolicy = SameSiteMode.Strict,
            };
            //Add ???
            app.UseCookiePolicy(cookiePolicyOptions);

            //app.UseCors();
        }
    }
}
