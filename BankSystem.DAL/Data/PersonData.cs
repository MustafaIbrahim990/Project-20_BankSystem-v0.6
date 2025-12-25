using System;
using System.Data;
using Microsoft.Data.SqlClient;
using BankSystem.DAL.Helpers;
using BankSystem.DTOs.PersonDTOs;
using BankSystem.Domain.Models;

namespace BankSystem.DAL.Data
{
    public class PersonData
    {
        //Private Properties (Stored Procedure Name) :-
        private const string People_GetAll = "People.GetAll";
        private const string People_GetById = "People.GetById";
        private const string People_Create = "People.AddNew";
        private const string People_UpdateById = "People.UpdateById";
        private const string People_DeleteById = "People.DeleteById";
        
        private const string People_ExistsById = "People.ExistsById";
        private const string People_ExistsByEmail = "People.ExistsByEmail";
        private const string People_ExistsByEmailForUpdate = "People.ExistsByEmailForUpdate";
        private const string People_ExistsByPhone = "People.ExistsByPhone";
        private const string People_ExistsByPhoneForUpdate = "People.ExistsByPhoneForUpdate";
        private const string People_IsClient = "People.IsClient";
        private const string People_IsUser = "People.IsUser";


        //Get All :-
        public static async Task<List<PersonReadDetailsDTO>> GetAllAsync(int pageNumber, int pageSize)
        {
            var people = new List<PersonReadDetailsDTO>();

            await using var connection = await DatabaseHelper.GetOpenConnectionAsync();
            await using var command = new SqlCommand(People_GetAll, connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            //Input Parameters :-
            command.Parameters.Add(DatabaseHelper.CreateParameter($"@PageNumber", SqlDbType.Int, pageNumber));
            command.Parameters.Add(DatabaseHelper.CreateParameter($"@PageSize", SqlDbType.Int, pageSize));

            await using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                people.Add(new PersonReadDetailsDTO
                (
                    reader.GetInt32("PersonID"),
                    DatabaseHelper.GetStringOrNull(reader, "FirstName"),
                    DatabaseHelper.GetStringOrNull(reader, "LastName"),
                    DatabaseHelper.GetStringOrNull(reader, "CountryName"),
                    DatabaseHelper.GetStringOrNull(reader, "CurrencyName"),
                    DatabaseHelper.GetStringOrNull(reader, "CurrencyCode"),
                    DatabaseHelper.GetDecimalOrNull(reader, "CurrencyRate"),
                    DatabaseHelper.GetStringOrNull(reader, "Email"),
                    DatabaseHelper.GetStringOrNull(reader, "Phone")
                ));
            }
            return people;
        }


        //Does Exist :-
        public static async Task<bool> ExistsByIdAsync(int personID)
        {
            await using var connection = await DatabaseHelper.GetOpenConnectionAsync();
            await using var command = new SqlCommand(People_ExistsById, connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            //Input Parameters :-
            command.Parameters.Add(DatabaseHelper.CreateParameter($"@PersonID", SqlDbType.Int, personID));

            //Output Parameters :-
            var result = DatabaseHelper.CreateOutputParameter($"@result", SqlDbType.Bit);
            command.Parameters.Add(result);

            await command.ExecuteNonQueryAsync();
            return (bool)(result.Value ?? false);
        }


        //Does Exists By Email :-
        public static async Task<bool> ExistsByEmailAsync(string email)
        {
            await using var connection = await DatabaseHelper.GetOpenConnectionAsync();
            await using var command = new SqlCommand(People_ExistsByEmail, connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            //Input Parameters :-
            command.Parameters.Add(DatabaseHelper.CreateParameter($"@Email", SqlDbType.NVarChar, email));

            //Output Parameters :-
            var result = DatabaseHelper.CreateOutputParameter("@result", SqlDbType.Bit);
            command.Parameters.Add(result);

            await command.ExecuteNonQueryAsync();
            return (bool)(result.Value ?? false);
        }
        public static async Task<bool> ExistsByEmailForUpdateAsync(int personId, string email)
        {
            await using var connection = await DatabaseHelper.GetOpenConnectionAsync();
            await using var command = new SqlCommand(People_ExistsByEmailForUpdate, connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            //Input Parameters :-
            command.Parameters.Add(DatabaseHelper.CreateParameter($"@PersonID", SqlDbType.Int, personId));
            command.Parameters.Add(DatabaseHelper.CreateParameter($"@Email", SqlDbType.NVarChar, email));

            //Output Parameters :-
            var result = DatabaseHelper.CreateOutputParameter("@result", SqlDbType.Bit);
            command.Parameters.Add(result);

            await command.ExecuteNonQueryAsync();
            return (bool)(result.Value ?? false);
        }


        //Does Exists By Phone :-
        public static async Task<bool> ExistsByPhoneAsync(string phone)
        {
            await using var connection = await DatabaseHelper.GetOpenConnectionAsync();
            await using var command = new SqlCommand(People_ExistsByPhone, connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            //Input Parameters :-
            command.Parameters.Add(DatabaseHelper.CreateParameter($"@Phone", SqlDbType.NVarChar, phone));

            //Output Parameters :-
            var result = DatabaseHelper.CreateOutputParameter("@result", SqlDbType.Bit);
            command.Parameters.Add(result);

            await command.ExecuteNonQueryAsync();
            return (bool)(result.Value ?? false);
        }
        public static async Task<bool> ExistsByPhoneForUpdateAsync(int personId, string phone)
        {
            await using var connection = await DatabaseHelper.GetOpenConnectionAsync();
            await using var command = new SqlCommand(People_ExistsByPhoneForUpdate, connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            //Input Parameters :-
            command.Parameters.Add(DatabaseHelper.CreateParameter($"@PersonID", SqlDbType.Int, personId));
            command.Parameters.Add(DatabaseHelper.CreateParameter($"@Phone", SqlDbType.NVarChar, phone));

            //Output Parameters :-
            var result = DatabaseHelper.CreateOutputParameter("@result", SqlDbType.Bit);
            command.Parameters.Add(result);

            await command.ExecuteNonQueryAsync();
            return (bool)(result.Value ?? false);
        }


        //People is Client :-
        public static async Task<bool> IsClientAsync(int personId)
        {
            await using var connection = await DatabaseHelper.GetOpenConnectionAsync();
            await using var command = new SqlCommand(People_IsClient, connection)
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


        //People is User :-
        public static async Task<bool> IsUserAsync(int personId)
        {
            await using var connection = await DatabaseHelper.GetOpenConnectionAsync();
            await using var command = new SqlCommand(People_IsUser, connection)
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


        //Get Info :-
        public static async Task<Person?> GetAsync(int personID)
        {
            await using var connection = await DatabaseHelper.GetOpenConnectionAsync();
            await using var command = new SqlCommand(People_GetById, connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            //Input Parameters :-
            command.Parameters.Add(DatabaseHelper.CreateParameter($"@PersonID", SqlDbType.Int, personID));
            await using var reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return new Person
                (
                    reader.GetInt32("PersonID"),
                    reader.GetString("FirstName"),
                    reader.GetString("LastName"),
                    reader.GetInt32("CountryID"),
                    reader.GetString("Email"),
                    reader.GetString("Phone")
                );
            }
            return null;
        }


        //Create :-
        public static async Task<int> CreateAsync(Person person)
        {
            await using var connection = await DatabaseHelper.GetOpenConnectionAsync();
            await using var command = new SqlCommand(People_Create, connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            //Input Parameters :-
            command.Parameters.Add(DatabaseHelper.CreateParameter($"@FirstName", SqlDbType.NVarChar, person.FirstName));
            command.Parameters.Add(DatabaseHelper.CreateParameter($"@LastName", SqlDbType.NVarChar, person.LastName));
            command.Parameters.Add(DatabaseHelper.CreateParameter($"@CountryID", SqlDbType.Int, person.CountryID));
            command.Parameters.Add(DatabaseHelper.CreateParameter($"@Email", SqlDbType.NVarChar, person.Email));
            command.Parameters.Add(DatabaseHelper.CreateParameter($"@Phone", SqlDbType.NVarChar, person.Phone));

            //Output Parameters :-
            var outputId = DatabaseHelper.CreateOutputParameter("@NewPersonID", SqlDbType.Int);
            command.Parameters.Add(outputId);

            await command.ExecuteNonQueryAsync();
            return (int)(outputId.Value ?? 0);
        }


        //Update :-
        public static async Task<bool> UpdateAsync(int id, Person person)
        {
            await using var connection = await DatabaseHelper.GetOpenConnectionAsync();
            await using var command = new SqlCommand(People_UpdateById, connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            //Input Parameters :-
            command.Parameters.Add(DatabaseHelper.CreateParameter($"@PersonID", SqlDbType.Int, id));
            command.Parameters.Add(DatabaseHelper.CreateParameter($"@FirstName", SqlDbType.NVarChar, person.FirstName));
            command.Parameters.Add(DatabaseHelper.CreateParameter($"@LastName", SqlDbType.NVarChar, person.LastName));
            command.Parameters.Add(DatabaseHelper.CreateParameter($"@CountryID", SqlDbType.Int, person.CountryID));
            command.Parameters.Add(DatabaseHelper.CreateParameter($"@Email", SqlDbType.NVarChar, person.Email));
            command.Parameters.Add(DatabaseHelper.CreateParameter($"@Phone", SqlDbType.NVarChar, person.Phone));

            //Output Parameters :-
            var result = DatabaseHelper.CreateOutputParameter("@result", SqlDbType.Bit);
            command.Parameters.Add(result);

            await command.ExecuteNonQueryAsync();
            return (bool)(result.Value ?? false);
        }


        //Delete :-
        public static async Task<bool> DeleteAsync(int personID)
        {
            await using var connection = await DatabaseHelper.GetOpenConnectionAsync();
            await using var command = new SqlCommand(People_DeleteById, connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            //Input Parameters :-
            command.Parameters.Add(DatabaseHelper.CreateParameter($"@PersonID", SqlDbType.Int, personID));

            //Output Parameters :-
            var result = DatabaseHelper.CreateOutputParameter("@result", SqlDbType.Bit);
            command.Parameters.Add(result);

            await command.ExecuteNonQueryAsync();
            return (bool)(result.Value ?? false);
        }
    }
}