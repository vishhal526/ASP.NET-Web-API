using System.Data;
using System.Data.SqlClient;
using WEB_API.Models;

namespace WEB_API.Data
{
    public class StateRepository
    {

        #region Configuration
        private readonly IConfiguration _connectionString;
        public StateRepository(IConfiguration _configuration)
        {
            _connectionString = _configuration;
        }
        #endregion
        
        #region GetAllStates

        public List<StateModel> GetAllStates()
        {
            var StateList = new List<StateModel>();
            String ConnectionString = _connectionString.GetConnectionString("ConnectionString");
            using (var conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                using (var cmd = new SqlCommand("PR_LOC_State_SelectAll", conn))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    using (var sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            var State = new StateModel
                            {
                                StateID = Convert.ToInt32(sdr["StateID"]),
                                CountryID = Convert.ToInt32(sdr["CountryID"]),
                                StateName = sdr["StateName"].ToString(),
                                CountryName = sdr["CountryName"].ToString(),

                                StateCode = sdr["StateCode"].ToString(),
                                CreatedDate = Convert.ToDateTime(sdr["CreatedDate"]),
                                ModifiedDate = sdr["ModifiedDate"] != DBNull.Value ? Convert.ToDateTime(sdr["ModifiedDate"]) : DateTime.MinValue
                            };
                            StateList.Add(State);
                        }
                    }
                }
            }
            return StateList;
        }
        #endregion
        
        #region Delete
        public bool Delete(int stateID)
        {
            string connectionString = _connectionString.GetConnectionString("ConnectionString");

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("PR_LOC_State_Delete", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@StateID", stateID);
                conn.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
                return rowsAffected > 0;
            }
        }

        #endregion
        
        #region Insert
        public bool Insert(StateModel state)
        {
            string connectionString = _connectionString.GetConnectionString("ConnectionString");

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("PR_LOC_State_Insert", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@CountryID", state.CountryID);
                cmd.Parameters.AddWithValue("@StateName", state.StateName);
                cmd.Parameters.AddWithValue("@StateCode", state.StateCode);

                conn.Open();
                int rowsAffected = cmd.ExecuteNonQuery(); // Execute the stored procedure

                // Return true if the insertion was successful
                return rowsAffected > 0;
            }
        }

        #endregion
        
        #region Update
        public bool Update(StateModel state)
        {
            string connectionString = _connectionString.GetConnectionString("ConnectionString");

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("PR_LOC_state_Update", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@StateID", state.StateID);
                cmd.Parameters.AddWithValue("@CountryID", state.CountryID); // New parameter
                cmd.Parameters.AddWithValue("@StateName", state.StateName);
                cmd.Parameters.AddWithValue("@StateCode", state.StateCode);

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

        #region GetState
        public List<StateModel> GetState(int StateID)
        {
            string connectionString = _connectionString.GetConnectionString("ConnectionString");
            var stateList = new List<StateModel>();

            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();

                using (var objCmd = new SqlCommand("PR_State_SelectByID", conn)) // Assuming PR_State_SelectByPK is the stored procedure
                {
                    objCmd.CommandType = CommandType.StoredProcedure;
                    objCmd.Parameters.AddWithValue("@StateID", StateID); // Adding StateID parameter to the stored procedure call

                    using (var objSDR = objCmd.ExecuteReader())
                    {
                        while (objSDR.Read())
                        {
                            var state = new StateModel
                            {
                                StateID = objSDR["StateID"] == DBNull.Value ? (int?)null : Convert.ToInt32(objSDR["StateID"]),
                                CountryID = Convert.ToInt32(objSDR["CountryID"]),
                                CountryName = objSDR["CountryName"].ToString(),
                                StateName = objSDR["StateName"].ToString(),
                                StateCode = objSDR["StateCode"].ToString(),
                                CreatedDate = Convert.ToDateTime(objSDR["CreatedDate"]),
                                ModifiedDate = objSDR["ModifiedDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(objSDR["ModifiedDate"])
                            };

                            stateList.Add(state); // Adding the state object to the list
                        }
                    }
                }
            }

            return stateList; // Returning the list of StateModel objects
        }
        #endregion

    }
}
