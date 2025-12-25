using BankSystem.BLL.Services;
using BankSystem.DTOs.ClientDTOs;
using BankSystem.DTOs.ClientTransferRecordDTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace BankSystem.API.Controllers
{
    [Route("api/ClientTransferRecords")]
    [ApiController]
    public class ClientTransferRecordsController : ControllerBase
    {
        //Get All Client Transfer Records :-
        [HttpGet()]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<ClientReadDetailsDTO>>> GetAllClientTransferRecordsAsync(int pageNumber = 1, int pageSize = 10)
        {
            if (pageNumber <= 0)
                return BadRequest("Page Number must be a positive.");

            if (pageSize <= 0)
                return BadRequest("Page Size must be a positive.");

            try
            {
                var transferRecords = await ClientTransferRecordService.GetAllAsync(pageNumber, pageSize);
                return Ok(transferRecords);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Message = $"An error occurred while fetching the client transfer records.",
                    Details = ex.Message
                });
            }
        }


        //Does Client Transfer Record Exist By Id :-
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("exists-by/{id:int:min(1)}")]
        public async Task<ActionResult<bool>> ExistsAsync(int id)
        {
            if (id <= 0)
                return BadRequest(new { Message = "Transfer Id must be a positive number." });

            try
            {
                return Ok(await ClientTransferRecordService.ExistsAsync(id));
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


        //Get Client Info By ID and PIN Code :-
        [HttpGet("by/{id:int:min(1)}", Name = "GetClientTransferRecordByIdAsync")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ClientTransferRecordReadDetailsDTO>> GetByIdAsync(int id)
        {
            if (id <= 0)
                return BadRequest(new { Message = "Transfer Id must be a positive number." });

            try
            {
                ClientTransferRecordReadDetailsDTO dto = await ClientTransferRecordService.GetByIdAsync(id);
                return Ok(dto);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (SqlException ex)
            {
                if (ex.Number == 50002)
                    return NotFound(new { Message = $"Client transfer record with Id [{id}] not found." });

                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Message = "An error occurred while fetching the client transfer record.",
                    Details = ex.Message
                });
            }
        }


        //Create Client :-
        [HttpPost()]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ClientTransferRecordReadDetailsDTO>> CreateAsync(ClientTransferRecordCreateDTO newDto)
        {
            if (newDto == null)
                return BadRequest(new { Message = "Invalid client transfer record data." });

            try
            {
                ClientTransferRecordReadDetailsDTO dto = await ClientTransferRecordService.CreateAsync(newDto);
                return CreatedAtRoute("GetClientTransferRecordAsync", new { id = dto.TransferID }, dto);
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
                    return NotFound(new { Message = $"Client with Id [{newDto.FromClientID}] not found." });

                if (ex.Number == 50004)
                    return NotFound(new { Message = $"Client with Id [{newDto.ToClientID}] not found." });

                if (ex.Number == 50005)
                    return BadRequest(new { Message = "Transfer cannot be made to same account." });

                if (ex.Number == 50006)
                    return NotFound(new { Message = $"User with Id [{newDto.ByUserID}] not found." });

                if (ex.Number == 50007)
                    return NotFound(new { Message = "Insufficient balance in source account." });

                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Message = "An error occurred while adding the client transfer record.",
                    Details = ex.Message
                });
            }
        }
    }
}