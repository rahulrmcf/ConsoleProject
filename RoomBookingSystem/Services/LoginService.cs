using System;
using RoomBookingSystem.Services;

namespace RoomBookingSystem
{
    public class LoginService
    {
        private readonly Authentication _authentication;
        private readonly Logger _logger;
        private readonly AdminRoomService _adminRoomService;
        private readonly UserRoomService _userRoomService;

        public LoginService(Authentication authentication, Logger logger, AdminRoomService adminRoomService, UserRoomService userRoomService)
        {
            _authentication = authentication;
            _logger = logger;
            _adminRoomService = adminRoomService;
            _userRoomService = userRoomService;
        }

        public void Run()
        {
            _logger.Log("STARTING APPLICATION.");
            while (true)
            {
                Console.WriteLine("=====================================");
                Console.WriteLine(" Welcome to the Room Booking System  ");
                Console.WriteLine("=====================================");
                Console.WriteLine("1. Admin Login");
                Console.WriteLine("2. User Login");
                Console.WriteLine("3. User Registration");
                Console.WriteLine("4. Exit");
                Console.Write("Select an option: ");
                int choice;
                if (!int.TryParse(Console.ReadLine(), out choice))
                {
                    Console.WriteLine("Invalid input. Please enter a number.");
                    continue;
                }

                switch (choice)
                {
                    case 1:
                        AdminLogin();
                        break;
                    case 2:
                        UserLogin();
                        break;
                    case 3:
                        UserRegistration();
                        break;
                    case 4:
                        _logger.Log("STOPPING APPLICATION.");
                        return; // Exit the program
                    default:
                        Console.WriteLine("Invalid option.");
                        break;
                }
            }
        }

        private void AdminLogin()
        {
            _logger.Log("Admin login accessed.");
            Console.WriteLine("=====================================");
            Console.WriteLine("             Admin Login             ");
            Console.WriteLine("=====================================");
            Console.Write("Username: ");
            string username = Console.ReadLine();
            Console.Write("Password: ");
            string password = Authentication.ReadPassword();

            if (_authentication.ValidateUser(username, password, true))
            {
                Console.WriteLine("Login successful.");
                _logger.Log($"Admin logged in: {username}");
                _adminRoomService.AdminMenu(username);
            }
            else
            {
                Console.WriteLine("Invalid credentials.");
                _logger.Log("Admin login failed.");
            }
        }

        private void UserLogin()
        {
            _logger.Log("User login accessed.");
            Console.WriteLine("=====================================");
            Console.WriteLine("              User Login             ");
            Console.WriteLine("=====================================");
            Console.Write("Username: ");
            string username = Console.ReadLine();
            Console.Write("Password: ");
            string password = Authentication.ReadPassword();

            if (_authentication.ValidateUser(username, password, false))
            {
                Console.WriteLine("Login successful.");
                _logger.Log($"User logged in: {username}");
                _userRoomService.UserMenu(username);
            }
            else
            {
                Console.WriteLine("Invalid credentials.");
                _logger.Log("User login failed.");
            }
        }

        private void UserRegistration()
        {
            _logger.Log("User registration accessed.");
            Console.WriteLine("=====================================");
            Console.WriteLine("          User Registration          ");
            Console.WriteLine("=====================================");
            Console.Write("Enter Username: ");
            string username = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(username))
            {
                Console.WriteLine("Username cannot be empty.");
                return;
            }

            // Check if the username already exists
            if (_authentication.CheckUserExists(username))
            {
                Console.WriteLine("A user with the same username already exists, please try another one.");
                return; // Exit the registration process
            }

            Console.Write("Enter Password: ");
            string password = Authentication.ReadPassword();

            if (string.IsNullOrWhiteSpace(password))
            {
                Console.WriteLine("Password cannot be empty.");
                return;
            }

            _authentication.RegisterUser(username, password);
            _logger.Log($"User registered: {username}");
            Console.WriteLine("User registered successfully.");
        }
    }
}