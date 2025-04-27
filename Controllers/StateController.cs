using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WEB_API.Data;
using WEB_API.Models;

namespace WEB_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StateController : ControllerBase
    {
        #region StateRepository
        private readonly StateRepository _stateRepository;

        public StateController(StateRepository stateRepository)
        {
            _stateRepository = stateRepository;
        }
        #endregion

        #region GetAllStates

        [HttpGet]
        public IActionResult GetAllStates()
        {
            try
            {
                var states = _stateRepository.GetAllStates();
                return Ok(JsonConvert.SerializeObject(states));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while fetching states.", error = ex.Message });
            }
        }
        #endregion

        #region DeleteState

        [HttpDelete("{id}")]
        public IActionResult DeleteState(int id)
        {
            var isDeleted = _stateRepository.Delete(id);
            if (!isDeleted)
            {
                return NotFound();
            }
            return NoContent();
        }
        #endregion

        #region InsertState

        [HttpPost]
        public IActionResult InsertState([FromBody] StateModel state)
        {
            if (state == null)
                return BadRequest();

            bool isInserted = _stateRepository.Insert(state);

            if (isInserted)
                return Ok(new { Message = "State inserted successfully!" });

            return StatusCode(500, "An error occurred while inserting the State.");
        }
        #endregion

        #region UpdateState

        [HttpPut("{id}")]
        public IActionResult UpdateState(int id, [FromBody] StateModel state)
        {
            if (state == null || id != state.StateID)
                return BadRequest();

            var isUpdated = _stateRepository.Update(state);
            if (!isUpdated)
                return NotFound();

            return NoContent();
        }
        #endregion

        #region GetCountries

        [HttpGet("countries")]
        public IActionResult GetCountries()
        {
            var countries = _stateRepository.GetCountries();
            if (!countries.Any())
                return NotFound("No countries found.");

            return Ok(countries);
        }
        #endregion

        #region GetState

        [HttpGet("{id}")]
        public IActionResult GetState(int id)
        {
            try
            {
                // Fetch the state using the state ID from the repository or service
                var state = _stateRepository.GetState(id);

                if (state == null)
                {
                    return NotFound(new { message = "State not found." });
                }

                return Ok(JsonConvert.SerializeObject(state));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while fetching the state.", error = ex.Message });
            }
        }

        #endregion

    }
}
