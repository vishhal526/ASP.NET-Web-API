using System.Data;
using System.Data.SqlClient;
using WEB_API.Models;

namespace WEB_API.Data
{
    public class CityRepository
    {

        #region Configuration
        private readonly IConfiguration _connectionString;

        public CityRepository(IConfiguration configuration)
        {
            _connectionString = configuration;
        }
        #endregion

        #region GetAllCities

        public List<CityModel> GetAllCities()
        {
            string connectionString = _connectionString.GetConnectionString("ConnectionString");
            var cityList = new List<CityModel>();

            // Retrieve the connection string from the configuration

            // Use a using block to manage the SqlConnection
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();

                // Prepare the SqlCommand
                using (var objCmd = new SqlCommand("PR_LOC_City_SelectAll", conn))
                {
                    objCmd.CommandType = CommandType.StoredProcedure;

                    // Execute the command and process the SqlDataReader
                    using (var objSDR = objCmd.ExecuteReader())
                    {
                        while (objSDR.Read())
                        {
                            // Populate the CityModel object and add it to the list
                            var city = new CityModel
                            {
                                CityID = Convert.ToInt32(objSDR["CityID"]),          
                                CountryID = Convert.ToInt32(objSDR["CountryID"]),    
                                StateID = Convert.ToInt32(objSDR["StateID"]),        
                                CityName = objSDR["CityName"].ToString(),            
                                CountryName = objSDR["CountryName"].ToString(),            
                                StateName = objSDR["StateName"].ToString(),
                                CityCode = objSDR["CityCode"].ToString(),            
                                CreatedDate = Convert.ToDateTime(objSDR["CreatedDate"]),
                                //ModifiedDate = Convert.ToDateTime(objSDR["ModifiedDate"]),
                                ModifiedDate = objSDR["ModifiedDate"] != DBNull.Value ? Convert.ToDateTime(objSDR["ModifiedDate"]) : DateTime.MinValue
                            };

                            cityList.Add(city);
                        }
                    }
                }
            }

            return cityList;
        }
        #endregion
        
        #region Delete

        public bool Delete(int cityID)
        {
            string connectionString = _connectionString.GetConnectionString("ConnectionString");

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("PR_LOC_City_Delete", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@CityID", cityID);
                conn.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
                return rowsAffected > 0;
            }
        }
        #endregion
        
        #region Insert

        public bool Insert(CityModel city)
        {
            string connectionString = _connectionString.GetConnectionString("ConnectionString");

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("PR_LOC_City_Insert", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@StateID", city.StateID);
                cmd.Parameters.AddWithValue("@CountryID", city.CountryID);
                cmd.Parameters.AddWithValue("@CityName", city.CityName);
                cmd.Parameters.AddWithValue("@CityCode", city.CityCode);

                conn.Open();
                int rowsAffected = cmd.ExecuteNonQuery(); // Execute the stored procedure

                // Return true if the insertion was successful
                return rowsAffected > 0;
            }
        }
        #endregion
        
        #region Update

        public bool Update(CityModel city)
        {
            string connectionString = _connectionString.GetConnectionString("ConnectionString");

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("PR_LOC_City_Update", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@CityID", city.CityID);
                cmd.Parameters.AddWithValue("@StateID", city.StateID);
                cmd.Parameters.AddWithValue("@CountryID", city.CountryID); // New parameter
                cmd.Parameters.AddWithValue("@CityName", city.CityName);
                cmd.Parameters.AddWithValue("@CityCode", city.CityCode);

                conn.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
                return rowsAffected > 0;
            }
        }
        #endregion
        
        #region GetCountries

        public IEnumerable<CountryDropDownModel> GetCountries()
        {
            var countries = new List<CountryDropDownModel>();
            string connectionString = _connectionString.GetConnectionString("ConnectionString");

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("PR_LOC_Country_SelectComboBox", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    countries.Add(new CountryDropDownModel
                    {
                        CountryID = Convert.ToInt32(reader["CountryID"]),
                        CountryName = reader["CountryName"].ToString()
                    });
                }
            }

            return countries;
        }
        #endregion
        
        #region GetStatesByCountryID

        public IEnumerable<StateDropDownModel> GetStatesByCountryID(int countryID)
        {
            var states = new List<StateDropDownModel>();
            string connectionString = _connectionString.GetConnectionString("ConnectionString");

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("PR_LOC_State_SelectComboBoxByCountryID", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@CountryID", countryID);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    states.Add(new StateDropDownModel
                    {
                        StateID = Convert.ToInt32(reader["StateID"]),
                        StateName = reader["StateName"].ToString()
                    });
                }
            }

            return states;
        }
        #endregion

        #region GetCity
        public List<CityModel> GetCity(int CityID)
        {
            string connectionString = _connectionString.GetConnectionString("ConnectionString");
            var cityList = new List<CityModel>();

            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();

                using (var objCmd = new SqlCommand("PR_City_SelectByID", conn)) // Assuming PR_City_SelectByPK is the stored procedure
                {
                    objCmd.CommandType = CommandType.StoredProcedure;
                    objCmd.Parameters.AddWithValue("@CityID", CityID); // Adding CityID parameter to the stored procedure call

                    using (var objSDR = objCmd.ExecuteReader())
                    {
                        while (objSDR.Read())
                        {
                            var city = new CityModel
                            {
                                CityID = objSDR["CityID"] == DBNull.Value ? (int?)null : Convert.ToInt32(objSDR["CityID"]),
                                CityName = objSDR["CityName"].ToString(),
                                CountryID = Convert.ToInt32(objSDR["CountryID"]),
                                StateID = Convert.ToInt32(objSDR["StateID"]),
                                CityCode = objSDR["CityCode"].ToString(),
                                CreatedDate = Convert.ToDateTime(objSDR["CreatedDate"]),
                                ModifiedDate = objSDR["ModifiedDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(objSDR["ModifiedDate"])
                            };

                            cityList.Add(city); // Adding the city object to the list
                        }
                    }
                }
            }

            return cityList; // Returning the list of CityModel objects
        }
        #endregion

    }
}
