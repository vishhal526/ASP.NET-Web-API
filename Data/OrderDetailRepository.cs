using System.Data;
using System.Data.SqlClient;
using WEB_API.Models;

namespace WEB_API.Data
{
    public class OrderDetailRepository
    {

        #region Configuration
        private readonly IConfiguration _connectionString;
        public OrderDetailRepository(IConfiguration configuration)
        {
            _connectionString = configuration;
        }
        #endregion

        #region GetAllOrderDetails
        // Retrieve all order details
        public List<OrderDetailModel> GetAllOrderDetails()
        {
            string connectionString = _connectionString.GetConnectionString("ConnectionString");
            var orderDetailList = new List<OrderDetailModel>();

            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();

                using (var objCmd = new SqlCommand("PR_OrderDetail_SelectAll", conn))
                {
                    objCmd.CommandType = CommandType.StoredProcedure;

                    using (var objSDR = objCmd.ExecuteReader())
                    {
                        while (objSDR.Read())
                        {
                            var orderDetail = new OrderDetailModel
                            {
                                OrderDetailID = Convert.ToInt32(objSDR["OrderDetailID"]),
                                OrderID = Convert.ToInt32(objSDR["OrderID"]),
                                ProductID = Convert.ToInt32(objSDR["ProductID"]),
                                ProductName = objSDR["ProductName"].ToString(),
                                Quantity = Convert.ToInt32(objSDR["Quantity"]),
                                Amount = Convert.ToDecimal(objSDR["Amount"]),
                                TotalAmount = Convert.ToDecimal(objSDR["TotalAmount"]),
                                UserID = Convert.ToInt32(objSDR["UserID"]),
                                UserName = objSDR["UserName"].ToString()
                            };

                            orderDetailList.Add(orderDetail);
                        }
                    }
                }
            }

            return orderDetailList;
        }

        #endregion
        
        #region Delete
        // Delete an order detail by ID
        public bool Delete(int orderDetailID)
        {
            string connectionString = _connectionString.GetConnectionString("ConnectionString");

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("PR_OrderDetail_DeleteByPK", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@OrderDetailID", orderDetailID);
                conn.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
                return rowsAffected > 0;
            }
        }

        #endregion
        
        #region Insert
        // Insert a new order detail
        public bool Insert(OrderDetailModel orderDetail)
        {
            string connectionString = _connectionString.GetConnectionString("ConnectionString");

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("PR_OrderDetail_Insert", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@OrderID", orderDetail.OrderID);
                cmd.Parameters.AddWithValue("@ProductID", orderDetail.ProductID);
                cmd.Parameters.AddWithValue("@Quantity", orderDetail.Quantity);
                cmd.Parameters.AddWithValue("@Amount", orderDetail.Amount);
                cmd.Parameters.AddWithValue("@TotalAmount", orderDetail.TotalAmount);
                cmd.Parameters.AddWithValue("@UserID", orderDetail.UserID);
                conn.Open();
                int rowsAffected = cmd.ExecuteNonQuery();

                return rowsAffected > 0;
            }
        }

        #endregion
        
        #region Update
        // Update an existing order detail
        public bool Update(OrderDetailModel orderDetail)
        {
            string connectionString = _connectionString.GetConnectionString("ConnectionString");

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("PR_OrderDetail_UpdateByPK", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@OrderDetailID", orderDetail.OrderDetailID);
                cmd.Parameters.AddWithValue("@OrderID", orderDetail.OrderID);
                cmd.Parameters.AddWithValue("@ProductID", orderDetail.ProductID);
                cmd.Parameters.AddWithValue("@Quantity", orderDetail.Quantity);
                cmd.Parameters.AddWithValue("@Amount", orderDetail.Amount);
                cmd.Parameters.AddWithValue("@TotalAmount", orderDetail.TotalAmount);
                cmd.Parameters.AddWithValue("@UserID", orderDetail.UserID);
                conn.Open();
                int rowsAffected = cmd.ExecuteNonQuery();

                return rowsAffected > 0;
            }
        }
        #endregion
        
        #region GetUsers

        public IEnumerable<OrderDetailUserDropDownModel> GetUsers()
        {
            var users = new List<OrderDetailUserDropDownModel>();
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
                    users.Add(new OrderDetailUserDropDownModel
                    {
                        UserID = Convert.ToInt32(reader["UserID"]),
                        UserName = reader["UserName"].ToString()
                    });
                }
            }

            return users;
        }
        #endregion
        
        #region GetOrders
        public IEnumerable<OrderDetailOrderDropDownModel> GetOrders()
        {
            var orders = new List<OrderDetailOrderDropDownModel>();
            string connectionString = _connectionString.GetConnectionString("ConnectionString");

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("PR_Order_DropDown", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    orders.Add(new OrderDetailOrderDropDownModel
                    {
                        OrderID = Convert.ToInt32(reader["OrderID"]),
                        OrderNumber = Convert.ToInt32(reader["OrderNumber"])
                    });
                }
            }

            return orders;
        }
        #endregion
        
        #region GetProducts
        public IEnumerable<OrderDetailProductDropDownModel> GetProducts()
        {
            var products = new List<OrderDetailProductDropDownModel>();
            string connectionString = _connectionString.GetConnectionString("ConnectionString");

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("PR_Product_DropDown", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    products.Add(new OrderDetailProductDropDownModel
                    {
                        ProductID = Convert.ToInt32(reader["ProductID"]),
                        ProductName = Convert.ToString(reader["ProductName"])
                    });
                }
            }

            return products;
        }
        #endregion

        #region GetOrderDetail
        public List<OrderDetailModel> GetOrderDetail(int OrderDetailID)
        {
            string connectionString = _connectionString.GetConnectionString("ConnectionString");
            var orderDetailList = new List<OrderDetailModel>();

            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();

                using (var objCmd = new SqlCommand("PR_OrderDetail_SelectByPK", conn)) // Assuming PR_OrderDetail_SelectByPK is the stored procedure
                {
                    objCmd.CommandType = CommandType.StoredProcedure;
                    objCmd.Parameters.AddWithValue("@OrderDetailID", OrderDetailID); // Adding OrderDetailID parameter to the stored procedure call

                    using (var objSDR = objCmd.ExecuteReader())
                    {
                        while (objSDR.Read())
                        {
                            var orderDetail = new OrderDetailModel
                            {
                                OrderDetailID = objSDR["OrderDetailID"] == DBNull.Value ? (int?)null : Convert.ToInt32(objSDR["OrderDetailID"]),
                                OrderID = Convert.ToInt32(objSDR["OrderID"]),
                                ProductID = Convert.ToInt32(objSDR["ProductID"]),
                                ProductName = objSDR["ProductName"].ToString(),
                                Quantity = Convert.ToInt32(objSDR["Quantity"]),
                                Amount = Convert.ToDecimal(objSDR["Amount"]),
                                TotalAmount = Convert.ToDecimal(objSDR["TotalAmount"]),
                                UserID = Convert.ToInt32(objSDR["UserID"]),
                                UserName = objSDR["UserName"].ToString()
                            };

                            orderDetailList.Add(orderDetail); // Adding the order detail object to the list
                        }
                    }
                }
            }

            return orderDetailList; // Returning the list of OrderDetailModel objects
        }
        #endregion

    }
}
