using BankSystem.DAL.Helpers;
using BankSystem.Domain.Models;
using BankSystem.DTOs.UserDTOs;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem.DAL.Data
{
    public class UserData
    {
        //Private Properties (Stored Procedure Name) :-
        private const string Users_GetAll = "Users.GetAll";
        private const string Users_GetById = "Users.GetById";
        private const string Users_GetByUsername = "Users.GetByUsername";
        private const string Users_GetByUsernameAndPassword = "Users.GetByUsernameAndPassword";
        private const string Users_GetByPersonId = "Users.GetByPersonId";

        private const string Users_AddNew = "Users.AddNew";
        private const string Users_UpdateById = "Users.UpdateById";
        private const string Users_UpdatePasswordByUsernameAndPassword = "Users.UpdatePasswordByUsernameAndPassword";
        private const string Users_DeleteById = "Users.DeleteById";

        private const string Users_ExistsById = "Users.ExistsById";
        private const string Users_ExistsByUsername = "Users.ExistsByUsername";
        private const string Users_ExistsByUsernameForUpdate = "Users.ExistsByUsernameForUpdate";
        private const string Users_ExistsByUsernameAndPassword = "Users.ExistsByUsernameAndPassword";
        private const string Users_ExistsByPersonId = "Users.ExistsByPersonId";

        private const string Users_HasLoginRecordsById = "Users.HasLoginRecordsById";
        private const string Users_HasClientTransferRecordById = "Users.HasClientTransferRecordById";


        //Get All :-
        public static async Task<List<UserReadDetailsDTO>> GetAllAsync(int pageNumber, int pageSize)
        {
            var users = new List<UserReadDetailsDTO>();

            await using var connection = await DatabaseHelper.GetOpenConnectionAsync();
            await using var command = new SqlCommand(Users_GetAll, connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            //Input Parameters :-
            command.Parameters.Add(DatabaseHelper.CreateParameter($"@PageNumber", SqlDbType.Int, pageNumber));
            command.Parameters.Add(DatabaseHelper.CreateParameter($"@PageSize", SqlDbType.Int, pageSize));

            await using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                users.Add(new UserReadDetailsDTO
                (
                    reader.GetInt32("UserID"),
                    DatabaseHelper.GetStringOrNull(reader, "UserName"),
                    DatabaseHelper.GetStringOrNull(reader, "FirstName"),
                    DatabaseHelper.GetStringOrNull(reader, "LastName"),
                    DatabaseHelper.GetStringOrNull(reader, "CountryName"),
                    DatabaseHelper.GetStringOrNull(reader, "Phone"),
                    DatabaseHelper.GetStringOrNull(reader, "Email"),
                    reader.GetInt32("PermissionLevel")
                ));
            }
            return users;
        }


        //Does Exist :-
        public static async Task<bool> ExistsByIdAsync(int userId)
        {
            await using var connection = await DatabaseHelper.GetOpenConnectionAsync();
            await using var command = new SqlCommand(Users_ExistsById, connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            //Input Parameters :-
            command.Parameters.Add(DatabaseHelper.CreateParameter($"@UserID", SqlDbType.Int, userId));

            //Output Parameters :-
            var result = DatabaseHelper.CreateOutputParameter($"@result", SqlDbType.Bit);
            command.Parameters.Add(result);

            await command.ExecuteNonQueryAsync();
            return (bool)(result.Value ?? false);
        }
        public static async Task<bool> ExistsByPersonIdAsync(int personId)
        {
            await using var connection = await DatabaseHelper.GetOpenConnectionAsync();
            await using var command = new SqlCommand(Users_ExistsByPersonId, connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            //Input Parameters :-
            command.Parameters.Add(DatabaseHelper.CreateParameter($"@PersonID", SqlDbType.Int, personId));

            //Output Parameters :-
            var result = DatabaseHelper.CreateOutputParameter($"@result", SqlDbType.Bit);
            command.Parameters.Add(result);

            await command.ExecuteNonQueryAsync();
            return (bool)(result.Value ?? false);
        }
        public static async Task<bool> ExistsByUsernameAsync(string userName)
        {
            await using var connection = await DatabaseHelper.GetOpenConnectionAsync();
            await using var command = new SqlCommand(Users_ExistsByUsername, connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            //Input Parameters :-
            command.Parameters.Add(DatabaseHelper.CreateParameter($"@UserName", SqlDbType.NVarChar, userName));

            //Output Parameters :-
            var result = DatabaseHelper.CreateOutputParameter($"@result", SqlDbType.Bit);
            command.Parameters.Add(result);

            await command.ExecuteNonQueryAsync();
            return (bool)(result.Value ?? false);
        }
        public static async Task<bool> ExistsByUsernameForUpdateAsync(int userId, string userName)
        {
            await using var connection = await DatabaseHelper.GetOpenConnectionAsync();
            await using var command = new SqlCommand(Users_ExistsByUsernameForUpdate, connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            //Input Parameters :-
            command.Parameters.Add(DatabaseHelper.CreateParameter($"@UserID", SqlDbType.Int, userId));
            command.Parameters.Add(DatabaseHelper.CreateParameter($"@UserName", SqlDbType.NVarChar, userName));

            //Output Parameters :-
            var result = DatabaseHelper.CreateOutputParameter($"@result", SqlDbType.Bit);
            command.Parameters.Add(result);

            await command.ExecuteNonQueryAsync();
            return (bool)(result.Value ?? false);
        }
        public static async Task<bool> ExistsByUsernameAndPasswordAsync(string username, string password)
        {
            await using var connection = await DatabaseHelper.GetOpenConnectionAsync();
            await using var command = new SqlCommand(Users_ExistsByUsernameAndPassword, connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            //Input Parameters :-
            command.Parameters.Add(DatabaseHelper.CreateParameter($"@UserName", SqlDbType.NVarChar, username));
            command.Parameters.Add(DatabaseHelper.CreateParameter($"@PasswordHashed", SqlDbType.NVarChar, password));

            //Output Parameters :-
            var result = DatabaseHelper.CreateOutputParameter($"@result", SqlDbType.Bit);
            command.Parameters.Add(result);

            await command.ExecuteNonQueryAsync();
            return (bool)(result.Value ?? false);
        }


        //Get Info :-
        public static async Task<User?> GetAsync(int userId)
        {
            await using var connection = await DatabaseHelper.GetOpenConnectionAsync();
            await using var command = new SqlCommand(Users_GetById, connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            //Input Parameters :-
            command.Parameters.Add(DatabaseHelper.CreateParameter($"@UserID", SqlDbType.Int, userId));
            await using var reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return new User
                (
                    reader.GetInt32("UserID"),
                    reader.GetInt32("PersonID"),
                    reader.GetString("UserName"),
                    reader.GetString("PasswordHashed"),
                    reader.GetString("Salt"),
                    reader.GetInt32("PermissionLevel")
                );
            }
            return null;
        }
        public static async Task<User?> GetByPersonIdAsync(int personId)
        {
            await using var connection = await DatabaseHelper.GetOpenConnectionAsync();
            await using var command = new SqlCommand(Users_GetByPersonId, connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            //Input Parameters :-
            command.Parameters.Add(DatabaseHelper.CreateParameter($"@PersonID", SqlDbType.Int, personId));
            await using var reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return new User
                (
                    reader.GetInt32("UserID"),
                    reader.GetInt32("PersonID"),
                    reader.GetString("UserName"),
                    reader.GetString("PasswordHashed"),
                    reader.GetString("Salt"),
                    reader.GetInt32("PermissionLevel")
                );
            }
            return null;
        }
        public static async Task<User?> GetAsync(string username)
        {
            await using var connection = await DatabaseHelper.GetOpenConnectionAsync();
            await using var command = new SqlCommand(Users_GetByUsername, connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            //Input Parameters :-
            command.Parameters.Add(DatabaseHelper.CreateParameter($"@UserName", SqlDbType.NVarChar, username));
            await using var reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return new User
                (
                    reader.GetInt32("UserID"),
                    reader.GetInt32("PersonID"),
                    reader.GetString("UserName"),
                    reader.GetString("PasswordHashed"),
                    reader.GetString("Salt"),
                    reader.GetInt32("PermissionLevel")
                );
            }
            return null;
        }
        public static async Task<User?> GetAsync(string username, string password)
        {
            await using var connection = await DatabaseHelper.GetOpenConnectionAsync();
            await using var command = new SqlCommand(Users_GetByUsernameAndPassword, connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            //Input Parameters :-
            command.Parameters.Add(DatabaseHelper.CreateParameter($"@UserName", SqlDbType.NVarChar, username));
            command.Parameters.Add(DatabaseHelper.CreateParameter($"@PasswordHashed", SqlDbType.NVarChar, password));
            await using var reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return new User
                (
                    reader.GetInt32("UserID"),
                    reader.GetInt32("PersonID"),
                    reader.GetString("UserName"),
                    reader.GetString("PasswordHashed"),
                    reader.GetString("Salt"),
                    reader.GetInt32("PermissionLevel")
                );
            }
            return null;
        }


        //Create :-
        public static async Task<int> CreateAsync(User user)
        {
            await using var connection = await DatabaseHelper.GetOpenConnectionAsync();
            await using var command = new SqlCommand(Users_AddNew, connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            //Input Parameters :-
            command.Parameters.Add(DatabaseHelper.CreateParameter($"@PersonID", SqlDbType.Int, user.PersonID));
            command.Parameters.Add(DatabaseHelper.CreateParameter($"@UserName", SqlDbType.NVarChar, user.UserName));
            command.Parameters.Add(DatabaseHelper.CreateParameter($"@PasswordHashed", SqlDbType.NVarChar, user.PasswordHashed));
            command.Parameters.Add(DatabaseHelper.CreateParameter($"@Salt", SqlDbType.NVarChar, user.Salt));
            command.Parameters.Add(DatabaseHelper.CreateParameter($"@PermissionLevel", SqlDbType.Int, user.PermissionLevel));

            //Output Parameters :-
            var outputId = DatabaseHelper.CreateOutputParameter("@NewUserID", SqlDbType.Int);
            command.Parameters.Add(outputId);

            await command.ExecuteNonQueryAsync();
            return (int)(outputId.Value ?? 0);
        }


        //Update :-
        public static async Task<bool> UpdateAsync(int id, User user)
        {
            await using var connection = await DatabaseHelper.GetOpenConnectionAsync();
            await using var command = new SqlCommand(Users_UpdateById, connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            //Input Parameters :-
            command.Parameters.Add(DatabaseHelper.CreateParameter($"@UserID", SqlDbType.Int, id));
            command.Parameters.Add(DatabaseHelper.CreateParameter($"@UserName", SqlDbType.NVarChar, user.UserName));
            command.Parameters.Add(DatabaseHelper.CreateParameter($"@PermissionLevel", SqlDbType.Int, user.PermissionLevel));

            //Output Parameters :-
            var result = DatabaseHelper.CreateOutputParameter("@result", SqlDbType.Bit);
            command.Parameters.Add(result);

            await command.ExecuteNonQueryAsync();
            return (bool)(result.Value ?? false);
        }
        public static async Task<bool> UpdatePasswordAsync(string username, string currentPassword, string newPasswordHashed, string newSalt)
        {
            await using var connection = await DatabaseHelper.GetOpenConnectionAsync();
            await using var command = new SqlCommand(Users_UpdatePasswordByUsernameAndPassword, connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            //Input Parameters :-
            command.Parameters.Add(DatabaseHelper.CreateParameter($"@UserName", SqlDbType.NVarChar, username));
            command.Parameters.Add(DatabaseHelper.CreateParameter($"@CurrentPasswordHashed", SqlDbType.NVarChar, currentPassword));
            command.Parameters.Add(DatabaseHelper.CreateParameter($"@NewPasswordHashed", SqlDbType.NVarChar, newPasswordHashed));
            command.Parameters.Add(DatabaseHelper.CreateParameter($"@NewSalt", SqlDbType.NVarChar, newSalt));

            //Output Parameters :-
            var result = DatabaseHelper.CreateOutputParameter("@result", SqlDbType.Bit);
            command.Parameters.Add(result);

            await command.ExecuteNonQueryAsync();
            return (bool)(result.Value ?? false);
        }


        //Delete :-
        public static async Task<bool> DeleteAsync(int userId)
        {
            await using var connection = await DatabaseHelper.GetOpenConnectionAsync();
            await using var command = new SqlCommand(Users_DeleteById, connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            //Input Parameters :-
            command.Parameters.Add(DatabaseHelper.CreateParameter($"@UserID", SqlDbType.Int, userId));

            //Output Parameters :-
            var result = DatabaseHelper.CreateOutputParameter("@result", SqlDbType.Bit);
            command.Parameters.Add(result);

            await command.ExecuteNonQueryAsync();
            return (bool)(result.Value ?? false);
        }


        //Has Login Records :-
        public static async Task<bool> HasLoginRecordsById(int userId)
        {
            await using var connection = await DatabaseHelper.GetOpenConnectionAsync();
            await using var command = new SqlCommand(Users_HasLoginRecordsById, connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            //Input Parameters :-
            command.Parameters.Add(DatabaseHelper.CreateParameter($"@UserID", SqlDbType.Int, userId));

            //Output Parameters :-
            var result = DatabaseHelper.CreateOutputParameter($"@result", SqlDbType.Bit);
            command.Parameters.Add(result);

            await command.ExecuteNonQueryAsync();
            return (bool)(result.Value ?? false);
        }


        //Has Client Transfer Records :-
        public static async Task<bool> HasClientTransferRecordById(int userId)
        {
            await using var connection = await DatabaseHelper.GetOpenConnectionAsync();
            await using var command = new SqlCommand(Users_HasClientTransferRecordById, connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            //Input Parameters :-
            command.Parameters.Add(DatabaseHelper.CreateParameter($"@UserID", SqlDbType.Int, userId));

            //Output Parameters :-
            var result = DatabaseHelper.CreateOutputParameter($"@result", SqlDbType.Bit);
            command.Parameters.Add(result);

            await command.ExecuteNonQueryAsync();
            return (bool)(result.Value ?? false);
        }
    }
}