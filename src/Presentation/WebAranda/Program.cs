using ElectronNET.API;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Persistence.Contexts;
using System;

namespace WebAranda
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            using var serviceScope = host.Services.CreateScope();
            var serviceProvider = serviceScope.ServiceProvider;
            try
            {
                //Crear y migrar las tablas a la base de datos
                serviceProvider.GetRequiredService<SQLiteDBContext>().Database.Migrate();
            }
            catch (Exception e)
            {
                var logging = serviceProvider.GetRequiredService<ILogger<Program>>();
                logging.LogError(e, "Ocurrió un error en la migración: ");
            }

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseElectron(args);
                    webBuilder.UseEnvironment("Development");
                    webBuilder.UseStartup<Startup>();
                });
    }
}
