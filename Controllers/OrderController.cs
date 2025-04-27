using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WEB_API.Data;
using WEB_API.Models;

namespace WEB_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        #region OrderRepository
        private readonly OrderRepository _orderRepository;

        public OrderController(OrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }
        #endregion

        #region GetAllOrders

        [HttpGet]
        public IActionResult GetAllOrders()
        {
            try
            {
                var orders = _orderRepository.GetAllOrders();
                return Ok(JsonConvert.SerializeObject(orders));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while fetching Orders.", error = ex.Message });
            }
        }
        #endregion

        #region DeleteOrder

        [HttpDelete("{id}")]
        public IActionResult DeleteOrder(int id)
        {
            var isDeleted = _orderRepository.Delete(id);
            if (!isDeleted)
            {
                return NotFound();
            }
            return NoContent();
        }
        #endregion

        #region InsertOrder

        [HttpPost]
        public IActionResult InsertOrder([FromBody] OrderModel order)
        {
            if (order == null)
                return BadRequest();

            bool isInserted = _orderRepository.Insert(order);

            if (isInserted)
                return Ok(new { Message = "Order inserted successfully!" });

            return StatusCode(500, "An error occurred while inserting the order.");
        }
        #endregion

        #region UpdateOrder

        [HttpPut("{id}")]
        public IActionResult UpdateOrder(int id, [FromBody] OrderModel order)
        {
            if (order == null || id != order.OrderID)
                return BadRequest();

            var isUpdated = _orderRepository.Update(order);
            if (!isUpdated)
                return NotFound();

            return NoContent();
        }
        #endregion

        #region GetUsers

        [HttpGet("users")]
        public IActionResult GetUsers()
        {
            var users = _orderRepository.GetUsers();
            if (!users.Any())
                return NotFound("No Users found.");

            return Ok(users);
        }
        #endregion

        #region GetCustomers

        [HttpGet("customers")]
        public IActionResult GetCustomers()
        {
            var customers = _orderRepository.GetCustomers();
            if (!customers.Any())
                return NotFound("No customers found.");

            return Ok(customers);
        }
        #endregion

        #region GetOrder

        [HttpGet("{id}")]
        public IActionResult GetOrder(int id)
        {
            try
            {
                // Fetch the order using the order ID from the repository or service
                var order = _orderRepository.GetOrder(id);

                if (order == null)
                {
                    return NotFound(new { message = "Order not found." });
                }

                return Ok(JsonConvert.SerializeObject(order));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while fetching the order.", error = ex.Message });
            }
        }

        #endregion

    }
}
