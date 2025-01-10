using System;
using RoomBookingSystem.Services;
using RoomBookingSystem.Models;
using System.Data;

namespace RoomBookingSystem
{
    public class AdminRoomService
    {
        private readonly RoomService _roomService;
        private readonly Logger _logger;

        public AdminRoomService(RoomService roomService, Logger logger)
        {
            _roomService = roomService;
            _logger = logger;
        }

        public void AdminMenu(string adminUsername)
        {
            _logger.Log("Admin menu accessed.");
            while (true)
            {
                Console.WriteLine("=====================================");
                Console.WriteLine("             Admin Menu              ");
                Console.WriteLine("=====================================");
                Console.WriteLine("1. Add Room");
                Console.WriteLine("2. Delete Room");
                Console.WriteLine("3. View Room Details");
                Console.WriteLine("4. Logout");
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
                        AddRoom(adminUsername);
                        break;
                    case 2:
                        DeleteRoom(adminUsername);
                        break;
                    case 3:
                        ViewRoomDetailsAdmin();
                        break;
                    case 4:
                        _logger.Log($"Admin logged out: {adminUsername}");
                        Console.WriteLine("Logged out.");
                        return; // Log out and return to main menu
                    default:
                        Console.WriteLine("Invalid option.");
                        break;
                }
            }
        }

        private void AddRoom(string adminUsername)
        {
            try
            {
                Console.Write("Enter Room Number: ");
                string roomNumberInput = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(roomNumberInput) || !int.TryParse(roomNumberInput, out int roomNumber))
                {
                    Console.WriteLine("Invalid Room Number.");
                    return;
                }

                if (_roomService.CheckRoomExists(roomNumber))
                {
                    Console.WriteLine("A room with this number already exists. Please try a different room number.");
                    return;
                }

                Console.Write("Enter Room Type (single, double, triple, quad, suite): ");
                string roomType = Console.ReadLine();
                Console.Write("Enter Price Per Day: ");
                if (!decimal.TryParse(Console.ReadLine(), out decimal pricePerDay))
                {
                    Console.WriteLine("Invalid Price Per Day.");
                    return;
                }

                Room room = new Room
                {
                    RoomNumber = roomNumber,
                    RoomType = roomType,
                    PricePerDay = pricePerDay,
                    IsAvailable = true
                };

                _roomService.AddRoom(room);
                _logger.Log($"Room Number {roomNumber} added by Admin: {adminUsername}");
                Console.WriteLine("Room added successfully.");
            }
            catch (Exception exception)
            {
                Console.WriteLine("Error: " + exception.Message);
                _logger.Log(exception, "Error adding room.");
            }
        }

        private void DeleteRoom(string adminUsername)
        {
            try
            {
                Console.Write("Enter Room Number to Delete: ");
                string roomNumberInput = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(roomNumberInput) || !int.TryParse(roomNumberInput, out int roomNumber))
                {
                    Console.WriteLine("Invalid Room Number.");
                    return;
                }

                // Check if the room number exists
                if (!_roomService.CheckRoomExists(roomNumber))
                {
                    Console.WriteLine("Room number does not exist. Please enter a valid room number.");
                    return;
                }

                _roomService.DeleteRoom(roomNumber);
                _logger.Log($"Room Number {roomNumber} deleted by Admin: {adminUsername}");
                Console.WriteLine("Room deleted successfully.");
            }
            catch (Exception exception)
            {
                Console.WriteLine("Error: " + exception.Message);
                _logger.Log(exception, "Error deleting room.");
            }
        }

        private void ViewRoomDetailsAdmin()
        {
            try
            {
                DataTable dt = _roomService.ViewRoomDetailsAdmin();
                if (dt.Rows.Count == 0)
                {
                    Console.WriteLine("No Rooms added.");
                }
                else
                {
                    Console.WriteLine("===================================================================================================================");
                    Console.WriteLine("                                                         Room Details                                               ");
                    Console.WriteLine("===================================================================================================================");
                    Console.WriteLine("{0,-10} {1,-15} {2,-10} {3,-15} {4,-15} {5,-15} {6,-20}", "Room ID", "Room Number", "Room Type", "Price Per Day", "Booked By", "Availability", "Booked Date");
                    Console.WriteLine(new string('-', 120));
                    foreach (DataRow row in dt.Rows)
                    {
                        string bookedBy = row["BookedBy"].ToString();
                        string availability = row["Availability"].ToString();
                        string bookedDate = row["BookedDate"] == DBNull.Value ? "N/A" : ((DateTime)row["BookedDate"]).ToString("yyyy-MM-dd HH:mm:ss");
                        Console.WriteLine("{0,-10} {1,-15} {2,-10} {3,-15} {4,-15} {5,-15} {6,-20}", row["RoomID"], row["RoomNumber"], row["RoomType"], row["PricePerDay"], bookedBy, availability, bookedDate);
                    }
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine("Error: " + exception.Message);
                _logger.Log(exception, "Error viewing room details for admin.");
            }
        }
    }
}