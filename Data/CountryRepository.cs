using Microsoft.Data.SqlClient;
using System.Data;
using System.Diagnostics.Metrics;
using WEB_API.Controllers;
using WEB_API.Models;

namespace WEB_API.Data
{
    public class CountryRepository
    {

        #region Configuration

        private readonly IConfiguration _connectionString;
        public CountryRepository(IConfiguration configuration)
        {
            _connectionString = configuration;
        }

        #endregion

        #region GetAllCountries
        public List<CountryModel> GetAllCountries()
        {
            var CountryList = new List<CountryModel>();

            String ConnectionString = _connectionString.GetConnectionString("ConnectionString");
            using (var conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                using (var cmd = new SqlCommand("PR_LOC_Country_SelectAll", conn))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    using (var sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            var country = new CountryModel()
                            {
                                CountryID = Convert.ToInt32(sdr["CountryID"]),
                                CountryName = sdr["CountryName"].ToString(),
                                CountryCode = sdr["CountryCode"].ToString(),
                                CreatedDate = Convert.ToDateTime(sdr["CreatedDate"]),
                                ModifiedDate = sdr["ModifiedDate"] != DBNull.Value ? Convert.ToDateTime(sdr["ModifiedDate"]) : DateTime.MinValue
                            };
                            CountryList.Add(country);
                        }
                    }
                }
            }


            return CountryList;
        }
        #endregion
        
        #region Delete
        public bool Delete(int countryID)
        {
            string connectionString = _connectionString.GetConnectionString("ConnectionString");

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("PR_LOC_Country_Delete", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@CountryID", countryID);
                conn.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
                return rowsAffected > 0;
            }
        }

        #endregion
        
        #region Insert
        public bool Insert(CountryModel country)
        {
            string connectionString = _connectionString.GetConnectionString("ConnectionString");

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("PR_LOC_Country_Insert", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@CountryName", country.CountryName);
                cmd.Parameters.AddWithValue("@CountryCode", country.CountryCode);

                conn.Open();
                int rowsAffected = cmd.ExecuteNonQuery(); // Execute the stored procedure

                // Return true if the insertion was successful
                return rowsAffected > 0;
            }
        }

        #endregion
        
        #region Update
        public bool Update(CountryModel country)
        {
            string connectionString = _connectionString.GetConnectionString("ConnectionString");

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("PR_LOC_Country_Update", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@CountryID", country.CountryID);
                cmd.Parameters.AddWithValue("@CountryName", country.CountryName);
                cmd.Parameters.AddWithValue("@CountryCode", country.CountryCode);

                conn.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
                return rowsAffected > 0;
            }
        }
        #endregion

        #region GetCountry
        public List<CountryModel> GetCountry(int CountryID)
        {
            string connectionString = _connectionString.GetConnectionString("ConnectionString");
            var countryList = new List<CountryModel>();

            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();

                using (var objCmd = new SqlCommand("PR_Country_SelectByID", conn)) // Assuming PR_Country_SelectByPK is the stored procedure
                {
                    objCmd.CommandType = CommandType.StoredProcedure;
                    objCmd.Parameters.AddWithValue("@CountryID", CountryID); // Adding CountryID parameter to the stored procedure call

                    using (var objSDR = objCmd.ExecuteReader())
                    {
                        while (objSDR.Read())
                        {
                            var country = new CountryModel
                            {
                                CountryID = objSDR["CountryID"] == DBNull.Value ? (int?)null : Convert.ToInt32(objSDR["CountryID"]),
                                CountryName = objSDR["CountryName"].ToString(),
                                CountryCode = objSDR["CountryCode"].ToString(),
                                CreatedDate = Convert.ToDateTime(objSDR["CreatedDate"]),
                                ModifiedDate = objSDR["ModifiedDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(objSDR["ModifiedDate"])
                            };

                            countryList.Add(country); // Adding the country object to the list
                        }
                    }
                }
            }

            return countryList; // Returning the list of CountryModel objects
        }
        #endregion

    }
}
