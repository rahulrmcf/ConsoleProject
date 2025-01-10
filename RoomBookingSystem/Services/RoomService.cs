using System;
using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RoomBookingSystem.Models;

namespace RoomBookingSystem.Services
{
    public class RoomService
    {
        private readonly string connectionString;
        private readonly ILogger<RoomService> _logger;

        public RoomService(IConfiguration configuration, ILogger<RoomService> logger)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection") ??
                               throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            _logger = logger;
        }

        public void AddRoom(Room room)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("sp_AddRoom", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@RoomNumber", room.RoomNumber);
                        cmd.Parameters.AddWithValue("@RoomType", room.RoomType);
                        cmd.Parameters.AddWithValue("@PricePerDay", room.PricePerDay);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Error adding room");
                throw;
            }
        }

        public void DeleteRoom(int roomNumber)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("sp_DeleteRoom", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@RoomNumber", roomNumber);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Error deleting room");
                throw;
            }
        }

        public DataTable ViewRoomDetailsAdmin()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("sp_ViewRoomDetailsAdmin", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
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
                _logger.LogError(exception, "Error viewing room details for admin");
                throw;
            }
        }

        public DataTable ViewRoomDetailsUser(string username)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("sp_ViewRoomDetailsUser", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Username", username);
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
                _logger.LogError(exception, "Error viewing room details for user");
                throw;
            }
        }

        public bool CheckRoomExists(int roomNumber)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("sp_CheckRoomExists", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@RoomNumber", roomNumber);
                        var result = cmd.ExecuteScalar();
                        return (int)result > 0;
                    }
                }
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Error checking if room exists");
                throw;
            }
        }
    }
}