﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace WEB_API.Models
{
    public class CityModel
    {
        public int? CityID { get; set; }
        public string CityName { get; set; }
        public int CountryID { get; set; }
        public int StateID { get; set; }
        public string CityCode { get; set; }
        public string? CountryName { get; set; }
        public string? StateName { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
    public class CountryDropDownModel
    {
        public int CountryID { get; set; }
        public string CountryName { get; set; }
    }
    public class StateDropDownModel
    {
        public int StateID { get; set; }
        public string StateName { get; set; }
    }
}
