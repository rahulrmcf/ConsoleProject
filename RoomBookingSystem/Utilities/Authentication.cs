using System;
using Microsoft.Extensions.Configuration;
using RoomBookingSystem.Services;

namespace RoomBookingSystem
{
    public class Authentication
    {
        private readonly UserService _userService;
        private readonly IConfiguration _configuration;

        public Authentication(UserService userService, IConfiguration configuration)
        {
            _userService = userService;
            _configuration = configuration;
        }

        public bool ValidateUser(string username, string password, bool isAdmin)
        {
            return _userService.ValidateUser(username, password, isAdmin);
        }

        public bool CheckUserExists(string username)
        {
            return _userService.CheckUserExists(username);
        }

        public void RegisterUser(string username, string password)
        {
            _userService.RegisterUser(username, password);
        }

        public static string ReadPassword()
        {
            string password = string.Empty;
            ConsoleKey key;

            do
            {
                var keyInfo = Console.ReadKey(intercept: true);
                key = keyInfo.Key;

                if (key == ConsoleKey.Backspace && password.Length > 0)
                {
                    Console.Write("\b \b");
                    password = password[0..^1];
                }
                else if (!char.IsControl(keyInfo.KeyChar))
                {
                    Console.Write("*");
                    password += keyInfo.KeyChar;
                }
            } while (key != ConsoleKey.Enter);

            Console.WriteLine();
            return password;
        }
    }
}