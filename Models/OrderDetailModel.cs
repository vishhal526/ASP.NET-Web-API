using System.ComponentModel.DataAnnotations;
namespace WEB_API.Models
{
    public class OrderDetailModel
    {
        public int? OrderDetailID { get; set; }
        public int OrderID { get; set; }
        public int ProductID { get; set; }
        public string ProductName { get; set; }

        public int Quantity { get; set; }
        public decimal Amount { get; set; }
        public decimal TotalAmount { get; set; }
        public int UserID { get; set; }
        public string? UserName { get; set; }

    }
    public class OrderDetailProductDropDownModel
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
    }
    public class OrderDetailOrderDropDownModel
    {
        public int OrderID { get; set; }
        public int OrderNumber { get; set; }
    }
    public class OrderDetailUserDropDownModel
    {
        public int UserID { get; set; }
        public string UserName { get; set; }
    }
}
