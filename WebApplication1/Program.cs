using WebApplication1.Interfaces;
using WebApplication1.Services;
using WebApplication1.Wrappers;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace WebApplication1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddSession();
            builder.Services.AddHttpClient();

            // Add authentication
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/Auth/Login";
                });

            builder.Services.AddAuthorization();

            // Add DI for services
            builder.Services.AddScoped<IGmailServiceHelper, GmailServiceHelper>();
            builder.Services.AddScoped<IEmailParser, EmailParser>();
            builder.Services.AddScoped<IGmailApiWrapper, GmailApiWrapper>();

            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseSession();

            app.UseAuthentication(); // must be before Authorization
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
