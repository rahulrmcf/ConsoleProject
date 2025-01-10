using System;
using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace RoomBookingSystem
{
    public class UserService
    {
        private readonly string connectionString;
        private readonly ILogger<UserService> _logger;

        public UserService(IConfiguration configuration, ILogger<UserService> logger)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection") ??
                               throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            _logger = logger;
        }

        public bool ValidateUser(string username, string password, bool isAdmin)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("sp_ValidateUser", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Username", username);
                        cmd.Parameters.AddWithValue("@Password", password);
                        cmd.Parameters.AddWithValue("@IsAdmin", isAdmin);
                        var result = cmd.ExecuteScalar();
                        return (int)result == 1;
                    }
                }
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Error validating user");
                throw;
            }
        }

        public bool CheckUserExists(string username)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("sp_CheckUserExists", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Username", username);
                        var result = cmd.ExecuteScalar();
                        return (int)result == 1;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking if user exists");
                throw;
            }
        }

        public void RegisterUser(string username, string password)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("sp_RegisterUser", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Username", username);
                        cmd.Parameters.AddWithValue("@Password", password);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Error registering user");
                throw;
            }
        }

        public int GetUserId(string username)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("sp_GetUserId", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Username", username);
                        var result = cmd.ExecuteScalar();
                        return result != null ? (int)result : throw new Exception("User not found");
                    }
                }
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Error getting user ID by username");
                throw;
            }
        }

        public string GetUsernameById(int userId)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("sp_GetUsernameById", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@UserId", userId);
                        var result = cmd.ExecuteScalar();
                        return result != null ? result.ToString() : "Unknown";
                    }
                }
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Error getting username by user ID");
                throw;
            }
        }
    }
}