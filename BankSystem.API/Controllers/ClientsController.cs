using BankSystem.BLL.Helpers;
using BankSystem.BLL.Services;
using BankSystem.DTOs.ClientDTOs;
using BankSystem.DTOs.PersonDTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace BankSystem.API.Controllers
{
    [Route("api/Clients")]
    [ApiController]
    public class ClientsController : ControllerBase
    {
        //Get All Clients :-
        [HttpGet()]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<ClientReadDetailsDTO>>> GetAllClientsAsync(int pageNumber = 1, int pageSize = 10)
        {
            if (pageNumber <= 0)
                return BadRequest("Page Number must be a positive.");

            if (pageSize <= 0)
                return BadRequest("Page Size must be a positive.");

            try
            {
                var clients = await ClientService.GetAllAsync(pageNumber, pageSize);
                return Ok(clients);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Message = $"An error occurred while fetching the clients.",
                    Details = ex.Message
                });
            }
        }


        //Does Client Exist By Id :-
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("exists-by-client-id/{id:int:min(1)}")]
        public async Task<ActionResult<bool>> ExistsAsync(int id)
        {
            if (id <= 0)
                return BadRequest(new { Message = "Client Id must be a positive number." });

            try
            {
                return Ok(await ClientService.ExistsByIdAsync(id));
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


        //Does Client Exist By AccountNumber :-
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("exists-by-account-number/{accountNumber:required}")]
        public async Task<ActionResult<bool>> ExistsAsync(string accountNumber)
        {
            if (string.IsNullOrWhiteSpace(accountNumber))
                return BadRequest(new { Message = "Account Number is required." });

            try
            {
                return Ok(await ClientService.ExistsByAccountNumberAsync(accountNumber));
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
        [HttpGet("by-client-id/{id:int:min(1)}", Name = "GetClientByIdAndPinCodeAsync")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ClientReadBasicDTO>> GetByIdAndPinCodeAsync(int id, string pinCode)
        {
            if (id <= 0)
                return BadRequest(new { Message = "Client Id must be a positive number." });

            if (string.IsNullOrWhiteSpace(pinCode))
                return BadRequest(new { Message = "PIN Code is required." });

            try
            {
                ClientReadBasicDTO dto = await ClientService.GetByIdAndPinCodeAsync(id, pinCode);
                return Ok(dto);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (SqlException ex)
            {
                if (ex.Number == 50003)
                    return NotFound(new { Message = $"Client with Id [{id}] not found." });

                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Message = "An error occurred while fetching the client.",
                    Details = ex.Message
                });
            }
        }


        //Get Client Info By AccountNumber and PIN Code :-
        [HttpGet("by-account-number/{accountNumber:required}", Name = "GetClientByAccountNumberAsync")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ClientReadBasicDTO>> GetByAccountNumberAndPinCodeAsync(string accountNumber, string pinCode)
        {
            if (string.IsNullOrWhiteSpace(accountNumber))
                return BadRequest(new { Message = "Account Number is required." });

            if (string.IsNullOrWhiteSpace(pinCode))
                return BadRequest(new { Message = "PIN Code is required." });

            try
            {
                ClientReadBasicDTO dto = await ClientService.GetByAccountNumberAndPinCodeAsync(accountNumber, pinCode);
                return Ok(dto);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (SqlException ex)
            {
                if (ex.Number == 50003)
                    return NotFound(new { Message = $"Client with account number [{accountNumber}] not found." });

                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Message = "An error occurred while fetching the client.",
                    Details = ex.Message
                });
            }
        }


        //Create Client :-
        [HttpPost()]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ClientReadBasicDTO>> CreateAsync(ClientCreateDTO newClientDTO)
        {
            if (newClientDTO == null)
                return BadRequest(new { Message = "Invalid client data." });

            try
            {
                ClientReadBasicDTO dto = await ClientService.CreateAsync(newClientDTO);
                return CreatedAtRoute("GetClientByIdAndPinCodeAsync", new { id = dto.ClientID }, dto);
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
                    return NotFound(new { Message = $"Person with Id [{newClientDTO.PersonID}] not found." });

                if (ex.Number == 50003)
                    return BadRequest(new { Message = $"Client with account number [{newClientDTO.AccountNumber}] already exists." });

                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Message = "An error occurred while adding the client.",
                    Details = ex.Message
                });
            }
        }


        //Change PIN Code By Id :-
        [HttpPut("change-pin-code-by-client-id")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<bool>> UpdatePinCodeAsync(UpdatePinCodeByClientIdDTO dto)
        {
            if (dto.ClientID <= 0)
                return BadRequest(new { Message = "Client Id must be a positive number." });

            if (string.IsNullOrWhiteSpace(dto.CurrentPinCode))
                return BadRequest(new { Message = "Current PIN Code is required." });

            if (string.IsNullOrWhiteSpace(dto.NewPinCode))
                return BadRequest(new { Message = "New PIN Code is required." });

            try
            {
                return Ok(await ClientService.UpdatePinCodeAsync(dto));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (SqlException ex)
            {
                if (ex.Number == 50002)
                    return NotFound(new { Message = $"Client with Id [{dto.ClientID}] not found." });

                if (ex.Number == 50004)
                    return BadRequest(new { Message = $"Incorrect Client Id Or PIN Code." });

                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Message = "An error occurred while changing the PIN Code.",
                    Details = ex.Message
                });
            }
        }


        //Change PIN Code By Account Number :-
        [HttpPut("change-pin-code-by-account-number")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<bool>> UpdatePinCodeAsync(UpdatePinCodeByAccountNumberDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.AccountNumber))
                return BadRequest(new { Message = "Account Number is required." });

            if (string.IsNullOrWhiteSpace(dto.CurrentPinCode))
                return BadRequest(new { Message = "Current PIN Code is required." });

            if (string.IsNullOrWhiteSpace(dto.NewPinCode))
                return BadRequest(new { Message = "New PIN Code is required." });

            try
            {
                return Ok(await ClientService.UpdatePinCodeAsync(dto));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (SqlException ex)
            {
                if (ex.Number == 50002)
                    return NotFound(new { Message = $"Client with account number [{dto.AccountNumber}] not found." });

                if (ex.Number == 50004)
                    return BadRequest(new { Message = $"Incorrect Account Number Or PIN Code." });

                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Message = "An error occurred while changing the PIN Code.",
                    Details = ex.Message
                });
            }
        }


        //Delete Client By Id :-
        [HttpDelete("by-client-id/{id:int:min(1)}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<bool>> DeleteAsync(int id, string pinCode)
        {
            if (id <= 0)
                return BadRequest(new { Message = "Client Id must be a positive number." });

            if (string.IsNullOrWhiteSpace(pinCode))
                return BadRequest(new { Message = "PIN Code is required." });

            try
            {
                return Ok(await ClientService.DeleteAsync(id, pinCode));
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
                    return NotFound(new { Message = $"Client with Id [{id}] not found." });

                if (ex.Number == 50003)
                    return NotFound(new { Message = $"Incorrect Client Id Or PIN Code." });

                if (ex.Number == 50004)
                    return NotFound(new { Message = $"Cannot delete client : this client is linked to an active transfer records." });

                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Message = "An error occurred while deleting the client.",
                    Details = ex.Message
                });
            }
        }


        //Delete Client By Account Number :-
        [HttpDelete("by-account-number/{accountNumber:required}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<bool>> DeleteAsync(string accountNumber, string pinCode)
        {
            if (string.IsNullOrWhiteSpace(accountNumber))
                return BadRequest(new { Message = "Account Number is required." });

            if (string.IsNullOrWhiteSpace(pinCode))
                return BadRequest(new { Message = "PIN Code is required." });

            try
            {
                return Ok(await ClientService.DeleteAsync(accountNumber, pinCode));
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
                    return NotFound(new { Message = $"Client with account number [{accountNumber}] not found." });

                if (ex.Number == 50003)
                    return NotFound(new { Message = $"Incorrect Account Number Or PIN Code." });

                if (ex.Number == 50004)
                    return NotFound(new { Message = $"Cannot delete client : this client is linked to an active transfer records." });

                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Message = "An error occurred while deleting the client.",
                    Details = ex.Message
                });
            }
        }


        //Get Total Clients Balance :-
        [HttpGet("total-clients-balance")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<decimal>> GetTotalClientsBalanceAsync()
        {
            try
            {
                return Ok(await ClientService.GetTotalClientsBalanceAsync());
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Message = $"An error occurred while fetching ths total clients balance.",
                    Details = ex.Message
                });
            }
        }


        //Deposit By ClientID :- 
        [HttpPut("deposit-by-client-id")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<bool>> DepositAsync(DepositByClientIdDTO dto)
        {
            if (dto.ClientID <= 0)
                return BadRequest(new { Message = "Client Id must be a positive number." });

            if (string.IsNullOrWhiteSpace(dto.PinCode))
                return BadRequest(new { Message = "PIN Code is required." });

            if (dto.Amount <= 0)
                return BadRequest(new { Message = "Deposit Amount cannot be a negative number." });

            try
            {
                return Ok(await ClientService.DepositAsync(dto));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (SqlException ex)
            {
                if (ex.Number == 50002)
                    return NotFound(new { Message = $"Client with Id [{dto.ClientID}] not found." });

                if (ex.Number == 50003)
                    return BadRequest(new { Message = $"Incorrect Client Id Or PIN Code." });

                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Message = $"An error occurred while depositing the amount.",
                    Details = ex.Message 
                });
            }
        }


        //Deposit By AccountNumber :-
        [HttpPut("deposit-by-account-number")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<bool>> DepositAsync(DepositByAccountNumberDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.AccountNumber))
                return BadRequest(new { Message = "Account Number is required." });

            if (string.IsNullOrWhiteSpace(dto.PinCode))
                return BadRequest(new { Message = "PIN Code is required." });

            if (dto.Amount <= 0)
                return BadRequest(new { Message = "Deposit Amount cannot be a negative number." });

            try
            {
                return Ok(await ClientService.DepositAsync(dto));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (SqlException ex)
            {
                if (ex.Number == 50002)
                    return NotFound(new { Message = $"Client with account number [{dto.AccountNumber}] not found." });

                if (ex.Number == 50003)
                    return BadRequest(new { Message = $"Incorrect Account Number Or PIN Code." });

                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Message = $"An error occurred while depositing the amount.",
                    Details = ex.Message
                });
            }
        }


        //Withdraw By ClientID :-
        [HttpPut("withdraw-by-client-id")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<bool>> WithdrawAsync(WithdrawByClientIdDTO dto)
        {
            if (dto.ClientID <= 0)
                return BadRequest(new { Message = "Client Id must be a positive number." });

            if (string.IsNullOrWhiteSpace(dto.PinCode))
                return BadRequest(new { Message = "PIN Code is required." });

            if (dto.Amount <= 0)
                return BadRequest(new { Message = "Withdraw Amount cannot be a negative number." });

            try
            {
                return Ok(await ClientService.WithdrawAsync(dto));
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
                    return NotFound(new { Message = $"Client with Id [{dto.ClientID}] not found." });

                if (ex.Number == 50003)
                    return NotFound(new { Message = $"Incorrect Client Id Or PIN Code." });

                if (ex.Number == 50004)
                    return NotFound(new { Message = $"Insufficient balance to complete the withdrawal." });

                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new 
                {
                    Message = $"An error occurred while withdrawing the amount.",
                    Details = ex.Message
                });
            }
        }


        //Withdraw By AccountNumber :-
        [HttpPut("withdraw-by-account-number")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<bool>> WithdrawAsync(WithdrawByAccountNumberDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.AccountNumber))
                return BadRequest(new { Message = "Account Number is required." });

            if (string.IsNullOrWhiteSpace(dto.PinCode))
                return BadRequest(new { Message = "PIN Code is required." });

            if (dto.Amount <= 0)
                return BadRequest(new { Message = "Withdraw Amount cannot be a negative number." });

            try
            {
                return Ok(await ClientService.WithdrawAsync(dto));
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
                    return NotFound(new { Message = $"Client with account number [{dto.AccountNumber}] not found." });

                if (ex.Number == 50003)
                    return NotFound(new { Message = $"Incorrect Account Number Or PIN Code." });

                if (ex.Number == 50004)
                    return NotFound(new { Message = $"Insufficient balance to complete the withdrawal." });

                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Message = $"An error occurred while withdrawing the amount.",
                    Details = ex.Message
                });
            }
        }


        //Is Valid Client Credentials by Client Id :-
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("credentials-Valid-by-client-id/{clientId:int:min(1)}")]
        public async Task<ActionResult<bool>> IsValidClientCredentialsAsync(int clientId, string pinCode)
        {
            if (clientId <= 0)
                return BadRequest(new { Message = "Client Id must be a positive number." });

            if (string.IsNullOrWhiteSpace(pinCode))
                return BadRequest(new { Message = "PIN Code is required." });

            try
            {
                return Ok(await ClientService.IsClientCredentialsValidAsync(clientId, pinCode));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (SqlException ex)
            {
                if (ex.Number == 50002)
                    return NotFound(new { Message = $"Client with Client Id [{clientId}] not found." });

                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Message = "An error occurred while checking client credentials.",
                    Details = ex.Message
                });
            }
        }


        //Is Valid Client Credentials by Account Number :-
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("credentials-Valid-by-account-number/{accountNumber:required}")]
        public async Task<ActionResult<bool>> IsValidClientCredentialsAsync(string accountNumber, string pinCode)
        {
            if (string.IsNullOrWhiteSpace(accountNumber))
                return BadRequest(new { Message = "Account Number is required." });

            if (string.IsNullOrWhiteSpace(pinCode))
                return BadRequest(new { Message = "PIN Code is required." });

            try
            {
                return Ok(await ClientService.IsClientCredentialsValidAsync(accountNumber, pinCode));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (SqlException ex)
            {
                if (ex.Number == 50002)
                    return NotFound(new { Message = $"Client with account number [{accountNumber}] not found." });

                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Message = "An error occurred while checking client credentials.",
                    Details = ex.Message
                });
            }
        }


        //Verify Client Credentials by Account Number :-
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("verify-credentials-by-client-id/{clientId:int:min(1)}")]
        public async Task<ActionResult<ClientReadBasicDTO>> VerifyClientCredentialsAsync(int clientId, string pinCode)
        {
            if (clientId <= 0)
                return BadRequest(new { Message = "Client Id must be a positive number." });

            if (string.IsNullOrWhiteSpace(pinCode))
                return BadRequest(new { Message = "PIN Code is required." });

            try
            {
                return Ok(await ClientService.VerifyClientCredentialsAsync(clientId, pinCode));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (SqlException ex)
            {
                if (ex.Number == 50002)
                    return NotFound(new { Message = $"Client with client id [{clientId}] not found." });

                if (ex.Number == 50004)
                    return BadRequest(new { Message = "Incorrect Client Id Or PIN Code." });

                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Message = "An error occurred while verifing client credentials.",
                    Details = ex.Message
                });
            }
        }


        //Verify Client Credentials by Account Number :-
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("verify-credentials-by-account-number/{accountNumber:required}")]
        public async Task<ActionResult<ClientReadBasicDTO>> VerifyClientCredentialsAsync(string accountNumber, string pinCode)
        {
            if (string.IsNullOrWhiteSpace(accountNumber))
                return BadRequest(new { Message = "Account Number is required." });

            if (string.IsNullOrWhiteSpace(pinCode))
                return BadRequest(new { Message = "PIN Code is required." });

            try
            {
                return Ok(await ClientService.VerifyClientCredentialsAsync(accountNumber, pinCode));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (SqlException ex)
            {
                if (ex.Number == 50002)
                    return NotFound(new { Message = $"Client with account number [{accountNumber}] not found." });

                if (ex.Number == 50004)
                    return BadRequest(new { Message = "Incorrect account number or PIN Code." });

                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Message = "An error occurred while verifing client credentials.",
                    Details = ex.Message
                });
            }
        }
    }
}