namespace WEB_API.Models
{
    public class OrderModel
    {
        public int? OrderID { get; set; }
        public DateTime OrderDate { get; set; }
        public int CustomerID { get; set; }
        public string? CustomerName { get; set; }
        public string PaymentMode { get; set; }
        public decimal? TotalAmount { get; set; }
        public string ShippingAddress { get; set; }
        public int UserID { get; set; }
        public string? UserName { get; set; }
    }
    public class OrderUserDropDownModel
    {
        public int UserID { get; set; }
        public string UserName { get; set; }
    }
    public class OrderCustomerDropDownModel
    {
        public int CustomerID { get; set; }
        public string CustomerName { get; set; }
    }
}
