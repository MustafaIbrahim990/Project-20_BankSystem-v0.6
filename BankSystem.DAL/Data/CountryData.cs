using BankSystem.DAL.Helpers;
using BankSystem.DAL.Settings;
using BankSystem.Domain.Models;
using BankSystem.DTOs.ClientDTOs;
using BankSystem.DTOs.CountryDTOs;
using Microsoft.Data.SqlClient;
using System;
using System.Data;

namespace BankSystem.DAL.Data
{
    public static class CountryData
    {
        //Private Properties (Stored Procedure Name) :-
        private const string Countries_GetAll = "Countries.GetAll";
        private const string Countries_GetById = "Countries.GetById";
        private const string Countries_GetByName = "Countries.GetByName";
        private const string Countries_GetByCurrencyId = "Countries.GetByCurrencyId";
        private const string Countries_Create = "Countries.AddNew";
        private const string Countries_UpdateById = "Countries.UpdateById";
        private const string Countries_DeleteById = "Countries.DeleteById";
        private const string Countries_ExistsById = "Countries.ExistsById";
        private const string Countries_ExistsByName = "Countries.ExistsByName";
        private const string Countries_ExistsByNameForUpdate = "Countries.ExistsByNameForUpdate";
        private const string Countries_HasPeople = "Countries.HasPeopleById";


        //Get All :-
        public static async Task<List<CountryReadDetailsDTO>> GetAllAsync(int pageNumber, int pageSize)
        {
            var countries = new List<CountryReadDetailsDTO>();

            await using var connection = await DatabaseHelper.GetOpenConnectionAsync();
            await using var command = new SqlCommand(Countries_GetAll, connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            //Input Parameters :-
            command.Parameters.Add(DatabaseHelper.CreateParameter($"@PageNumber", SqlDbType.Int, pageNumber));
            command.Parameters.Add(DatabaseHelper.CreateParameter($"@PageSize", SqlDbType.Int, pageSize));

            await using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                countries.Add(new CountryReadDetailsDTO
                (
                    reader.GetInt32("CountryID"),
                    DatabaseHelper.GetStringOrNull(reader, "CountryName"),
                    DatabaseHelper.GetStringOrNull(reader, "CurrencyName"),
                    DatabaseHelper.GetStringOrNull(reader, "CurrencyCode"),
                    DatabaseHelper.GetDecimalOrNull(reader, "CurrencyRate")
                ));
            }
            return countries;
        }


        //Does Exist :-
        public static async Task<bool> ExistsAsync(int countryId)
        {
            await using var connection = await DatabaseHelper.GetOpenConnectionAsync();
            await using var command = new SqlCommand(Countries_ExistsById, connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            //Input Parameters :-
            command.Parameters.Add(DatabaseHelper.CreateParameter($"@CountryID", SqlDbType.Int, countryId));

            //Output Parameters :-
            var result = DatabaseHelper.CreateOutputParameter($"@result", SqlDbType.Bit);
            command.Parameters.Add(result);

            await command.ExecuteNonQueryAsync();
            return (bool)(result.Value ?? false);
        }
        public static async Task<bool> ExistsAsync(string countryName)
        {
            await using var connection = await DatabaseHelper.GetOpenConnectionAsync();
            await using var command = new SqlCommand(Countries_ExistsByName, connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            //Input Parameters :-
            command.Parameters.Add(DatabaseHelper.CreateParameter($"@CountryName", SqlDbType.NVarChar, countryName));

            //Output Parameters :-
            var result = DatabaseHelper.CreateOutputParameter($"@result", SqlDbType.Bit);
            command.Parameters.Add(result);

            await command.ExecuteNonQueryAsync();
            return (bool)(result.Value ?? false);
        }
        public static async Task<bool> ExistsAsync(int countryId, string countryName)
        {
            await using var connection = await DatabaseHelper.GetOpenConnectionAsync();
            await using var command = new SqlCommand(Countries_ExistsByNameForUpdate, connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            //Input Parameters :-
            command.Parameters.Add(DatabaseHelper.CreateParameter($"@CountryID", SqlDbType.Int, countryId));
            command.Parameters.Add(DatabaseHelper.CreateParameter($"@CountryName", SqlDbType.NVarChar, countryName));

            //Output Parameters :-
            var result = DatabaseHelper.CreateOutputParameter($"@result", SqlDbType.Bit);
            command.Parameters.Add(result);

            await command.ExecuteNonQueryAsync();
            return (bool)(result.Value ?? false);
        }


        //Get Info :-
        public static async Task<Country?> GetAsync(int countryId)
        {
            await using var connection = await DatabaseHelper.GetOpenConnectionAsync();
            await using var command = new SqlCommand(Countries_GetById, connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            //Input Parameters :-
            command.Parameters.Add(DatabaseHelper.CreateParameter($"@CountryID", SqlDbType.Int, countryId));
            await using var reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return new Country
                (
                    reader.GetInt32("CountryID"),
                    reader.GetString("CountryName"),
                    reader.GetInt32("CurrencyID")
                );
            }
            return null;
        }
        public static async Task<Country?> GetAsync(string countryName)
        {
            await using var connection = await DatabaseHelper.GetOpenConnectionAsync();
            await using var command = new SqlCommand(Countries_GetByName, connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            //Input Parameters :-
            command.Parameters.Add(DatabaseHelper.CreateParameter($"@CountryName", SqlDbType.NVarChar, countryName));
            await using var reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return new Country
                (
                    reader.GetInt32("CountryID"),
                    reader.GetString("CountryName"),
                    reader.GetInt32("CurrencyID")
                );
            }
            return null;
        }
        public static async Task<List<CountryReadDetailsDTO>> GetByCurrencyIdAsync(int currencyId)
        {
            var countries = new List<CountryReadDetailsDTO>();

            await using var connection = await DatabaseHelper.GetOpenConnectionAsync();
            await using var command = new SqlCommand(Countries_GetByCurrencyId, connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            //Input Parameters :-
            command.Parameters.Add(DatabaseHelper.CreateParameter($"@CurrencyID", SqlDbType.Int, currencyId));

            await using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                countries.Add(new CountryReadDetailsDTO
                (
                    reader.GetInt32("CountryID"),
                    DatabaseHelper.GetStringOrNull(reader, "CountryName"),
                    DatabaseHelper.GetStringOrNull(reader, "CurrencyName"),
                    DatabaseHelper.GetStringOrNull(reader, "CurrencyCode"),
                    DatabaseHelper.GetDecimalOrNull(reader, "CurrencyRate")
                ));
            }
            return countries;
        }


        //Create :-
        public static async Task<int> CreateAsync(Country country)
        {
            await using var connection = await DatabaseHelper.GetOpenConnectionAsync();
            await using var command = new SqlCommand(Countries_Create, connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            //Input Parameters :-
            command.Parameters.Add(DatabaseHelper.CreateParameter($"@CountryName", SqlDbType.NVarChar, country.CountryName));
            command.Parameters.Add(DatabaseHelper.CreateParameter($"@CurrencyID", SqlDbType.Int, country.CurrencyID));

            //Output Parameters :-
            var outputId = DatabaseHelper.CreateOutputParameter("@NewCountryID", SqlDbType.Int);
            command.Parameters.Add(outputId);

            await command.ExecuteNonQueryAsync();
            return (int)(outputId.Value ?? 0);
        }


        //Update :-
        public static async Task<bool> UpdateAsync(int id, Country country)
        {
            await using var connection = await DatabaseHelper.GetOpenConnectionAsync();
            await using var command = new SqlCommand(Countries_UpdateById, connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            //Input Parameters :-
            command.Parameters.Add(DatabaseHelper.CreateParameter($"@CountryID", SqlDbType.Int, id));
            command.Parameters.Add(DatabaseHelper.CreateParameter($"@CountryName", SqlDbType.NVarChar, country.CountryName));
            command.Parameters.Add(DatabaseHelper.CreateParameter($"@CurrencyID", SqlDbType.Int, country.CurrencyID));

            //Output Parameters :-
            var result = DatabaseHelper.CreateOutputParameter("@result", SqlDbType.Bit);
            command.Parameters.Add(result);

            await command.ExecuteNonQueryAsync();
            return (bool)(result.Value ?? false);
        }


        //Delete :-
        public static async Task<bool> DeleteAsync(int countryId)
        {
            await using var connection = await DatabaseHelper.GetOpenConnectionAsync();
            await using var command = new SqlCommand(Countries_DeleteById, connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            //Input Parameters :-
            command.Parameters.Add(DatabaseHelper.CreateParameter($"@CountryID", SqlDbType.Int, countryId));

            //Output Parameters :-
            var result = DatabaseHelper.CreateOutputParameter($"@result", SqlDbType.Bit);
            command.Parameters.Add(result);

            await command.ExecuteNonQueryAsync();
            return (bool)(result.Value ?? false);
        }


        //Has People :-
        public static async Task<bool> HasPeople(int countryId)
        {
            await using var connection = await DatabaseHelper.GetOpenConnectionAsync();
            await using var command = new SqlCommand(Countries_HasPeople, connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            //Input Parameters :-
            command.Parameters.Add(DatabaseHelper.CreateParameter($"@CountryID", SqlDbType.Int, countryId));

            //Output Parameters :-
            var result = DatabaseHelper.CreateOutputParameter($"@result", SqlDbType.Bit);
            command.Parameters.Add(result);

            await command.ExecuteNonQueryAsync();
            return (bool)(result.Value ?? false);
        }
    }
}