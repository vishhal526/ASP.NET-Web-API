using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WEB_API.Data;
using WEB_API.Models;

namespace WEB_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BillController : ControllerBase
    {
        #region BillRepository
        private readonly BillRepository _billRepository;
        public BillController(BillRepository billRepository)
        {
            _billRepository = billRepository;
        }
        #endregion

        #region GetAllBills

        [HttpGet]
        public IActionResult GetAllBills()
        {
            try
            {
                // Fetch all cities using the repository
                var bills = _billRepository.GetAllBills();

                // Return the serialized list of cities as JSON
                return Ok(JsonConvert.SerializeObject(bills));
            }
            catch (Exception ex)
            {
                // Handle exceptions and return a proper error response
                return StatusCode(500, new { message = "An error occurred while fetching Bills.", error = ex.Message });
            }
        }
        #endregion
        
        #region DeleteBill

        [HttpDelete("{id}")]
        public IActionResult DeleteBill(int id)
        {
            var isDeleted = _billRepository.Delete(id);
            if (!isDeleted)
            {
                return NotFound();
            }
            return NoContent();
        }
        #endregion
        
        #region InsertBill

        [HttpPost]
        public IActionResult InsertBill([FromBody] BillModel bill)
        {
            if (bill == null)
                return BadRequest();

            bool isInserted = _billRepository.Insert(bill);

            if (isInserted)
                return Ok(new { Message = "Bill inserted successfully!" });

            return StatusCode(500, "An error occurred while inserting the bill.");
        }
        #endregion
        
        #region UpdateBill

        [HttpPut("{id}")]
        public IActionResult UpdateBill(int id, [FromBody] BillModel bill)
        {
            if (bill == null || id != bill.BillID)
                return BadRequest();

            var isUpdated = _billRepository.Update(bill);
            if (!isUpdated)
                return NotFound();

            return NoContent();
        }
        #endregion
        
        #region GetUsers

        [HttpGet("users")]
        public IActionResult GetUsers()
        {
            var users = _billRepository.GetUsers();
            if (!users.Any())
                return NotFound("No Users found.");

            return Ok(users);
        }
        #endregion
     
        #region GetOrders

        [HttpGet("orders")]
        public IActionResult GetOrders()
        {
            var orders = _billRepository.GetOrders();
            if (!orders.Any())
                return NotFound("No Orders found.");

            return Ok(orders);
        }
        #endregion

        #region GetBill

        [HttpGet("{id}")]
        public IActionResult GetBill(int id)
        {
            try
            {
                // Fetch the bill using the bill ID from the repository or service
                var bill = _billRepository.GetBill(id);

                if (bill == null)
                {
                    return NotFound(new { message = "Bill not found." });
                }

                return Ok(JsonConvert.SerializeObject(bill));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while fetching bill.", error = ex.Message });
            }
        }

        #endregion

    }
}
