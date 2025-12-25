using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using BankSystem.BLL.Services;
using BankSystem.DTOs.PersonDTOs;

namespace BankSystem.API.Controllers
{
    [Route("api/People")]
    [ApiController]
    public class PeopleController : ControllerBase
    {
        //Get All People :-
        [HttpGet()]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<PersonReadDetailsDTO>>> GetAllPeopleAsync(int pageNumber = 1, int pageSize = 10)
        {
            if (pageNumber <= 0)
                return BadRequest(new { Message = "Page number must be a valid number." });

            if (pageSize <= 0)
                return BadRequest(new { Message = "Page size must be a valid number." });

            try
            {
                var people = await PersonService.GetAllAsync(pageNumber, pageSize);
                return Ok(people);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                { 
                    Message = "An error occurred while fetching people.",
                    Details = ex.Message
                });
            }
        }


        //Person Exists By Id :-
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("exists-by-person/{id:int:min(1)}")]
        public async Task<ActionResult<bool>> ExistsAsync(int id)
        {
            if (id <= 0)
                return BadRequest(new { Message = "Person Id must be a valid number." });
            try
            {
                return Ok(await PersonService.ExistsAsync(id));
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


        //Get Person Info By ID :-
        [HttpGet("{id:int:min(1)}", Name = "GetPersonByIdAsync")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PersonReadBasicDTO>> GetByIdAsync(int id)
        {
            if (id <= 0)
                return BadRequest(new { Message = "Person Id must be a valid number." });

            try
            {
                PersonReadBasicDTO dto = await PersonService.GetAsync(id);
                return Ok(dto);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (SqlException ex) 
            {
                if (ex.Number == 50002)
                    return NotFound(new { Message = $"Person with Id [{id}] not found." });

                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Message = "An error occurred while fetching the person.",
                    Details = ex.Message
                });
            }
        }


        //Add New Person :-
        [HttpPost()]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PersonReadBasicDTO>> CreateAsync(PersonCreateDTO newPersonDTO)
        {
            if (newPersonDTO == null)
                return BadRequest(new { Message = "Invalid person data." });

            try
            {
                PersonReadBasicDTO dto = await PersonService.CreateAsync(newPersonDTO);
                return CreatedAtRoute("GetPersonByIdAsync", new { id = dto.PersonID }, dto);
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
                if (ex.Number == 50001)
                    return BadRequest(new { Message = $"Country with Id [{newPersonDTO.CountryID}] not found." });

                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Message = "An error occurred while adding the person.",
                    Details = ex.Message
                });
            }
        }


        //Update Person By Id :-
        [HttpPut("{id:int:min(1)}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PersonReadBasicDTO>> UpdateAsync(int id, PersonUpdateDTO updatedPersonDTO)
        {
            if (id <= 0)
                return BadRequest(new { Message = "Page number must be a valid number." });

            if (updatedPersonDTO == null)
                return BadRequest(new { Message = "Invalid person data." });


            try
            {
                PersonReadBasicDTO dto = await PersonService.UpdateAsync(id, updatedPersonDTO);
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
                    return NotFound(new { Message = $"Person with Id [{id}] not found." });

                if (ex.Number == 50003)
                    return BadRequest(new { Message = $"Country with Id [{updatedPersonDTO.CountryID}] not found." });

                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Message = "An error occurred while updating the person.",
                    Details = ex.Message
                });
            }
        }


        //Delete Person By Id :-
        [HttpDelete("{id:int:min(1)}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<bool>> DeleteAsync(int id)
        {
            if (id <= 0)
                return BadRequest(new { Message = "Person Id must be a valid number." });

            try
            {
                return Ok(await PersonService.DeleteAsync(id));
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
                    return NotFound(new { Message = $"Person with Id [{id}] not found." });

                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Message = "An error occurred while deleting the person.",
                    Details = ex.Message
                });
            }
        }


        //Is Email Unique :-
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("email-unique/{email:required}")]
        public async Task<ActionResult<bool>> IsEmailUniqueAsync(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return BadRequest(new { Message = "Email is required." });

            try
            {
                return Ok(await PersonService.IsEmailUniqueAsync(email));
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


        //Is Phone Unique :-
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("phone-unique/{phone:required}")]
        public async Task<ActionResult<bool>> IsPhoneUniqueAsync(string phone)
        {
            if (string.IsNullOrWhiteSpace(phone))
                return BadRequest(new { Message = "Phone is required." });

            try
            {
                return Ok(await PersonService.IsPhoneUniqueAsync(phone));
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
    }
}