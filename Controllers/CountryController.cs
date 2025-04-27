using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics.Metrics;
using WEB_API.Data;
using WEB_API.Models;

namespace WEB_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController : ControllerBase
    {
        #region CountryRepository , Validator
        private readonly CountryRepository _countryRepository;

        private readonly IValidator<CountryModel> _validator;

        public CountryController(CountryRepository CountryRepositry, IValidator<CountryModel> validator)
        {
            _countryRepository = CountryRepositry;
            _validator = validator;

        }
        #endregion

        #region GetAllCountries

        [HttpGet]
        public IActionResult GetAllCountries()
        {
            try
            {
                // Fetch all countries using the repository
                var countries = _countryRepository.GetAllCountries();

                // Return the serialized list of countries as JSON
                return Ok(JsonConvert.SerializeObject(countries));
            }
            catch (Exception ex)
            {
                // Handle exceptions and return a proper error response
                return StatusCode(500, new { message = "An error occurred while fetching countries.", error = ex.Message });
            }
        }
        #endregion

        #region DeleteCountry

        [HttpDelete("{id}")]
        public IActionResult DeleteCountry(int id)
        {
            var isDeleted = _countryRepository.Delete(id);
            if (!isDeleted)
            {
                return NotFound();
            }
            return NoContent();
        }
        #endregion

        #region InsertCountry

        [HttpPost]
        public IActionResult InsertCountry([FromBody] CountryModel country)
        {
            var validationResult = _validator.Validate(country);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            if (country == null)
                return BadRequest();

            bool isInserted = _countryRepository.Insert(country);

            if (isInserted)
                return Ok(new { Message = "Country inserted successfully!" });

            return StatusCode(500, "An error occurred while inserting the country.");
        }
        #endregion

        #region UpdateCountry

        [HttpPut("{id}")]
        public IActionResult UpdateCountry(int id, [FromBody] CountryModel country)
        {
            if (country == null || id != country.CountryID)
                return BadRequest();

            var isUpdated = _countryRepository.Update(country);
            if (!isUpdated)
                return NotFound();

            return NoContent();
        }
        #endregion

        #region GetCountry

        [HttpGet("{id}")]
        public IActionResult GetCountry(int id)
        {
            try
            {
                // Fetch the country using the country ID from the repository or service
                var country = _countryRepository.GetCountry(id);

                if (country == null)
                {
                    return NotFound(new { message = "Country not found." });
                }

                return Ok(JsonConvert.SerializeObject(country));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while fetching country.", error = ex.Message });
            }
        }

        #endregion

    }

}
