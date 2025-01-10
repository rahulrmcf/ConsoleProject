using System;
using RoomBookingSystem.Services;
using System.Data;

namespace RoomBookingSystem
{
    public class UserRoomService
    {
        private readonly UserService _userService;
        private readonly RoomService _roomService;
        private readonly BookingService _bookingService;
        private readonly Logger _logger;

        public UserRoomService(UserService userService, RoomService roomService, BookingService bookingService, Logger logger)
        {
            _userService = userService;
            _roomService = roomService;
            _bookingService = bookingService;
            _logger = logger;
        }

        public void UserMenu(string username)
        {
            while (true)
            {
                Console.WriteLine("=====================================");
                Console.WriteLine("               User Menu             ");
                Console.WriteLine("=====================================");
                Console.WriteLine("1. Book Room");
                Console.WriteLine("2. Cancel Booking");
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
                        BookRoom(username);
                        break;
                    case 2:
                        CancelBooking(username);
                        break;
                    case 3:
                        ViewRoomDetailsUser(username);
                        break;
                    case 4:
                        _logger.Log($"User logged out: {username}");
                        Console.WriteLine("Logged out.");
                        return; // Log out and return to main menu
                    default:
                        Console.WriteLine("Invalid option.");
                        break;
                }
            }
        }

        public void BookRoom(string username)
        {
            try
            {
                Console.Write("Enter Room Number to Book: ");
                string roomNumberInput = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(roomNumberInput) || !int.TryParse(roomNumberInput, out int roomNumber))
                {
                    Console.WriteLine("Invalid Room Number.");
                    return;
                }

                // Pass the Room Number and Username to the booking service
                _bookingService.BookRoom(roomNumber, username);
                _logger.Log($"Room Number {roomNumber} booked by User: {username}");
                Console.WriteLine("Room booked successfully.");
            }
            catch (Exception exception)
            {
                Console.WriteLine("Error: " + exception.Message);
                _logger.Log(exception, "Error booking room.");
            }
        }

        public void CancelBooking(string username)
        {
            try
            {
                int userId = _userService.GetUserId(username);

                // Check if the user has any active bookings
                DataTable activeBookings = _bookingService.GetActiveBookingsByUser(userId);
                if (activeBookings.Rows.Count == 0)
                {
                    Console.WriteLine("You have no active bookings to cancel.");
                    return;
                }

                Console.Write("Enter Room Number to Cancel Booking: ");
                string roomNumberInput = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(roomNumberInput) || !int.TryParse(roomNumberInput, out int roomNumber))
                {
                    Console.WriteLine("Invalid Room Number.");
                    return;
                }

                // Check if the user has an active booking for the provided room number
                bool hasActiveBooking = false;
                foreach (DataRow row in activeBookings.Rows)
                {
                    if ((int)row["RoomNumber"] == roomNumber)
                    {
                        hasActiveBooking = true;
                        break;
                    }
                }

                if (!hasActiveBooking)
                {
                    Console.WriteLine("You do not have a booking for this room.");
                    return;
                }

                _bookingService.CancelBooking(roomNumber, username);
                _logger.Log($"Booking for Room Number {roomNumber} cancelled by User: {username}");
                Console.WriteLine("Booking cancelled successfully.");
            }
            catch (Exception exception)
            {
                Console.WriteLine("Error: " + exception.Message);
                _logger.Log(exception, "Error cancelling booking.");
            }
        }

        public void ViewRoomDetailsUser(string username)
        {
            try
            {
                DataTable dt = _roomService.ViewRoomDetailsUser(username);
                if (dt.Rows.Count == 0)
                {
                    Console.WriteLine("No Rooms added.");
                }
                else
                {
                    int userId = _userService.GetUserId(username);
                    Console.WriteLine("===================================================================================================================");
                    Console.WriteLine("                                                         Room Details                                               ");
                    Console.WriteLine("===================================================================================================================");
                    Console.WriteLine("{0,-10} {1,-15} {2,-15} {3,-15} {4,-25} {5,-30}", "Room ID", "Room Number", "Room Type", "Price Per Day", "Availability", "Booked Date");
                    Console.WriteLine(new string('-', 122));
                    foreach (DataRow row in dt.Rows)
                    {
                        string availability;
                        if (row["UserID"] == DBNull.Value)
                        {
                            availability = "Available";
                        }
                        else if ((int)row["UserID"] == userId)
                        {
                            availability = "Booked by You";
                        }
                        else
                        {
                            availability = "Booked by Another User";
                        }

                        string bookedDate = row["BookedDate"] == DBNull.Value ? "N/A" : ((DateTime)row["BookedDate"]).ToString("yyyy-MM-dd HH:mm:ss");
                        Console.WriteLine("{0,-10} {1,-15} {2,-15} {3,-15} {4,-25} {5,-30}", row["RoomID"], row["RoomNumber"], row["RoomType"], row["PricePerDay"], availability, bookedDate);
                    }
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine("Error: " + exception.Message);
                _logger.Log(exception, "Error viewing room details for user.");
            }
        }
    }
}