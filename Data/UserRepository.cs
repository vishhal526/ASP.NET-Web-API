//using Microsoft.CodeAnalysis.Elfie.Diagnostics;
using System.Data;
using System.Data.SqlClient;
using WEB_API.Models;

namespace WEB_API.Data
{
    public class UserRepository
    {

        #region Configuration
        private readonly IConfiguration _connectionString;
        public UserRepository(IConfiguration configuration)
        {
            _connectionString = configuration;
        }
        #endregion
        
        #region GetAllUsers

        // Retrieve all users
        public List<UserModel> GetAllUsers()
        {
            string connectionString = _connectionString.GetConnectionString("ConnectionString");
            var userList = new List<UserModel>();

            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();

                using (var objCmd = new SqlCommand("PR_User_SelectAll", conn))
                {
                    objCmd.CommandType = CommandType.StoredProcedure;

                    using (var objSDR = objCmd.ExecuteReader())
                    {
                        while (objSDR.Read())
                        {
                            var user = new UserModel
                            {
                                UserID = Convert.ToInt32(objSDR["UserID"]),
                                UserName = objSDR["UserName"].ToString(),
                                Email = objSDR["Email"].ToString(),
                                Password = objSDR["Password"].ToString(),
                                MobileNo = objSDR["MobileNo"].ToString(),
                                Address = objSDR["Address"].ToString(),
                                IsActive = Convert.ToBoolean(objSDR["IsActive"])
                            };

                            userList.Add(user);
                        }
                    }
                }
            }

            return userList;
        }
        #endregion
        
        #region Delete

        // Delete a user by ID
        public bool Delete(int userID)
        {
            string connectionString = _connectionString.GetConnectionString("ConnectionString");

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("PR_User_DeleteByPK", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@UserID", userID);
                conn.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
                return rowsAffected > 0;
            }
        }
        #endregion
        
        #region Insert

        // Insert a new user
        public bool Insert(UserModel user)
        {
            string connectionString = _connectionString.GetConnectionString("ConnectionString");

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("PR_User_Insert", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@UserName", user.UserName);
                cmd.Parameters.AddWithValue("@Email", user.Email);
                cmd.Parameters.AddWithValue("@Password", user.Password);
                cmd.Parameters.AddWithValue("@MobileNo", user.MobileNo);
                cmd.Parameters.AddWithValue("@Address", user.Address);
                cmd.Parameters.AddWithValue("@IsActive", user.IsActive);
                conn.Open();
                int rowsAffected = cmd.ExecuteNonQuery();

                return rowsAffected > 0;
            }
        }
        #endregion
        
        #region Update

        // Update an existing user
        public bool Update(UserModel user)
        {
            string connectionString = _connectionString.GetConnectionString("ConnectionString");

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("PR_User_UpdateByPK", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@UserID", user.UserID);
                cmd.Parameters.AddWithValue("@UserName", user.UserName);
                cmd.Parameters.AddWithValue("@Email", user.Email);
                cmd.Parameters.AddWithValue("@Password", user.Password);
                cmd.Parameters.AddWithValue("@MobileNo", user.MobileNo);
                cmd.Parameters.AddWithValue("@Address", user.Address);
                cmd.Parameters.AddWithValue("@IsActive", user.IsActive);
                conn.Open();
                int rowsAffected = cmd.ExecuteNonQuery();

                return rowsAffected > 0;
            }
        }
        #endregion

        #region GetUser
        public List<UserModel> GetUser(int UserID)
        {
            string connectionString = _connectionString.GetConnectionString("ConnectionString");
            var userList = new List<UserModel>();

            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();

                using (var objCmd = new SqlCommand("PR_User_SelectByPK", conn))
                {
                    objCmd.CommandType = CommandType.StoredProcedure;
                    objCmd.Parameters.AddWithValue("@UserID", UserID);
                    using (var objSDR = objCmd.ExecuteReader())
                    {
                        while (objSDR.Read())
                        {
                            var u = new UserModel
                            {
                                UserID = Convert.ToInt32(objSDR["UserID"]),
                                UserName = objSDR["UserName"].ToString(),
                                Email = objSDR["Email"].ToString(),
                                Password = objSDR["Password"].ToString(),
                                MobileNo = objSDR["MobileNo"].ToString(),
                                Address = objSDR["Address"].ToString(),
                                IsActive = Convert.ToBoolean(objSDR["IsActive"])
                            };

                            userList.Add(u); // Fixed: Adding 'u' to the list
                        }
                    }
                }
            }

            return userList; // Fixed: Returning 'userList'
        }

        #endregion
    }

}
