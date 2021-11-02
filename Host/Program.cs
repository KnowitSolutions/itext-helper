using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using Serilog.Formatting.Compact;
using HostBuilder = Microsoft.Extensions.Hosting.Host;

namespace Host
{
	public class Program
	{
		static Program() =>
			   AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);

		private static ILogger<Program> _logger;

		public static async Task<int> Main(string[] args)
		{
			IHost host;
			Log.Logger = CreateProgramLogger();

			try
			{
				Log.Information("Service is starting up.");
				Log.Information("Initializing the host.");

				host = CreateHostBuilder(args).Build();

				using var scope = host.Services.CreateScope();
				_logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

				Log.Information("Host initialized.");
			}
			catch (Exception ex)
			{
				Log.Fatal(ex, "Host terminated unexpectedly");
				Log.CloseAndFlush();
				throw;
			}

			try
			{
				_logger.LogInformation("Starting the host.");
				await host.RunAsync();
				return 0;
			}
			catch (Exception ex)
			{
				_logger.LogCritical(ex, "Host terminated unexpectedly");
				throw;
			}
			finally
			{
				Log.CloseAndFlush();
			}
		}

		public static IHostBuilder CreateHostBuilder(string[] args) => HostBuilder
			.CreateDefaultBuilder(args)
			.UseWindowsService()
			.UseSerilog(ConfigureSerilog)
			.ConfigureWebHostDefaults(webBuilder => webBuilder
				.ConfigureAppConfiguration(ConfigureAppConfiguration)
				.UseStartup<Startup>()
			);

		private static void ConfigureAppConfiguration(WebHostBuilderContext context, IConfigurationBuilder config)
		{
			void SetConfigurationBasePath()
			{
				var module = Process.GetCurrentProcess().MainModule;
				if (module?.ModuleName != "dotnet" && module?.ModuleName != "dotnet.exe" && module?.ModuleName != "w3wp" && module?.ModuleName != "w3wp.exe")
				{
					config.SetBasePath(Path.GetDirectoryName(module?.FileName));
				}
			}
			SetConfigurationBasePath();
			config.AddJsonFile($"configmap/appsettings.json", optional: true, reloadOnChange: true);
			config.AddJsonFile($"secret/appsettings.json", optional: true, reloadOnChange: true);
		}

		private static void ConfigureSerilog(HostBuilderContext context, LoggerConfiguration config)
		{
			config
				.MinimumLevel.Debug()
				.MinimumLevel.Override("System", LogEventLevel.Warning)
				.MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
				.MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
				.ReadFrom.Configuration(context.Configuration)
				.Enrich.FromLogContext()
			;
		}

		private static Logger CreateProgramLogger() => new LoggerConfiguration()
				.MinimumLevel.Debug()
				.MinimumLevel.Override("System", LogEventLevel.Warning)
				.MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
				.MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
				.Enrich.FromLogContext()
				.WriteTo.Console(new RenderedCompactJsonFormatter())
				.CreateLogger();
	}
}
