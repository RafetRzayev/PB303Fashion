using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using PB303Fashion.DataAccessLayer;
using PB303Fashion.Models;

namespace PB303Fashion
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllersWithViews();

            builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(10);
            });

            Constants.CategoryImagePath = Path.Combine(builder.Environment.WebRootPath, "assets", "svg", "fashion");
            Constants.GalleryImagePath = Path.Combine(builder.Environment.WebRootPath, "assets", "images", "gallery");

            var app = builder.Build();

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSession();
          
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "areas",
                    pattern: "{area:exists}/{controller=Category}/{action=Index}/{id?}");

                app.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });           

            app.Run();
        }
    }
}