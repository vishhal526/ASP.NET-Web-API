﻿using System.ComponentModel.DataAnnotations;

namespace WEB_API.Models
{
    public class CustomerModel
    {
        public int? CustomerID { get; set; }
        public string CustomerName { get; set; }
        public string HomeAddress { get; set; }
        public string Email { get; set; }
        public string MobileNo { get; set; }
        public string GSTNO { get; set; }
        public string CityName { get; set; }
        public string PinCode { get; set; }
        public decimal NetAmount { get; set; }
        public int UserID { get; set; }
        public string? UserName { get; set; }
    }
    public class CustomerUserDropDownModel
    {
        public int UserID { get; set; }
        public string UserName { get; set; }
    }
}
