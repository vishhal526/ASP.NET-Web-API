using Microsoft.SqlServer.Server;
using System.Data;
using System.Data.SqlClient;
using WEB_API.Models;

namespace WEB_API.Data
{
    public class BillRepository
    {

        #region Configuration
        private readonly IConfiguration _connectionString;

        public BillRepository(IConfiguration configuration)
        {
            _connectionString = configuration;
        }
        #endregion

        #region GetAllBills
        public List<BillModel> GetAllBills()
        {
            string connectionString = _connectionString.GetConnectionString("ConnectionString");
            var billList = new List<BillModel>();

            // Retrieve the connection string from the configuration

            // Use a using block to manage the SqlConnection
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();

                // Prepare the SqlCommand
                using (var objCmd = new SqlCommand("PR_Bill_SelectAll", conn))
                {
                    objCmd.CommandType = CommandType.StoredProcedure;

                    // Execute the command and process the SqlDataReader
                    using (var objSDR = objCmd.ExecuteReader())
                    {
                        while (objSDR.Read())
                        {
                            // Populate the BillModel object and add it to the list
                            var bill = new BillModel
                            {
                                BillID = Convert.ToInt32(objSDR["BillID"]),
                                OrderID = Convert.ToInt32(objSDR["OrderID"]),
                                //OrderNumber = Convert.ToInt32(objSDR["OrderNumber"]),
                                UserID = Convert.ToInt32(objSDR["UserID"]),
                                UserName = objSDR["UserName"].ToString(),
                                BillNumber = objSDR["BillNumber"].ToString(),
                                BillDate = Convert.ToDateTime(objSDR["BillDate"]),
                                TotalAmount = Convert.ToDecimal(objSDR["TotalAmount"]),
                                Discount = objSDR["Discount"] != DBNull.Value ? Convert.ToDecimal(objSDR["Discount"]) : Decimal.MinValue,
                                NetAmount = Convert.ToDecimal(objSDR["NetAmount"]),
                                // ModifiedDate = objSDR["ModifiedDate"] != DBNull.Value ? Convert.ToDateTime(objSDR["ModifiedDate"]) : DateTime.MinValue
                            };

                            billList.Add(bill);
                        }
                    }
                }
            }

            return billList;
        }
        #endregion

        #region Delete
        public bool Delete(int billID)
        {
            string connectionString = _connectionString.GetConnectionString("ConnectionString");

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("PR_Bill_DeleteByPK", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@billID", billID);
                conn.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
                return rowsAffected > 0;
            }
        }
        #endregion

        #region Insert

        public bool Insert(BillModel bill)
        {
            string connectionString = _connectionString.GetConnectionString("ConnectionString");

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("PR_Bill_Insert", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@OrderID", bill.OrderID);
                cmd.Parameters.AddWithValue("@UserID", bill.UserID);
                cmd.Parameters.AddWithValue("@BillNumber", bill.BillNumber);
                cmd.Parameters.AddWithValue("@BillDate", bill.BillDate);
                cmd.Parameters.AddWithValue("@TotalAmount", bill.TotalAmount);
                cmd.Parameters.AddWithValue("@Discount",(object?)bill.Discount?? DBNull.Value);
                cmd.Parameters.AddWithValue("@NetAmount", bill.NetAmount);
                conn.Open();
                int rowsAffected = cmd.ExecuteNonQuery(); // Execute the stored procedure

                // Return true if the insertion was successful
                return rowsAffected > 0;
            }
        }
        #endregion

        #region Update

        public bool Update(BillModel bill)
        {
            string connectionString = _connectionString.GetConnectionString("ConnectionString");

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("PR_Bill_UpdateByPK", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@BillID", bill.BillID);
                cmd.Parameters.AddWithValue("@OrderID", bill.OrderID);
                cmd.Parameters.AddWithValue("@UserID", bill.UserID);
                cmd.Parameters.AddWithValue("@BillNumber", bill.BillNumber);
                cmd.Parameters.AddWithValue("@BillDate", bill.BillDate);
                cmd.Parameters.AddWithValue("@TotalAmount", bill.TotalAmount);
                cmd.Parameters.AddWithValue("@Discount", (object?)bill.Discount ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@NetAmount", bill.NetAmount);
                conn.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
                return rowsAffected > 0;
            }
        }
        #endregion

        #region GetUsers

        public IEnumerable<BillUserDropDownModel> GetUsers()
        {
            var users = new List<BillUserDropDownModel>();
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
                    users.Add(new BillUserDropDownModel
                    {
                        UserID = Convert.ToInt32(reader["UserID"]),
                        UserName= reader["UserName"].ToString()
                    });
                }
            }

            return users;
        }
        #endregion

        #region GetOrders

        public IEnumerable<BillOrderDropDownModel> GetOrders()
        {
            var orders = new List<BillOrderDropDownModel>();
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
                    orders.Add(new BillOrderDropDownModel
                    {
                        OrderID = Convert.ToInt32(reader["OrderID"]),
                        OrderNumber = Convert.ToInt32(reader["OrderNumber"])
                    });
                }
            }

            return orders;
        }
        #endregion

        #region GetBill
        public List<BillModel> GetBill(int BillID)
        {
            string connectionString = _connectionString.GetConnectionString("ConnectionString");
            var billList = new List<BillModel>();

            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();

                using (var objCmd = new SqlCommand("PR_Bill_SelectByPK", conn)) // Assuming PR_Bill_SelectByPK is the stored procedure
                {
                    objCmd.CommandType = CommandType.StoredProcedure;
                    objCmd.Parameters.AddWithValue("@BillID", BillID); // Adding BillID parameter to the stored procedure call

                    using (var objSDR = objCmd.ExecuteReader())
                    {
                        while (objSDR.Read())
                        {
                            var bill = new BillModel
                            {
                                BillID = objSDR["BillID"] == DBNull.Value ? (int?)null : Convert.ToInt32(objSDR["BillID"]),
                                BillNumber = objSDR["BillNumber"].ToString(),
                                BillDate = Convert.ToDateTime(objSDR["BillDate"]),
                                OrderID = Convert.ToInt32(objSDR["OrderID"]),
                                TotalAmount = Convert.ToDecimal(objSDR["TotalAmount"]),
                                Discount = objSDR["Discount"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(objSDR["Discount"]),
                                NetAmount = Convert.ToDecimal(objSDR["NetAmount"]),
                                UserID = Convert.ToInt32(objSDR["UserID"]),
                                UserName = objSDR["UserName"].ToString()
                            };

                            billList.Add(bill); // Adding the bill object to the list
                        }
                    }
                }
            }

            return billList; // Returning the list of BillModel objects
        }
        #endregion

    }
}
