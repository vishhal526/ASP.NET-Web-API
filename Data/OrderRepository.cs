using System.Data;
using System.Data.SqlClient;
using WEB_API.Models;

namespace WEB_API.Data
{
    public class OrderRepository
    {

        #region Configuration
        private readonly IConfiguration _connectionString;
        public OrderRepository(IConfiguration configuration)
        {
            _connectionString = configuration;
        }
        #endregion

        #region GetAllOrders
        // Retrieve all orders
        public List<OrderModel> GetAllOrders()
        {
            string connectionString = _connectionString.GetConnectionString("ConnectionString");
            var orderList = new List<OrderModel>();

            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();

                using (var objCmd = new SqlCommand("PR_Order_SelectAll", conn))
                {
                    objCmd.CommandType = CommandType.StoredProcedure;

                    using (var objSDR = objCmd.ExecuteReader())
                    {
                        while (objSDR.Read())
                        {
                            var order = new OrderModel
                            {
                                OrderID = Convert.ToInt32(objSDR["OrderID"]),
                                OrderDate = Convert.ToDateTime(objSDR["OrderDate"]),
                                CustomerID = Convert.ToInt32(objSDR["CustomerID"]),
                                CustomerName = objSDR["CustomerName"].ToString(),
                                PaymentMode = objSDR["PaymentMode"].ToString(),
                                TotalAmount = objSDR["TotalAmount"] != DBNull.Value ? Convert.ToDecimal(objSDR["TotalAmount"]) : (decimal?)null,
                                ShippingAddress = objSDR["ShippingAddress"].ToString(),
                                UserID = Convert.ToInt32(objSDR["UserID"]),
                                UserName = objSDR["UserName"].ToString(),
                            };

                            orderList.Add(order);
                        }
                    }
                }
            }

            return orderList;
        }
        #endregion

        #region Delete
        // Delete an order by ID
        public bool Delete(int orderID)
        {
            string connectionString = _connectionString.GetConnectionString("ConnectionString");

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("PR_Order_DeleteByPK", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@OrderID", orderID);
                conn.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
                return rowsAffected > 0;
            }
        }
        #endregion

        #region Insert
        // Insert a new order
        public bool Insert(OrderModel order)
        {
            string connectionString = _connectionString.GetConnectionString("ConnectionString");

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("PR_Order_Insert", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@OrderDate", order.OrderDate);
                cmd.Parameters.AddWithValue("@CustomerID", order.CustomerID);
                cmd.Parameters.AddWithValue("@PaymentMode", order.PaymentMode);
                cmd.Parameters.AddWithValue("@TotalAmount", (object?)order.TotalAmount ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@ShippingAddress", order.ShippingAddress);
                cmd.Parameters.AddWithValue("@UserID", order.UserID);
                conn.Open();
                int rowsAffected = cmd.ExecuteNonQuery();

                return rowsAffected > 0;
            }
        }
        #endregion

        #region Update
        // Update an existing order
        public bool Update(OrderModel order)
        {
            string connectionString = _connectionString.GetConnectionString("ConnectionString");

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("PR_Order_UpdateByPK", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@OrderID", order.OrderID);
                cmd.Parameters.AddWithValue("@OrderDate", order.OrderDate);
                cmd.Parameters.AddWithValue("@CustomerID", order.CustomerID);
                cmd.Parameters.AddWithValue("@PaymentMode", order.PaymentMode);
                cmd.Parameters.AddWithValue("@TotalAmount", (object?)order.TotalAmount ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@ShippingAddress", order.ShippingAddress);
                cmd.Parameters.AddWithValue("@UserID", order.UserID);
                conn.Open();
                int rowsAffected = cmd.ExecuteNonQuery();

                return rowsAffected > 0;
            }
        }
        #endregion
       
        #region GetUsers
        public IEnumerable<OrderUserDropDownModel> GetUsers()
        {
            var users = new List<OrderUserDropDownModel>();
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
                    users.Add(new OrderUserDropDownModel
                    {
                        UserID = Convert.ToInt32(reader["UserID"]),
                        UserName = reader["UserName"].ToString()
                    });
                }
            }

            return users;
        }
        #endregion
        
        #region GetCustomers
        public IEnumerable<OrderCustomerDropDownModel> GetCustomers()
        {
            var customers = new List<OrderCustomerDropDownModel>();
            string connectionString = _connectionString.GetConnectionString("ConnectionString");

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("PR_Customer_DropDown", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    customers.Add(new OrderCustomerDropDownModel
                    {
                        CustomerID = Convert.ToInt32(reader["CustomerID"]),
                        CustomerName = reader["CustomerName"].ToString()
                    });
                }
            }

            return customers;
        }
        #endregion

        #region GetOrder
        public List<OrderModel> GetOrder(int OrderID)
        {
            string connectionString = _connectionString.GetConnectionString("ConnectionString");
            var orderList = new List<OrderModel>();

            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();

                using (var objCmd = new SqlCommand("PR_Order_SelectByPK", conn)) // Assuming PR_Order_SelectByPK is the stored procedure
                {
                    objCmd.CommandType = CommandType.StoredProcedure;
                    objCmd.Parameters.AddWithValue("@OrderID", OrderID); // Adding OrderID parameter to the stored procedure call

                    using (var objSDR = objCmd.ExecuteReader())
                    {
                        while (objSDR.Read())
                        {
                            var order = new OrderModel
                            {
                                OrderID = objSDR["OrderID"] == DBNull.Value ? (int?)null : Convert.ToInt32(objSDR["OrderID"]),
                                OrderDate = Convert.ToDateTime(objSDR["OrderDate"]),
                                CustomerID = Convert.ToInt32(objSDR["CustomerID"]),
                                CustomerName = objSDR["CustomerName"].ToString(),
                                PaymentMode = objSDR["PaymentMode"].ToString(),
                                TotalAmount = objSDR["TotalAmount"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(objSDR["TotalAmount"]),
                                ShippingAddress = objSDR["ShippingAddress"].ToString(),
                                UserID = Convert.ToInt32(objSDR["UserID"]),
                                UserName = objSDR["UserName"].ToString(),
                            };

                            orderList.Add(order); // Adding the order object to the list
                        }
                    }
                }
            }

            return orderList; // Returning the list of OrderModel objects
        }
        #endregion

    }
}
