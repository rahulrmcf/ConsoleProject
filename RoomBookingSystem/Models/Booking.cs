namespace RoomBookingSystem.Models
{
    public class Booking
    {
        public int BookingID { get; set; }        // Primary Key
        public int RoomID { get; set; }           // Foreign Key referencing RoomID
        public int UserID { get; set; }           // Foreign Key referencing UserID
        public DateTime BookedDate { get; set; }  // Date when the room was booked
        public string Status { get; set; }        // Booking status (e.g., 'Booked', 'Cancelled')
    }
}