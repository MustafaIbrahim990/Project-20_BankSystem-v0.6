using BankSystem.BLL.Services;
using BankSystem.DTOs.ClientDTOs;
using BankSystem.DTOs.ClientTransferRecordDTOs;
using BankSystem.DTOs.UserLoginRecordDTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace BankSystem.API.Controllers
{
    [Route("api/UserLoginRecords")]
    [ApiController]
    public class UserLoginRecordsController : ControllerBase
    {
        //Get All User Login Records :-
        [HttpGet()]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<UserLoginRecordReadDetailsDTO>>> GetAllUserLoginRecordsAsync(int pageNumber = 1, int pageSize = 10)
        {
            if (pageNumber <= 0)
                return BadRequest("Page Number must be a positive.");

            if (pageSize <= 0)
                return BadRequest("Page Size must be a positive.");

            try
            {
                var loginRecords = await UserLoginRecordService.GetAllAsync(pageNumber, pageSize);
                return Ok(loginRecords);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Message = $"An error occurred while fetching the user login records.",
                    Details = ex.Message
                });
            }
        }


        //Does User Login Record Exist By Id :-
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("exists-by-login-id/{id:int:min(1)}")]
        public async Task<ActionResult<bool>> ExistsAsync(int id)
        {
            if (id <= 0)
                return BadRequest(new { Message = "Login Id must be a positive number." });

            try
            {
                return Ok(await UserLoginRecordService.ExistsAsync(id));
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


        //Get User Login Record Info By ID :-
        [HttpGet("by-login-id/{id:int:min(1)}", Name = "GetUserLoginRecordByIdAsync")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<UserLoginRecordReadBasicDTO>> GetByIdAsync(int id)
        {
            if (id <= 0)
                return BadRequest(new { Message = "Login Id must be a positive number." });

            try
            {
                UserLoginRecordReadBasicDTO dto = await UserLoginRecordService.GetByIdAsync(id);
                return Ok(dto);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (SqlException ex)
            {
                if (ex.Number == 50002)
                    return NotFound(new { Message = $"User login record with Id [{id}] not found." });

                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Message = "An error occurred while fetching the user login record.",
                    Details = ex.Message
                });
            }
        }


        //Get User Login Record Info By ID :-
        [HttpGet("by-user-id/{userId:int:min(1)}", Name = "GetUserLoginRecordByUserIdAsync")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<UserLoginRecordReadBasicDTO>>> GetByUserIdAsync(int userId)
        {
            if (userId <= 0)
                return BadRequest(new { Message = "User Id must be a positive number." });

            try
            {
                List<UserLoginRecordReadBasicDTO> dto = await UserLoginRecordService.GetByUserIdAsync(userId);
                return Ok(dto);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (SqlException ex)
            {
                if (ex.Number == 50002)
                    return NotFound(new { Message = $"User login record with user id [{userId}] not found." });

                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Message = "An error occurred while fetching the user login record.",
                    Details = ex.Message
                });
            }
        }
    }
}