using Microsoft.AspNetCore.OpenApi;
using Microsoft.AspNetCore.Http.HttpResults;
namespace WEB_API.Models
{
    public class BillModel
    {
        public int? BillID { get; set; }
        public string BillNumber { get; set; }
        public DateTime BillDate { get; set; }
        public int OrderID { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal? Discount { get; set; }
        public decimal NetAmount { get; set; }
        public int UserID { get; set; }
        public string? UserName{ get; set; }
    }
    public class BillUserDropDownModel
    {
        public int UserID { get; set; }
        public string UserName { get; set; }
    }
    public class BillOrderDropDownModel
    {
        public int OrderID { get; set; }
        public int OrderNumber { get; set; }
    }
}