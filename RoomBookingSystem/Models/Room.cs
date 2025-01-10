namespace RoomBookingSystem.Models
{
    public class Room
    {
        public int RoomID { get; set; }           // Primary Key
        public int RoomNumber { get; set; }       // Unique Room Number
        public string RoomType { get; set; }      // Type of Room (e.g., Single, Double)
        public decimal PricePerDay { get; set; }  // Price Per Day
        public bool IsAvailable { get; set; }     // Availability Status
    }
}