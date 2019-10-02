using System;
using System.Diagnostics;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Events;

namespace SampleAPI.API
{
    public class Program
    {
        public static int Main(string[] args)
        {
            Serilog.Debugging.SelfLog.Enable(Console.Error);
            var seqConfiguration = new LoggerConfiguration()
                .MinimumLevel.Debug()
//                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .Enrich.FromLogContext()
                .Enrich.WithMachineName()
                .Enrich.WithEnvironmentUserName()
                .Enrich.WithProperty("ApplicationId", "SampleAPI")
                .WriteTo.Console()
                .WriteTo.File("Logs/errors.txt", LogEventLevel.Error)
                .WriteTo.File("Logs/fatals.txt", LogEventLevel.Fatal)
                .WriteTo.File("Logs/warnings.txt", LogEventLevel.Warning)
                .WriteTo.File("Logs/information.txt", LogEventLevel.Information)
                .WriteTo.Seq("http://seqserver", LogEventLevel.Information);
            if (!Debugger.IsAttached)
                seqConfiguration =
                    seqConfiguration.MinimumLevel.Override(DbLoggerCategory.Database.Command.Name, LogEventLevel.Error);
            Log.Logger = seqConfiguration.CreateLogger();
            try
            {
                Log.Information("Starting web host.");
                CreateWebHostBuilder(args).Build().Run();
                return 0;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly!");
                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseSerilog()
                .UseIISIntegration()
                .UseStartup<Startup>();
    }
}
