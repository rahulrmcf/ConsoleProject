/*  Title : Room Booking System
    Author: Rahul P R
    Created at : 22/12/2024
    Updated at : 25/12/2024
    Reviewed by : Sabapathi Shanmugam
    Reviewed at : 26/12/2024 */

using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using RoomBookingSystem.Services;

namespace RoomBookingSystem
{
    class Program
    {
        static void Main(string[] args)
        {
            // Configure Serilog for logging to a file
            Log.Logger = new LoggerConfiguration()
                .WriteTo.File("log.txt", rollingInterval: RollingInterval.Infinite)
                .CreateLogger();

            var host = CreateHostBuilder(args).Build();

            var loginService = host.Services.GetRequiredService<LoginService>();
            loginService.Run();
        }

        static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog() // Use Serilog for logging
                .ConfigureAppConfiguration((context, config) =>
                {
                    config.SetBasePath(AppDomain.CurrentDomain.BaseDirectory);
                    config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                })
                .ConfigureServices((context, services) =>
                {
                    IConfiguration configuration = context.Configuration;

                    // Register services with DI container
                    services.AddSingleton<IConfiguration>(configuration);
                    services.AddSingleton<UserService>();
                    services.AddSingleton<RoomService>();
                    services.AddSingleton<BookingService>();
                    services.AddSingleton<Authentication>(); 
                    services.AddSingleton<Logger>(); 
                    services.AddSingleton<LoginService>(); 
                    services.AddSingleton<AdminRoomService>(); 
                    services.AddSingleton<UserRoomService>(); 
                });
    }
}