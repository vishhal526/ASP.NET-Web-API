using System.Data;
using System.Data.SqlClient;
using WEB_API.Models;

namespace WEB_API.Data
{
    public class CustomerRepository
    {

        #region Configuration
        private readonly IConfiguration _connectionString;
        public CustomerRepository(IConfiguration configuration)
        {
            _connectionString = configuration;
        }
        #endregion

        #region GetAllCustomers
        // Retrieve all customers
        public List<CustomerModel> GetAllCustomers()
        {
            string connectionString = _connectionString.GetConnectionString("ConnectionString");
            var customerList = new List<CustomerModel>();

            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();

                using (var objCmd = new SqlCommand("PR_Customer_SelectAll", conn))
                {
                    objCmd.CommandType = CommandType.StoredProcedure;

                    using (var objSDR = objCmd.ExecuteReader())
                    {
                        while (objSDR.Read())
                        {
                            var customer = new CustomerModel
                            {
                                CustomerID = Convert.ToInt32(objSDR["CustomerID"]),
                                CustomerName = objSDR["CustomerName"].ToString(),
                                HomeAddress = objSDR["HomeAddress"].ToString(),
                                Email = objSDR["Email"].ToString(),
                                MobileNo = objSDR["MobileNo"].ToString(),
                                GSTNO = objSDR["GSTNO"].ToString(),
                                CityName = objSDR["CityName"].ToString(),
                                PinCode = objSDR["PinCode"].ToString(),
                                NetAmount = Convert.ToDecimal(objSDR["NetAmount"]),
                                UserID = Convert.ToInt32(objSDR["UserID"]),
                                UserName = objSDR["UserName"].ToString()
                            };

                            customerList.Add(customer);
                        }
                    }
                }
            }

            return customerList;
        }

        #endregion
       
        #region Delete
        // Delete a customer by ID
        public bool Delete(int customerID)
        {
            string connectionString = _connectionString.GetConnectionString("ConnectionString");

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("PR_Customer_DeleteByPK", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@CustomerID", customerID);
                conn.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
                return rowsAffected > 0;
            }
        }

        #endregion
        
        #region Insert
        // Insert a new customer
        public bool Insert(CustomerModel customer)
        {
            string connectionString = _connectionString.GetConnectionString("ConnectionString");

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("PR_Customer_Insert", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@CustomerName", customer.CustomerName);
                cmd.Parameters.AddWithValue("@HomeAddress", customer.HomeAddress);
                cmd.Parameters.AddWithValue("@Email", customer.Email);
                cmd.Parameters.AddWithValue("@MobileNo", customer.MobileNo);
                cmd.Parameters.AddWithValue("@GSTNO", customer.GSTNO);
                cmd.Parameters.AddWithValue("@CityName", customer.CityName);
                cmd.Parameters.AddWithValue("@PinCode", customer.PinCode);
                cmd.Parameters.AddWithValue("@NetAmount", customer.NetAmount);
                cmd.Parameters.AddWithValue("@UserID", customer.UserID);
                conn.Open();
                int rowsAffected = cmd.ExecuteNonQuery();

                return rowsAffected > 0;
            }
        }

        #endregion
        
        #region Update
        // Update an existing customer
        public bool Update(CustomerModel customer)
        {
            string connectionString = _connectionString.GetConnectionString("ConnectionString");

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("PR_Customer_UpdateByPK", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@CustomerID", customer.CustomerID);
                cmd.Parameters.AddWithValue("@CustomerName", customer.CustomerName);
                cmd.Parameters.AddWithValue("@HomeAddress", customer.HomeAddress);
                cmd.Parameters.AddWithValue("@Email", customer.Email);
                cmd.Parameters.AddWithValue("@MobileNo", customer.MobileNo);
                cmd.Parameters.AddWithValue("@GSTNO", customer.GSTNO);
                cmd.Parameters.AddWithValue("@CityName", customer.CityName);
                cmd.Parameters.AddWithValue("@PinCode", customer.PinCode);
                cmd.Parameters.AddWithValue("@NetAmount", customer.NetAmount);
                cmd.Parameters.AddWithValue("@UserID", customer.UserID);
                conn.Open();
                int rowsAffected = cmd.ExecuteNonQuery();

                return rowsAffected > 0;
            }
        }
        #endregion
        
        #region GetUsers

        public IEnumerable<CustomerUserDropDownModel> GetUsers()
        {
            var users = new List<CustomerUserDropDownModel>();
            string connectionString = _connectionString.GetConnectionString("ConnectionString");

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("PR_User_DropDown", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    users.Add(new CustomerUserDropDownModel
                    {
                        UserID = Convert.ToInt32(reader["UserID"]),
                        UserName = reader["UserName"].ToString()
                    });
                }
            }

            return users;
        }
        #endregion

        #region GetCustomer
        public List<CustomerModel> GetCustomer(int CustomerID)
        {
            string connectionString = _connectionString.GetConnectionString("ConnectionString");
            var customerList = new List<CustomerModel>();

            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();

                using (var objCmd = new SqlCommand("PR_Customer_SelectByPK", conn)) // Assuming PR_Customer_SelectByPK is the stored procedure
                {
                    objCmd.CommandType = CommandType.StoredProcedure;
                    objCmd.Parameters.AddWithValue("@CustomerID", CustomerID); // Adding CustomerID parameter to the stored procedure call

                    using (var objSDR = objCmd.ExecuteReader())
                    {
                        while (objSDR.Read())
                        {
                            var customer = new CustomerModel
                            {
                                CustomerID = objSDR["CustomerID"] == DBNull.Value ? (int?)null : Convert.ToInt32(objSDR["CustomerID"]),
                                CustomerName = objSDR["CustomerName"].ToString(),
                                HomeAddress = objSDR["HomeAddress"].ToString(),
                                Email = objSDR["Email"].ToString(),
                                MobileNo = objSDR["MobileNo"].ToString(),
                                GSTNO = objSDR["GSTNO"].ToString(),
                                CityName = objSDR["CityName"].ToString(),
                                PinCode = objSDR["PinCode"].ToString(),
                                NetAmount = Convert.ToDecimal(objSDR["NetAmount"]),
                                UserID = Convert.ToInt32(objSDR["UserID"]),
                                UserName = objSDR["UserName"].ToString()
                            };

                            customerList.Add(customer); // Adding the customer object to the list
                        }
                    }
                }
            }

            return customerList; // Returning the list of CustomerModel objects
        }
        #endregion

    }
}
