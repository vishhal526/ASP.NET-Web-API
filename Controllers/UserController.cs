using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WEB_API.Data;
using WEB_API.Models;

namespace WEB_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        #region UserRepository
        private readonly UserRepository _userRepository;

        public UserController(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        #endregion

        #region GetAllUsers

        [HttpGet]
        public IActionResult GetAllUsers()
        {
            try
            {
                var users = _userRepository.GetAllUsers();
                return Ok(JsonConvert.SerializeObject(users));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while fetching Users.", error = ex.Message });
            }
        }
        #endregion

        #region DeleteUser

        [HttpDelete("{id}")]
        public IActionResult DeleteUser(int id)
        {
            var isDeleted = _userRepository.Delete(id);
            if (!isDeleted)
            {
                return NotFound();
            }
            return NoContent();
        }
        #endregion

        #region InsertUser

        [HttpPost]
        public IActionResult InsertUser([FromBody] UserModel user)
        {
            if (user == null)
                return BadRequest();

            bool isInserted = _userRepository.Insert(user);

            if (isInserted)
                return Ok(new { Message = "User inserted successfully!" });

            return StatusCode(500, "An error occurred while inserting the user.");
        }
        #endregion

        #region UpdateUser

        [HttpPut("{id}")]
        public IActionResult UpdateUser(int id, [FromBody] UserModel user)
        {
            if (user == null || id != user.UserID)
                return BadRequest();

            var isUpdated = _userRepository.Update(user);
            if (!isUpdated)
                return NotFound();

            return NoContent();
        }
        #endregion

        #region GetUser

        [HttpGet("{id}")]
        public IActionResult GetUser(int id)
        {
            try
            {
                var user = _userRepository.GetUser(id);
                return Ok(JsonConvert.SerializeObject(user));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while fetching User.", error = ex.Message });
            }
        }
        #endregion
    }
}
