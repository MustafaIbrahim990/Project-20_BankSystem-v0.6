using BankSystem.DAL.Helpers;
using BankSystem.Domain.Models;
using BankSystem.DTOs.CurrencyDTOs;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem.DAL.Data
{
    public static class CurrencyData
    {
        //Private Properties (Stored Procedure Name) :-
        private const string Currencies_GetAll = "Currencies.GetAll";
        private const string Currencies_GetById = "Currencies.GetById";
        private const string Currencies_GetByCode = "Currencies.GetByCode";
        private const string Currencies_Create = "Currencies.AddNew";
        private const string Currencies_UpdateById = "Currencies.UpdateById";
        private const string Currencies_UpdateRateById = "Currencies.UpdateRateById";
        private const string Currencies_DeleteById = "Currencies.DeleteById";
                             
        private const string Currencies_ExistsById = "Currencies.ExistsById";
        private const string Currencies_ExistsByCode = "Currencies.ExistsByCode";
        private const string Currencies_ExistsByCodeForUpdate = "Currencies.ExistsByCodeForUpdate";
        private const string Currencies_ExistsByName = "Currencies.ExistsByName";
        private const string Currencies_ExistsByNameForUpdate = "Currencies.ExistsByNameForUpdate";
        private const string Currencies_HasCountriesById = "Currencies.HasCountriesById";


        //Get All :-
        public static async Task<List<CurrencyReadDTO>> GetAllAsync(int pageNumber, int pageSize)
        {
            var currencies = new List<CurrencyReadDTO>();

            await using var connection = await DatabaseHelper.GetOpenConnectionAsync();
            await using var command = new SqlCommand(Currencies_GetAll, connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            //Input Parameters :-
            command.Parameters.Add(DatabaseHelper.CreateParameter($"@PageNumber", SqlDbType.Int, pageNumber));
            command.Parameters.Add(DatabaseHelper.CreateParameter($"@PageSize", SqlDbType.Int, pageSize));

            await using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                currencies.Add(new CurrencyReadDTO
                (
                    reader.GetInt32("CurrencyID"),
                    reader.GetString("CurrencyName"),
                    reader.GetString("CurrencyCode"),
                    reader.GetDecimal("CurrencyRate")
                ));
            }
            return currencies;
        }


        //Does Exist :-
        public static async Task<bool> ExistsByIdAsync(int currencyId)
        {
            await using var connection = await DatabaseHelper.GetOpenConnectionAsync();
            await using var command = new SqlCommand(Currencies_ExistsById, connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            //Input Parameters :-
            command.Parameters.Add(DatabaseHelper.CreateParameter($"@CurrencyID", SqlDbType.Int, currencyId));

            //Output Parameters :-
            var result = DatabaseHelper.CreateOutputParameter($"@result", SqlDbType.Bit);
            command.Parameters.Add(result);

            await command.ExecuteNonQueryAsync();
            return (bool)(result.Value ?? false);
        }
        public static async Task<bool> ExistsByCodeAsync(string currencyCode)
        {
            await using var connection = await DatabaseHelper.GetOpenConnectionAsync();
            await using var command = new SqlCommand(Currencies_ExistsByCode, connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            //Input Parameters :-
            command.Parameters.Add(DatabaseHelper.CreateParameter($"@CurrencyCode", SqlDbType.NVarChar, currencyCode));

            //Output Parameters :-
            var result = DatabaseHelper.CreateOutputParameter($"@result", SqlDbType.Bit);
            command.Parameters.Add(result);

            await command.ExecuteNonQueryAsync();
            return (bool)(result.Value ?? false);
        }
        public static async Task<bool> ExistsByCodeForUpdateAsync(int currencyId, string currencyCode)
        {
            await using var connection = await DatabaseHelper.GetOpenConnectionAsync();
            await using var command = new SqlCommand(Currencies_ExistsByCodeForUpdate, connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            //Input Parameters :-
            command.Parameters.Add(DatabaseHelper.CreateParameter($"@CurrencyID", SqlDbType.Int, currencyId));
            command.Parameters.Add(DatabaseHelper.CreateParameter($"@CurrencyCode", SqlDbType.NVarChar, currencyCode));

            //Output Parameters :-
            var result = DatabaseHelper.CreateOutputParameter($"@result", SqlDbType.Bit);
            command.Parameters.Add(result);

            await command.ExecuteNonQueryAsync();
            return (bool)(result.Value ?? false);
        }
        public static async Task<bool> ExistsByNameAsync(string currencyName)
        {
            await using var connection = await DatabaseHelper.GetOpenConnectionAsync();
            await using var command = new SqlCommand(Currencies_ExistsByName, connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            //Input Parameters :-
            command.Parameters.Add(DatabaseHelper.CreateParameter($"@CurrencyName", SqlDbType.NVarChar, currencyName));

            //Output Parameters :-
            var result = DatabaseHelper.CreateOutputParameter($"@result", SqlDbType.Bit);
            command.Parameters.Add(result);

            await command.ExecuteNonQueryAsync();
            return (bool)(result.Value ?? false);
        }
        public static async Task<bool> ExistsByNameForUpdateAsync(int currencyId, string currencyName)
        {
            await using var connection = await DatabaseHelper.GetOpenConnectionAsync();
            await using var command = new SqlCommand(Currencies_ExistsByNameForUpdate, connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            //Input Parameters :-
            command.Parameters.Add(DatabaseHelper.CreateParameter($"@CurrencyID", SqlDbType.Int, currencyId));
            command.Parameters.Add(DatabaseHelper.CreateParameter($"@CurrencyName", SqlDbType.NVarChar, currencyName));

            //Output Parameters :-
            var result = DatabaseHelper.CreateOutputParameter($"@result", SqlDbType.Bit);
            command.Parameters.Add(result);

            await command.ExecuteNonQueryAsync();
            return (bool)(result.Value ?? false);
        }


        //Get Info :-
        public static async Task<Currency?> GetAsync(int currencyId)
        {
            await using var connection = await DatabaseHelper.GetOpenConnectionAsync();
            await using var command = new SqlCommand(Currencies_GetById, connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            //Input Parameters :-
            command.Parameters.Add(DatabaseHelper.CreateParameter($"@CurrencyID", SqlDbType.Int, currencyId));
            await using var reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return new Currency
                (
                    reader.GetInt32("CurrencyID"),
                    reader.GetString("CurrencyName"),
                    reader.GetString("CurrencyCode"),
                    reader.GetDecimal("CurrencyRate")
                );
            }
            return null;
        }
        public static async Task<Currency?> GetAsync(string currencyCode)
        {
            await using var connection = await DatabaseHelper.GetOpenConnectionAsync();
            await using var command = new SqlCommand(Currencies_GetByCode, connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            //Input Parameters :-
            command.Parameters.Add(DatabaseHelper.CreateParameter($"@CurrencyCode", SqlDbType.NVarChar, currencyCode));
            await using var reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return new Currency
                (
                    reader.GetInt32("CurrencyID"),
                    reader.GetString("CurrencyName"),
                    reader.GetString("CurrencyCode"),
                    reader.GetDecimal("CurrencyRate")
                );
            }
            return null;
        }


        //Create :-
        public static async Task<int> CreateAsync(Currency currency)
        {
            await using var connection = await DatabaseHelper.GetOpenConnectionAsync();
            await using var command = new SqlCommand(Currencies_Create, connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            //Input Parameters :-
            command.Parameters.Add(DatabaseHelper.CreateParameter($"@CurrencyName", SqlDbType.NVarChar, currency.CurrencyName));
            command.Parameters.Add(DatabaseHelper.CreateParameter($"@CurrencyCode", SqlDbType.NVarChar, currency.CurrencyCode));
            command.Parameters.Add(DatabaseHelper.CreateParameter($"@CurrencyRate", SqlDbType.Decimal, currency.CurrencyRate));

            //Output Parameters :-
            var outputId = DatabaseHelper.CreateOutputParameter("@NewCurrencyID", SqlDbType.Int);
            command.Parameters.Add(outputId);

            await command.ExecuteNonQueryAsync();
            return (int)(outputId.Value ?? 0);
        }


        //Update :-
        public static async Task<bool> UpdateAsync(int id, Currency currency)
        {
            await using var connection = await DatabaseHelper.GetOpenConnectionAsync();
            await using var command = new SqlCommand(Currencies_UpdateById, connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            //Input Parameters :-
            command.Parameters.Add(DatabaseHelper.CreateParameter($"@CurrencyID", SqlDbType.Int, id));
            command.Parameters.Add(DatabaseHelper.CreateParameter($"@CurrencyName", SqlDbType.NVarChar, currency.CurrencyName));
            command.Parameters.Add(DatabaseHelper.CreateParameter($"@CurrencyCode", SqlDbType.NVarChar, currency.CurrencyCode));
            command.Parameters.Add(DatabaseHelper.CreateParameter($"@CurrencyRate", SqlDbType.Decimal, currency.CurrencyRate));

            //Output Parameters :-
            var result = DatabaseHelper.CreateOutputParameter("@result", SqlDbType.Bit);
            command.Parameters.Add(result);

            await command.ExecuteNonQueryAsync();
            return (bool)(result.Value ?? false);
        }
        public static async Task<bool> UpdateRateAsync(int id, decimal newCurrencyRate)
        {
            await using var connection = await DatabaseHelper.GetOpenConnectionAsync();
            await using var command = new SqlCommand(Currencies_UpdateRateById, connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            //Input Parameters :-
            command.Parameters.Add(DatabaseHelper.CreateParameter($"@CurrencyID", SqlDbType.Int, id));
            command.Parameters.Add(DatabaseHelper.CreateParameter($"@NewCurrencyRate", SqlDbType.Decimal, newCurrencyRate));

            //Output Parameters :-
            var result = DatabaseHelper.CreateOutputParameter("@result", SqlDbType.Bit);
            command.Parameters.Add(result);

            await command.ExecuteNonQueryAsync();
            return (bool)(result.Value ?? false);
        }


        //Delete :-
        public static async Task<bool> DeleteAsync(int currencyId)
        {
            await using var connection = await DatabaseHelper.GetOpenConnectionAsync();
            await using var command = new SqlCommand(Currencies_DeleteById, connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            //Input Parameters :-
            command.Parameters.Add(DatabaseHelper.CreateParameter($"@CurrencyID", SqlDbType.Int, currencyId));

            //Output Parameters :-
            var result = DatabaseHelper.CreateOutputParameter($"@result", SqlDbType.Bit);
            command.Parameters.Add(result);

            await command.ExecuteNonQueryAsync();
            return (bool)(result.Value ?? false);
        }


        //Has Countries :-
        public static async Task<bool> HasCountriesById(int currencyId)
        {
            await using var connection = await DatabaseHelper.GetOpenConnectionAsync();
            await using var command = new SqlCommand(Currencies_HasCountriesById, connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            //Input Parameters :-
            command.Parameters.Add(DatabaseHelper.CreateParameter($"@CurrencyID", SqlDbType.Int, currencyId));

            //Output Parameters :-
            var result = DatabaseHelper.CreateOutputParameter($"@result", SqlDbType.Bit);
            command.Parameters.Add(result);

            await command.ExecuteNonQueryAsync();
            return (bool)(result.Value ?? false);
        }
    }
}