using System.Data;
using System.Data.SqlClient;
using WEB_API.Models;

namespace WEB_API.Data
{
    public class ProductRepository
    {

        #region Configuration
        private readonly IConfiguration _connectionString;
        public ProductRepository(IConfiguration configuration)
        {
            _connectionString = configuration;
        }
        #endregion

        #region GetAllProducts
        // Retrieve all products
        public List<ProductModel> GetAllProducts()
        {
            string connectionString = _connectionString.GetConnectionString("ConnectionString");
            var productList = new List<ProductModel>();

            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();

                using (var objCmd = new SqlCommand("PR_Product_SelectAll", conn))
                {
                    objCmd.CommandType = CommandType.StoredProcedure;

                    using (var objSDR = objCmd.ExecuteReader())
                    {
                        while (objSDR.Read())
                        {
                            var product = new ProductModel
                            {
                                ProductID = Convert.ToInt32(objSDR["ProductID"]),
                                ProductName = objSDR["ProductName"].ToString(),
                                ProductPrice = Convert.ToDecimal(objSDR["ProductPrice"]),
                                ProductCode = objSDR["ProductCode"].ToString(),
                                Description = objSDR["Description"].ToString(),
                                UserID = Convert.ToInt32(objSDR["UserID"]),
                                UserName = objSDR["UserName"].ToString()
                            };

                            productList.Add(product);
                        }
                    }
                }
            }

            return productList;
        }
        #endregion

        #region Delete
        // Delete a product by ID
        public bool Delete(int productID)
        {
            string connectionString = _connectionString.GetConnectionString("ConnectionString");

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("PR_Product_DeleteByPK", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@ProductID", productID);
                conn.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
                return rowsAffected > 0;
            }
        }
        #endregion

        #region Insert
        // Insert a new product
        public bool Insert(ProductModel product)
        {
            string connectionString = _connectionString.GetConnectionString("ConnectionString");

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("PR_Product_Insert", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@ProductName", product.ProductName);
                cmd.Parameters.AddWithValue("@ProductPrice", product.ProductPrice);
                cmd.Parameters.AddWithValue("@ProductCode", product.ProductCode);
                cmd.Parameters.AddWithValue("@Description", product.Description);
                cmd.Parameters.AddWithValue("@UserID", product.UserID);
                conn.Open();
                int rowsAffected = cmd.ExecuteNonQuery();

                return rowsAffected > 0;
            }
        }
        #endregion

        #region Update
        // Update an existing product
        public bool Update(ProductModel product)
        {
            string connectionString = _connectionString.GetConnectionString("ConnectionString");

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("PR_Product_UpdateByPK", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@ProductID", product.ProductID);
                cmd.Parameters.AddWithValue("@ProductName", product.ProductName);
                cmd.Parameters.AddWithValue("@ProductPrice", product.ProductPrice);
                cmd.Parameters.AddWithValue("@ProductCode", product.ProductCode);
                cmd.Parameters.AddWithValue("@Description", product.Description);
                cmd.Parameters.AddWithValue("@UserID", product.UserID);
                conn.Open();
                int rowsAffected = cmd.ExecuteNonQuery();

                return rowsAffected > 0;
            }
        }
        #endregion
       
        #region GetUsers
        public IEnumerable<ProductUserDropDownModel> GetUsers()
        {
            var users = new List<ProductUserDropDownModel>();
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
                    users.Add(new ProductUserDropDownModel
                    {
                        UserID = Convert.ToInt32(reader["UserID"]),
                        UserName = reader["UserName"].ToString()
                    });
                }
            }

            return users;
        }
        #endregion

        #region GetProduct
        public List<ProductModel> GetProduct(int ProductID)
        {
            string connectionString = _connectionString.GetConnectionString("ConnectionString");
            var productList = new List<ProductModel>();

            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();

                using (var objCmd = new SqlCommand("PR_Product_SelectByPK", conn)) // Assuming PR_Product_SelectByPK is the stored procedure
                {
                    objCmd.CommandType = CommandType.StoredProcedure;
                    objCmd.Parameters.AddWithValue("@ProductID", ProductID); // Adding ProductID parameter to the stored procedure call

                    using (var objSDR = objCmd.ExecuteReader())
                    {
                        while (objSDR.Read())
                        {
                            var product = new ProductModel
                            {
                                ProductID = objSDR["ProductID"] == DBNull.Value ? (int?)null : Convert.ToInt32(objSDR["ProductID"]),
                                ProductName = objSDR["ProductName"].ToString(),
                                ProductPrice = Convert.ToDecimal(objSDR["ProductPrice"]),
                                ProductCode = objSDR["ProductCode"].ToString(),
                                Description = objSDR["Description"].ToString(),
                                UserID = Convert.ToInt32(objSDR["UserID"]),
                                UserName = objSDR["UserName"].ToString()
                            };

                            productList.Add(product); // Adding the product object to the list
                        }
                    }
                }
            }

            return productList; // Returning the list of ProductModel objects
        }
        #endregion

    }
}
