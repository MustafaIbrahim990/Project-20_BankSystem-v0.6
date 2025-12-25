using BankSystem.BLL.Services;
using BankSystem.DTOs.ClientDTOs;
using BankSystem.DTOs.CountryDTOs;
using BankSystem.DTOs.CurrencyDTOs;
using BankSystem.DTOs.PersonDTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace BankSystem.API.Controllers
{
    [Route("api/Currencies")]
    [ApiController]
    public class CurrenciesController : ControllerBase
    {
        //Get All Currencies :-
        [HttpGet()]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<CurrencyReadDTO>>> GetAllCurrenciesAsync(int pageNumber = 1, int pageSize = 10)
        {
            if (pageNumber <= 0)
                return BadRequest("Page number must be a positive.");

            if (pageSize <= 0)
                return BadRequest("Page size must be a positive.");

            try
            {
                var currencies = await CurrencyService.GetAllAsync(pageNumber, pageSize);
                return Ok(currencies);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Message = $"An error occurred while fetching the currencies.",
                    Details = ex.Message
                });
            }
        }


        //Does Currency Exist By Id :-
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("exists-by-currency-id/{id:int:min(1)}")]
        public async Task<ActionResult<bool>> ExistsAsync(int id)
        {
            if (id <= 0)
                return BadRequest(new { Message = "Currency Id must be a positive number." });

            try
            {
                return Ok(await CurrencyService.ExistsByIdAsync(id));
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


        //Does Currency Exist By CurrencyCode :-
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("exists-by-currency-code/{currencyCode:required}")]
        public async Task<ActionResult<bool>> ExistsAsync(string currencyCode)
        {
            if (string.IsNullOrWhiteSpace(currencyCode))
                return BadRequest(new { Message = "Currency code is required." });

            try
            {
                return Ok(await CurrencyService.ExistsByCodeAsync(currencyCode));
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


        //Get Currency Info By ID :-
        [HttpGet("by-currency-id/{id:int:min(1)}", Name = "GetCurrencyByIdAsync")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<CurrencyReadDTO>> GetByIdAsync(int id)
        {
            if (id <= 0)
                return BadRequest(new { Message = "Currency ID must be a positive number." });

            try
            {
                CurrencyReadDTO dto = await CurrencyService.GetAsync(id);
                return Ok(dto);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (SqlException ex)
            {
                if (ex.Number == 50002)
                    return NotFound(new { Message = $"Currency with Id [{id}] not found." });

                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Message = "An error occurred while fetching the currency.",
                    Details = ex.Message
                });
            }
        }


        //Get Currency Info By CurrencyCode :-
        [HttpGet("by-currency-code/{currencyCode:required}", Name = "GetCurrencyByCurrencyCodeAsync")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<CurrencyReadDTO>> GetByCurrencyCodeAsync(string currencyCode)
        {
            if (string.IsNullOrWhiteSpace(currencyCode))
                return BadRequest(new { Message = "currency code is required." });

            try
            {
                CurrencyReadDTO dto = await CurrencyService.GetAsync(currencyCode);
                return Ok(dto);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (SqlException ex)
            {
                if (ex.Number == 50002)
                    return NotFound(new { Message = $"currency with code [{currencyCode}] not found." });

                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Message = "An error occurred while fetching the currency.",
                    Details = ex.Message
                });
            }
        }


        //Create Currency :-
        [HttpPost()]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<CurrencyReadDTO>> CreateAsync(CurrencyCreateDTO newCurrencyDTO)
        {
            if (newCurrencyDTO == null)
                return BadRequest(new { Message = "Invalid currency data." });

            try
            {
                CurrencyReadDTO dto = await CurrencyService.CreateAsync(newCurrencyDTO);
                return CreatedAtRoute("GetCurrencyByIdAsync", new { id = dto.CurrencyID }, dto);
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
                    return BadRequest(new { Message = $"Currency with name [{newCurrencyDTO.CurrencyName}] already exists." });

                if (ex.Number == 50004)
                    return BadRequest(new { Message = $"Currency with code [{newCurrencyDTO.CurrencyCode}] already exists." });

                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Message = "An error occurred while adding the currency.",
                    Details = ex.Message
                });
            }
        }


        //Update Currency By Id :-
        [HttpPut("{id:int:min(1)}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<CurrencyReadDTO>> UpdateAsync(int id, CurrencyUpdateDTO updatedCurrencyDTO)
        {
            if (id <= 0)
                return BadRequest(new { Message = "Currency Id must be a positive number." });

            if (updatedCurrencyDTO == null)
                return BadRequest(new { Message = "Invalid currency data." });

            try
            {
                CurrencyReadDTO dto = await CurrencyService.UpdateAsync(id, updatedCurrencyDTO);
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
                    return BadRequest(new { Message = $"Currency with name [{updatedCurrencyDTO.CurrencyName}] already exists." });

                if (ex.Number == 50004)
                    return BadRequest(new { Message = $"Currency with code [{updatedCurrencyDTO.CurrencyCode}] already exists." });

                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Message = "An error occurred while updating the currency.",
                    Details = ex.Message
                });
            }
        }


        //Delete Currency By Id :-
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpDelete("by-currency-id/{id:int:min(1)}")]
        public async Task<ActionResult<bool>> DeleteAsync(int id)
        {
            if (id <= 0)
                return BadRequest(new { Message = "Currency ID must be a positive number." });

            try
            {
                return Ok(await CurrencyService.DeleteAsync(id));
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
                    return NotFound(new { Message = $"Currency with Id [{id}] not found." });

                if (ex.Number == 50003)
                    return BadRequest(new { Message = $"Cannot delete currency : this currency linked to an active country." });

                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Message = "An error occurred while deleting the currency.",
                    Details = ex.Message
                });
            }
        }


        //Delete Currency By CurrencyCode :-
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpDelete("by-currency-code/{currencyCode:required}")]
        public async Task<ActionResult<bool>> DeleteAsync(string currencyCode)
        {
            if (string.IsNullOrWhiteSpace(currencyCode))
                return BadRequest(new { Message = "Currency code is required." });

            try
            {
                return Ok(await CurrencyService.DeleteAsync(currencyCode));
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
                    return NotFound(new { Message = $"Currency with code [{currencyCode}] not found." });

                if (ex.Number == 50003)
                    return BadRequest(new { Message = $"Cannot delete currency : this currency linked to an active country." });

                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Message = "An error occurred while deleting the currency.",
                    Details = ex.Message
                });
            }
        }
    }
}