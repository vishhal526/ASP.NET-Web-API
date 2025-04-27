using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WEB_API.Data;
using WEB_API.Models;

namespace WEB_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        #region ProductRepository
        private readonly ProductRepository _productRepository;

        public ProductController(ProductRepository productRepository)
        {
            _productRepository = productRepository;
        }
        #endregion

        #region GetAllProducts

        [HttpGet]
        public IActionResult GetAllProducts()
        {
            try
            {
                var products = _productRepository.GetAllProducts();
                return Ok(JsonConvert.SerializeObject(products));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while fetching Products.", error = ex.Message });
            }
        }
        #endregion

        #region DeleteProduct

        [HttpDelete("{id}")]
        public IActionResult DeleteProduct(int id)
        {
            var isDeleted = _productRepository.Delete(id);
            if (!isDeleted)
            {
                return NotFound();
            }
            return NoContent();
        }
        #endregion

        #region InsertProduct

        [HttpPost]
        public IActionResult InsertProduct([FromBody] ProductModel product)
        {
            if (product == null)
                return BadRequest();

            bool isInserted = _productRepository.Insert(product);

            if (isInserted)
                return Ok(new { Message = "Product inserted successfully!" });

            return StatusCode(500, "An error occurred while inserting the product.");
        }
        #endregion

        #region UpdateProduct

        [HttpPut("{id}")]
        public IActionResult UpdateProduct(int id, [FromBody] ProductModel product)
        {
            if (product == null || id != product.ProductID)
                return BadRequest();

            var isUpdated = _productRepository.Update(product);
            if (!isUpdated)
                return NotFound();

            return NoContent();
        }
        #endregion

        #region GetUsers

        [HttpGet("users")]
        public IActionResult GetUsers()
        {
            var users = _productRepository.GetUsers();
            if (!users.Any())
                return NotFound("No Users found.");

            return Ok(users);
        }
        #endregion

        #region GetProduct

        [HttpGet("{id}")]
        public IActionResult GetProduct(int id)
        {
            try
            {
                // Fetch the product using the product ID from the repository or service
                var product = _productRepository.GetProduct(id);

                if (product == null)
                {
                    return NotFound(new { message = "Product not found." });
                }

                return Ok(JsonConvert.SerializeObject(product));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while fetching the product.", error = ex.Message });
            }
        }

        #endregion

    }
}

