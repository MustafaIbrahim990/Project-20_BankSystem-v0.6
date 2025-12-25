using BankSystem.BLL;
using BankSystem.BLL.Services;
using BankSystem.DTOs.ClientDTOs;
using BankSystem.DTOs.CountryDTOs;
using BankSystem.DTOs.PersonDTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace BankSystem.API.Controllers
{
    [Route("api/Countries")]
    [ApiController]
    public class CountriesController : ControllerBase
    {
        //Get All Countries :-
        [HttpGet()]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<CountryReadDetailsDTO>>> GetAllCountriesAsync(int pageNumber = 1, int pageSize = 10)
        {
            if (pageNumber <= 0)
                return BadRequest("Page number must be a positive.");

            if (pageSize <= 0)
                return BadRequest("Page size must be a positive.");

            try
            {
                var countries = await CountryService.GetAllAsync(pageNumber, pageSize);
                return Ok(countries);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Message = $"An error occurred while fetching the countries.",
                    Details = ex.Message
                });
            }
        }


        //Does Country Exist By Id :-
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("exists-by-country-id/{id:int:min(1)}")]
        public async Task<ActionResult<bool>> ExistsAsync(int id)
        {
            if (id <= 0)
                return BadRequest(new { Message = "Country Id must be a positive number." });

            try
            {
                return Ok(await CountryService.ExistsAsync(id));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (SqlException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Message = "An error occurred while checking existence.",
                    Details = ex.Message
                });
            }
        }


        //Does Country Exist By CountryName :-
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("exists-by-country-name/{countryName:required}")]
        public async Task<ActionResult<bool>> ExistsAsync(string countryName)
        {
            if (string.IsNullOrWhiteSpace(countryName))
                return BadRequest(new { Message = "Country name is required." });

            try
            {
                return Ok(await CountryService.ExistsAsync(countryName));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (SqlException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Message = "An error occurred while checking existence.",
                    Details = ex.Message
                });
            }
        }


        //Get Country Info By ID :-
        [HttpGet("by-country-id/{id:int:min(1)}", Name = "GetCountryByIdAsync")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ClientReadBasicDTO>> GetByIdAsync(int id)
        {
            if (id <= 0)
                return BadRequest(new { Message = "Country Id must be a positive number." });

            try
            {
                CountryReadBasicDTO dto = await CountryService.GetAsync(id);
                return Ok(dto);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Message = ex.Message});
            }
            catch (SqlException ex)
            {
                if (ex.Number == 50002)
                    return NotFound(new { Message = $"Country with Id [{id}] not found." });

                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Message = "An error occurred while fetching the country.",
                    Details = ex.Message
                });
            }
        }


        //Get Country Info By CountryName :-
        [HttpGet("by-country-name/{countryName:required}", Name = "GetCountryByCountryNameAsync")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ClientReadBasicDTO>> GetByCountryNameAsync(string countryName)
        {
            if (string.IsNullOrWhiteSpace(countryName))
                return BadRequest(new { Message = "Country name is required." });

            try
            {
                CountryReadBasicDTO dto = await CountryService.GetAsync(countryName);
                return Ok(dto);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (SqlException ex)
            {
                if (ex.Number == 50002)
                    return NotFound(new { Message = $"Country with name [{countryName}] not found." });

                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Message = "An error occurred while fetching the country.",
                    Details = ex.Message
                });
            }
        }


        //Get Country Info By ID :-
        [HttpGet("by-currency-id/{currencyId:int:min(1)}", Name = "GetCountriesByCurrencyIDAsync")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<CountryReadDetailsDTO>>> GetCountriesByCurrencyIDAsync(int currencyId)
        {
            if (currencyId <= 0)
                return BadRequest(new { Message = "Currency Id must be a positive number." });

            try
            {
                var countries = await CountryService.GetCountriesByCurrencyIdAsync(currencyId);
                return Ok(countries);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (SqlException ex)
            {
                if (ex.Number == 50002)
                    return NotFound(new { Message = $"Country with currency id [{currencyId}] not found." });

                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Message = "An error occurred while fetching the country.",
                    Details = ex.Message
                });
            }
        }


        //Create Country :-
        [HttpPost()]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<CountryReadBasicDTO>> CreateAsync(CountryCreateDTO newCountryDTO)
        {
            if (newCountryDTO == null)
                return BadRequest(new { Message = "Invalid country data." });

            try
            {
                CountryReadBasicDTO dto = await CountryService.CreateAsync(newCountryDTO);
                return CreatedAtRoute("GetCountryByIdAsync", new { id = dto.CountryID }, dto);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (SqlException ex)
            {
                if (ex.Number == 50002)
                    return BadRequest(new { Message = $"Currency with Id [{newCountryDTO.CurrencyID}] not found." });

                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Message = "An error occurred while adding the country.",
                    Details = ex.Message
                });
            }
        }


        //Update Country By Id :-
        [HttpPut("{id:int:min(1)}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PersonReadBasicDTO>> UpdateAsync(int id, CountryUpdateDTO updatedCountryDTO)
        {
            if (id <= 0)
                return BadRequest(new { Message = "Country Id must be a positive number." });

            if (updatedCountryDTO == null)
                return BadRequest(new { Message = "Invalid country data." });

            try
            {
                CountryReadBasicDTO dto = await CountryService.UpdateAsync(id, updatedCountryDTO);
                return Ok(dto);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (SqlException ex)
            {
                if (ex.Number == 50002)
                    return NotFound(new { Message = $"Currency with Id [{updatedCountryDTO.CurrencyID}] not found." });

                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Message = "An error occurred while updating the country.",
                    Details = ex.Message
                });
            }
        }


        //Delete Country By Id :-
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpDelete("by-country-id/{id:int:min(1)}")]
        public async Task<ActionResult<bool>> DeleteAsync(int id)
        {
            if (id <= 0)
                return BadRequest(new { Message = "Country Id must be a positive number." });

            try
            {
                return Ok(await CountryService.DeleteAsync(id));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (SqlException ex)
            {
                if (ex.Number == 50002)
                    return NotFound(new { Message = $"Country with Id [{id}] not found." });

                if (ex.Number == 50003)
                    return BadRequest(new { Message = $"Cannot delete country : this country linked to an active person account." });

                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Message = "An error occurred while deleting the country.",
                    Details = ex.Message
                });
            }
        }


        //Delete Country By Id :-
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpDelete("by-country-name/{countryName:required}")]
        public async Task<ActionResult<bool>> DeleteAsync(string countryName)
        {
            if (string.IsNullOrWhiteSpace(countryName))
                return BadRequest(new { Message = "Country name is required." });

            try
            {
                return Ok(await CountryService.DeleteAsync(countryName));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (SqlException ex)
            {
                if (ex.Number == 50002)
                    return NotFound(new { Message = $"Country with name [{countryName}] not found." });

                if (ex.Number == 50003)
                    return BadRequest(new { Message = $"Cannot delete country : this country linked to an active person account." });

                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Message = "An error occurred while deleting the country.",
                    Details = ex.Message
                });
            }
        }
    }
}