using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WEB_API.Data;
using WEB_API.Models;

namespace WEB_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderDetailController : ControllerBase
    {
        #region OrderDetailRepository
        private readonly OrderDetailRepository _orderDetailRepository;

        public OrderDetailController(OrderDetailRepository orderDetailRepository)
        {
            _orderDetailRepository = orderDetailRepository;
        }
        #endregion

        #region GetAllOrderDetails

        [HttpGet]
        public IActionResult GetAllOrderDetails()
        {
            try
            {
                var orderDetails = _orderDetailRepository.GetAllOrderDetails();
                return Ok(JsonConvert.SerializeObject(orderDetails));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while fetching Order Details.", error = ex.Message });
            }
        }
        #endregion

        #region DeleteOrderDetail

        [HttpDelete("{id}")]
        public IActionResult DeleteOrderDetail(int id)
        {
            var isDeleted = _orderDetailRepository.Delete(id);
            if (!isDeleted)
            {
                return NotFound();
            }
            return NoContent();
        }
        #endregion

        #region InsertOrderDetail

        [HttpPost]
        public IActionResult InsertOrderDetail([FromBody] OrderDetailModel orderDetail)
        {
            if (orderDetail == null)
                return BadRequest();

            bool isInserted = _orderDetailRepository.Insert(orderDetail);

            if (isInserted)
                return Ok(new { Message = "Order Detail inserted successfully!" });

            return StatusCode(500, "An error occurred while inserting the order detail.");
        }
        #endregion

        #region UpdateOrderDetail

        [HttpPut("{id}")]
        public IActionResult UpdateOrderDetail(int id, [FromBody] OrderDetailModel orderDetail)
        {
            if (orderDetail == null || id != orderDetail.OrderDetailID)
                return BadRequest();

            var isUpdated = _orderDetailRepository.Update(orderDetail);
            if (!isUpdated)
                return NotFound();

            return NoContent();
        }
        #endregion

        #region GetUsers

        [HttpGet("users")]
        public IActionResult GetUsers()
        {
            var users = _orderDetailRepository.GetUsers();
            if (!users.Any())
                return NotFound("No Users found.");

            return Ok(users);
        }
        #endregion

        #region GetOrders

        [HttpGet("orders")]
        public IActionResult GetOrders()
        {
            var orders = _orderDetailRepository.GetOrders();
            if (!orders.Any())
                return NotFound("No Orders found.");

            return Ok(orders);
        }
        #endregion

        #region GetProducts

        [HttpGet("products")]
        public IActionResult GetProducts()
        {
            var products = _orderDetailRepository.GetProducts();
            if (!products.Any())
                return NotFound("No products found.");

            return Ok(products);
        }
        #endregion

        #region GetOrderDetail

        [HttpGet("{id}")]
        public IActionResult GetOrderDetail(int id)
        {
            try
            {
                // Fetch the order detail using the order detail ID from the repository or service
                var orderDetail = _orderDetailRepository.GetOrderDetail(id);

                if (orderDetail == null)
                {
                    return NotFound(new { message = "Order Detail not found." });
                }

                return Ok(JsonConvert.SerializeObject(orderDetail));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while fetching the order detail.", error = ex.Message });
            }
        }

        #endregion

    }
}
