using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Voting.Data.Data;

namespace Voting.WebApp
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            using(var scope = host.Services.CreateScope())
            using (var context = scope.ServiceProvider.GetService<ApplicationDbContext>())
            using(var userManager = scope.ServiceProvider.GetService<UserManager<IdentityUser>>())
            {
                await context.Database.EnsureDeletedAsync();
                await context.Database.EnsureCreatedAsync();
                // await context.Database.MigrateAsync();

                var testUser = new IdentityUser()
                {
                    UserName = "test@test.com",
                    Email = "test@test.com",
                    Id = "B31BCA55-7146-4E4C-8FC4-786216D4C92A",
                    EmailConfirmed = true
                };
                var otherUser = new IdentityUser()
                {
                    UserName = "other@test.com",
                    Email = "other@test.com",
                    Id = "9546E2DC-944E-4F26-B9F8-556E261BFD3D",
                    EmailConfirmed = true
                };
                if (await userManager.FindByEmailAsync("test@test.com") == null)
                {
                    var result =await userManager.CreateAsync(testUser, "Test123.");    
                }

                if (await userManager.FindByEmailAsync("other@test.com") == null)
                {
                    await userManager.CreateAsync(otherUser, "Test123.");
                }
                
            }
                
                
                host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
    }
}