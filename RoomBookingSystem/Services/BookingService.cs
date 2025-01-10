using System;
using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace RoomBookingSystem.Services
{
    public class BookingService
    {
        private readonly string connectionString;
        private readonly ILogger<BookingService> _logger;

        public BookingService(IConfiguration configuration, ILogger<BookingService> logger)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection") ??
                               throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            _logger = logger;
        }

        public void BookRoom(int roomNumber, string username)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("sp_BookRoom", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@RoomNumber", roomNumber);
                        cmd.Parameters.AddWithValue("@Username", username);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Error booking room");
                throw;
            }
        }

        public void CancelBooking(int roomNumber, string username)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("sp_CancelBooking", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@RoomNumber", roomNumber);
                        cmd.Parameters.AddWithValue("@Username", username);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Error cancelling booking");
                throw;
            }
        }

        public DataTable GetActiveBookingsByUser(int userId)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("sp_GetActiveBookingsByUser", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@UserId", userId);
                        using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);
                            return dt;
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Error getting active bookings by user");
                throw;
            }
        }
    }
}