using BankSystem.BLL.Services;
using BankSystem.DTOs.ClientDTOs;
using BankSystem.DTOs.UserDTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace BankSystem.API.Controllers
{
    [Route("api/Users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        //Get All Users :-
        [HttpGet()]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<UserReadDetailsDTO>>> GetAllUsersAsync(int pageNumber = 1, int pageSize = 10)
        {
            if (pageNumber <= 0)
                return BadRequest("Page number must be a positive.");

            if (pageSize <= 0)
                return BadRequest("Page size must be a positive.");

            try
            {
                var users = await UserService.GetAllAsync(pageNumber, pageSize);
                return Ok(users);
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
                    Message = $"An error occurred while fetching the users.",
                    Details = ex.Message
                });
            }
        }


        //Does User Exist By Id :-
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("exists-by-user-id/{id:int:min(1)}")]
        public async Task<ActionResult<bool>> ExistsAsync(int id)
        {
            if (id <= 0)
                return BadRequest(new { Message = "User Id must be a positive number." });

            try
            {
                return Ok(await UserService.ExistsAsync(id));
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


        //Does User Exist By Username :-
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("exists-by-username/{username:required}")]
        public async Task<ActionResult<bool>> ExistsAsync(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                return BadRequest(new { Message = "Username is required." });

            try
            {
                return Ok(await UserService.ExistsAsync(username));
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


        //Does User Exist By Username and Password :-
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("exists-by-username/{username:required}/password")]
        public async Task<ActionResult<bool>> ExistsAsync(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username))
                return BadRequest(new { Message = "Username is required." });

            if (string.IsNullOrWhiteSpace(password))
                return BadRequest(new { Message = "Password is required." });

            try
            {
                return Ok(await UserService.ExistsAsync(username, password));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (SqlException ex)
            {
                if (ex.Number == 50003)
                    return NotFound(new { Message = $"User with Username [{username}] not found." });

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


        //Get User Info By ID :-
        [HttpGet("by-user-id/{id:int:min(1)}", Name = "GetUserByIdAsync")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<UserReadBasicDTO>> GetByIdAsync(int id)
        {
            if (id <= 0)
                return BadRequest(new { Message = "User Id must be a positive number." });

            try
            {
                UserReadBasicDTO dto = await UserService.GetAsync(id);
                return Ok(dto);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (SqlException ex)
            {
                if (ex.Number == 50003)
                    return NotFound(new { Message = $"User with ID [{id}] not found." });

                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Message = "An error occurred while fetching the user.",
                    Details = ex.Message
                });
            }
        }


        //Get User Info By Username and Password :-
        [HttpGet("by-username/{username:required}", Name = "GetUserByUsernameAndPasswordAsync")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<UserReadBasicDTO>> GetByUsernameAndPasswordAsync(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username))
                return BadRequest(new { Message = "Username is required." });

            if (string.IsNullOrWhiteSpace(password))
                return BadRequest(new { Message = "Password is required." });

            try
            {
                UserReadBasicDTO dto = await UserService.GetAsync(username, password);
                return Ok(dto);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (SqlException ex)
            {
                if (ex.Number == 50003)
                    return NotFound(new { Message = $"User with Username [{username}] not found." });

                if (ex.Number == 50004)
                    return NotFound(new { Message = $"Incorrect Username Or Password." });

                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Message = "An error occurred while fetching the user.",
                    Details = ex.Message
                });
            }
        }


        //Create User :-
        [HttpPost()]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<UserReadBasicDTO>> CreateAsync(UserCreateDTO newUserDTO)
        {
            if (newUserDTO == null)
                return BadRequest(new { Message = "Invalid user data." });

            try
            {
                UserReadBasicDTO dto = await UserService.CreateAsync(newUserDTO);
                return CreatedAtRoute("GetUserByIdAsync", new { id = dto.UserID }, dto);
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
                    return NotFound(new { Message = $"Person with ID [{newUserDTO.PersonID}] not found." });

                if (ex.Number == 50004)
                    return BadRequest(new { Message = $"User with username [{newUserDTO.UserName}] already in use." });

                if (ex.Number == 50006)
                    return BadRequest(new { Message = $"An error occurred while processing the password." });

                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Message = "An error occurred while adding the user.",
                    Details = ex.Message
                });
            }
        }


        //Update User By Id :-
        [HttpPut("{id:int:min(1)}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<UserReadBasicDTO>> UpdateAsync(int id, UserUpdateDTO updatedUserDTO)
        {
            if (id <= 0)
                return BadRequest(new { Message = "User Id must be a positive number." });

            if (updatedUserDTO == null)
                return BadRequest(new { Message = "Invalid user data." });

            try
            {
                UserReadBasicDTO dto = await UserService.UpdateAsync(id, updatedUserDTO);
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
                    return BadRequest(new { Message = $"User with Id [{id}] not found." });

                if (ex.Number == 50004)
                    return BadRequest(new { Message = $"User with Username [{updatedUserDTO.UserName}] already in use." });

                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Message = "An error occurred while updating the user.",
                    Details = ex.Message
                });
            }
        }


        //Change PIN Code By Id :-
        [HttpPut("change-password")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<bool>> UpdatePasswordByIdAsync(UpdatePasswordDTO dto)
        {
            if (dto == null) 
                return BadRequest(new { Message = "Invalid user data." });

            try
            {
                return Ok(await UserService.UpdatePasswordAsync(dto));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (SqlException ex)
            {
                if (ex.Number == 50003)
                    return NotFound(new { Message = $"User with username [{dto.UserName}] not found." });

                if (ex.Number == 50004)
                    return NotFound(new { Message = $"Incorrect Username or Password." });

                if (ex.Number == 50006)
                    return NotFound(new { Message = $"An error occurred while processing the password." });

                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Message = "An error occurred while changing the password of user.",
                    Details = ex.Message
                });
            }
        }


        //Delete User By Id :-
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpDelete("by-client-id/{id:int:min(1)}")]
        public async Task<ActionResult<bool>> DeleteAsync(int id)
        {
            if (id <= 0)
                return BadRequest(new { Message = "User Id must be a positive number." });

            try
            {
                return Ok(await UserService.DeleteAsync(id));
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
                    return NotFound(new { Message = $"User with Id [{id}] not found." });

                if (ex.Number == 50003)
                    return BadRequest(new { Message = $"Cannot delete user : this user is linked to active login records." });

                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Message = "An error occurred while deleting the user.",
                    Details = ex.Message
                });
            }
        }


        //Delete User By Username and Password :-
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpDelete("by-username/{username:required}")]
        public async Task<ActionResult<bool>> DeleteAsync(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username))
                return BadRequest(new { Message = "Username is required." });

            if (string.IsNullOrWhiteSpace(password))
                return BadRequest(new { Message = "Password is required." });

            try
            {
                return Ok(await UserService.DeleteAsync(username, password));
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
                if (ex.Number == 50003)
                    return NotFound(new { Message = $"User with username [{username}] not found." });

                if (ex.Number == 50004)
                    return NotFound(new { Message = "Incorrect UserName or Password." });

                if (ex.Number == 50005)
                    return BadRequest(new { Message = "Cannot delete user : this user is linked to active login records." });

                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Message = "An error occurred while deleting the user.",
                    Details = ex.Message
                });
            }
        }


        //Is Valid User Credentials :-
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("verify-Credentials/{username:required}")]
        public async Task<ActionResult<ClientReadBasicDTO>> VerifyUserCredentialsAsync(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username))
                return BadRequest(new { Message = "Username is required." });

            if (string.IsNullOrWhiteSpace(password))
                return BadRequest(new { Message = "Password is required." });

            try
            {
                return Ok(await UserService.VerifyUserCredentialsAsync(username, password));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (SqlException ex)
            {
                if (ex.Number == 50003)
                    return NotFound(new { Message = $"User with username [{username}] not found." });

                if (ex.Number == 50004)
                    return BadRequest(new { Message = "Incorrect Username Or Password." });

                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Message = "An error occurred while verifing credentials of user.",
                    Details = ex.Message
                });
            }
        }
    }
}