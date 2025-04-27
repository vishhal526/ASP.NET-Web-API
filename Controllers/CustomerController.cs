using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WEB_API.Data;
using WEB_API.Models;

namespace WEB_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        #region CustomerRepository
        private readonly CustomerRepository _customerRepository;

        public CustomerController(CustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }
        #endregion

        #region GetAllCustomers

        [HttpGet]
        public IActionResult GetAllCustomers()
        {
            try
            {
                var customers = _customerRepository.GetAllCustomers();
                return Ok(JsonConvert.SerializeObject(customers));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while fetching Customers.", error = ex.Message });
            }
        }
        #endregion

        #region DeleteCustomer

        [HttpDelete("{id}")]
        public IActionResult DeleteCustomer(int id)
        {
            var isDeleted = _customerRepository.Delete(id);
            if (!isDeleted)
            {
                return NotFound();
            }
            return NoContent();
        }
        #endregion

        #region InsertCustomer

        [HttpPost]
        public IActionResult InsertCustomer([FromBody] CustomerModel customer)
        {
            if (customer == null)
                return BadRequest();

            bool isInserted = _customerRepository.Insert(customer);

            if (isInserted)
                return Ok(new { Message = "Customer inserted successfully!" });

            return StatusCode(500, "An error occurred while inserting the customer.");
        }
        #endregion

        #region UpdateCustomer

        [HttpPut("{id}")]
        public IActionResult UpdateCustomer(int id, [FromBody] CustomerModel customer)
        {
            if (customer == null || id != customer.CustomerID)
                return BadRequest();

            var isUpdated = _customerRepository.Update(customer);
            if (!isUpdated)
                return NotFound();

            return NoContent();
        }
        #endregion

        #region GetUsers

        [HttpGet("users")]
        public IActionResult GetUsers()
        {
            var users = _customerRepository.GetUsers();
            if (!users.Any())
                return NotFound("No Users found.");

            return Ok(users);
        }
        #endregion

        #region GetCustomer

        [HttpGet("{id}")]
        public IActionResult GetCustomer(int id)
        {
            try
            {
                // Fetch the customer using the customer ID from the repository or service
                var customer = _customerRepository.GetCustomer(id);

                if (customer == null)
                {
                    return NotFound(new { message = "Customer not found." });
                }

                return Ok(JsonConvert.SerializeObject(customer));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while fetching the customer.", error = ex.Message });
            }
        }

        #endregion

    }

}
