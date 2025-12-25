using BankSystem.DAL.Helpers;
using BankSystem.DTOs.ClientDTOs;
using Microsoft.Data.SqlClient;
using BankSystem.Domain.Models;
using System;
using System.Data;

namespace BankSystem.DAL.Data
{
    public static class ClientData
    {
        //Private Properties (Stored Procedure Name) :-
        private const string Clients_GetAll = "Clients.GetAll";
        private const string Clients_GetById = "Clients.GetById";
        private const string Clients_GetByAccountNumber = "Clients.GetByAccountNumber";
        private const string Clients_GetByPersonId = "Clients.GetByPersonId";
        private const string Clients_GetTotalBalances = "Clients.GetTotalBalances";

        private const string Clients_AddNew = "Clients.AddNew";
        private const string Clients_UpdatePinCodeByAccountNumberAndPinCode = "Clients.UpdatePinCodeByAccountNumberAndPinCode";
        private const string Clients_DeleteById = "Clients.DeleteById";

        private const string Clients_ExistsById = "Clients.ExistsById";
        private const string Clients_ExistsByIdAndPinCode = "Clients.ExistsByIdAndPinCode";
        private const string Clients_ExistsByAccountNumber = "Clients.ExistsByAccountNumber";
        private const string Clients_ExistsByAccountNumberForUpdate = "Clients.ExistsByAccountNumberForUpdate";
        private const string Clients_ExistsByAccountNumberAndPinCode = "Clients.ExistsByAccountNumberAndPinCode";
        private const string Clients_ExistsByPersonId = "Clients.ExistsByPersonId";
        private const string Clients_HasTransferRecordById = "Clients.HasTransferRecordById";

        private const string Clients_DepositByAccountNumberAndPinCode = "Clients.DepositByAccountNumberAndPinCode";
        private const string Clients_WithdrawByAccountNumberAndPinCode = "Clients.WithdrawByAccountNumberAndPinCode";


        //Get All :-
        public static async Task<List<ClientReadDetailsDTO>> GetAllAsync(int pageNumber, int pageSize)
        {
            var clients = new List<ClientReadDetailsDTO>();

            await using var connection = await DatabaseHelper.GetOpenConnectionAsync();
            await using var command = new SqlCommand(Clients_GetAll, connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            //Input Parameters :-
            command.Parameters.Add(DatabaseHelper.CreateParameter($"@PageNumber", SqlDbType.Int, pageNumber));
            command.Parameters.Add(DatabaseHelper.CreateParameter($"@PageSize", SqlDbType.Int, pageSize));

            await using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                clients.Add(new ClientReadDetailsDTO
                (
                    reader.GetInt32("ClientID"),
                    DatabaseHelper.GetStringOrNull(reader, "FirstName"),
                    DatabaseHelper.GetStringOrNull(reader, "LastName"),
                    DatabaseHelper.GetStringOrNull(reader, "CountryName"),
                    DatabaseHelper.GetStringOrNull(reader, "Email"),
                    DatabaseHelper.GetStringOrNull(reader, "Phone"),
                    DatabaseHelper.GetStringOrNull(reader, "AccountNumber"),
                    DatabaseHelper.GetDecimalOrNull(reader, "AccountBalance")
                ));
            }
            return clients;
        }


        //Does Exist :-
        public static async Task<bool> ExistsByIdAsync(int clientId)
        {
            await using var connection = await DatabaseHelper.GetOpenConnectionAsync();
            await using var command = new SqlCommand(Clients_ExistsById, connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            //Input Parameters :-
            command.Parameters.Add(DatabaseHelper.CreateParameter($"@ClientID", SqlDbType.Int, clientId));

            //Output Parameters :-
            var result = DatabaseHelper.CreateOutputParameter($"@result", SqlDbType.Bit);
            command.Parameters.Add(result);

            await command.ExecuteNonQueryAsync();
            return (bool)(result.Value ?? false);
        }
        public static async Task<bool> ExistsByIdAndPinCodeAsync(int clientId, string PinCodeHashed)
        {
            await using var connection = await DatabaseHelper.GetOpenConnectionAsync();
            await using var command = new SqlCommand(Clients_ExistsByIdAndPinCode, connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            //Input Parameters :-
            command.Parameters.Add(DatabaseHelper.CreateParameter($"@ClientID", SqlDbType.Int, clientId));
            command.Parameters.Add(DatabaseHelper.CreateParameter($"@PinCodeHashed", SqlDbType.NVarChar, PinCodeHashed));

            //Output Parameters :-
            var result = DatabaseHelper.CreateOutputParameter($"@result", SqlDbType.Bit);
            command.Parameters.Add(result);

            await command.ExecuteNonQueryAsync();
            return (bool)(result.Value ?? false);
        }
        public static async Task<bool> ExistsByAccountNumberAsync(string accountNumber)
        {
            await using var connection = await DatabaseHelper.GetOpenConnectionAsync();
            await using var command = new SqlCommand(Clients_ExistsByAccountNumber, connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            //Input Parameters :-
            command.Parameters.Add(DatabaseHelper.CreateParameter($"@AccountNumber", SqlDbType.NVarChar, accountNumber));

            //Output Parameters :-
            var result = DatabaseHelper.CreateOutputParameter($"@result", SqlDbType.Bit);
            command.Parameters.Add(result);

            await command.ExecuteNonQueryAsync();
            return (bool)(result.Value ?? false);
        }
        public static async Task<bool> ExistsByAccountNumberForUpdateAsync(int clientId, string accountNumber)
        {
            await using var connection = await DatabaseHelper.GetOpenConnectionAsync();
            await using var command = new SqlCommand(Clients_ExistsByAccountNumberForUpdate, connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            //Input Parameters :-
            command.Parameters.Add(DatabaseHelper.CreateParameter($"@ClientID", SqlDbType.Int, clientId));
            command.Parameters.Add(DatabaseHelper.CreateParameter($"@AccountNumber", SqlDbType.NVarChar, accountNumber));

            //Output Parameters :-
            var result = DatabaseHelper.CreateOutputParameter($"@result", SqlDbType.Bit);
            command.Parameters.Add(result);

            await command.ExecuteNonQueryAsync();
            return (bool)(result.Value ?? false);
        }
        public static async Task<bool> ExistsByAccountNumberAndPinCodeAsync(string accountNumber, string PinCodeHashed)
        {
            await using var connection = await DatabaseHelper.GetOpenConnectionAsync();
            await using var command = new SqlCommand(Clients_ExistsByAccountNumberAndPinCode, connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            //Input Parameters :-
            command.Parameters.Add(DatabaseHelper.CreateParameter($"@AccountNumber", SqlDbType.NVarChar, accountNumber));
            command.Parameters.Add(DatabaseHelper.CreateParameter($"@PinCodeHashed", SqlDbType.NVarChar, PinCodeHashed));

            //Output Parameters :-
            var result = DatabaseHelper.CreateOutputParameter($"@result", SqlDbType.Bit);
            command.Parameters.Add(result);

            await command.ExecuteNonQueryAsync();
            return (bool)(result.Value ?? false);
        }
        public static async Task<bool> ExistsByPersonIdAsync(int personId)
        {
            await using var connection = await DatabaseHelper.GetOpenConnectionAsync();
            await using var command = new SqlCommand(Clients_ExistsByPersonId, connection)
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
        public static async Task<Client?> GetByIdAsync(int clientId)
        {
            await using var connection = await DatabaseHelper.GetOpenConnectionAsync();
            await using var command = new SqlCommand(Clients_GetById, connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            //Input Parameters :-
            command.Parameters.Add(DatabaseHelper.CreateParameter($"@ClientID", SqlDbType.Int, clientId));
            await using var reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return new Client
                (
                    reader.GetInt32("ClientID"),
                    reader.GetInt32("PersonID"),
                    reader.GetString("AccountNumber"),
                    reader.GetDecimal("AccountBalance"),
                    reader.GetString("PinCodeHashed"),
                    reader.GetString("Salt")
                );
            }
            return null;
        }
        public static async Task<Client?> GetByAccountNumberAsync(string accountNumber)
        {
            await using var connection = await DatabaseHelper.GetOpenConnectionAsync();
            await using var command = new SqlCommand(Clients_GetByAccountNumber, connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            //Input Parameters :-
            command.Parameters.Add(DatabaseHelper.CreateParameter($"@AccountNumber", SqlDbType.NVarChar, accountNumber));
            await using var reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return new Client
                (
                    reader.GetInt32("ClientID"),
                    reader.GetInt32("PersonID"),
                    reader.GetString("AccountNumber"),
                    reader.GetDecimal("AccountBalance"),
                    reader.GetString("PinCodeHashed"),
                    reader.GetString("Salt")
                );
            }
            return null;
        }
        public static async Task<Client?> GetByPersonIdAsync(int personId)
        {
            await using var connection = await DatabaseHelper.GetOpenConnectionAsync();
            await using var command = new SqlCommand(Clients_GetByPersonId, connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            //Input Parameters :-
            command.Parameters.Add(DatabaseHelper.CreateParameter($"@PersonID", SqlDbType.Int, personId));
            await using var reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return new Client
                (
                    reader.GetInt32("ClientID"),
                    reader.GetInt32("PersonID"),
                    reader.GetString("AccountNumber"),
                    reader.GetDecimal("AccountBalance"),
                    reader.GetString("PinCodeHashed"),
                    reader.GetString("Salt")
                );
            }
            return null;
        }
        

        //Create :-
        public static async Task<int> CreateAsync(Client client)
        {
            await using var connection = await DatabaseHelper.GetOpenConnectionAsync();
            await using var command = new SqlCommand(Clients_AddNew, connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            //Input Parameters :-
            command.Parameters.Add(DatabaseHelper.CreateParameter($"@PersonID", SqlDbType.NVarChar, client.PersonID));
            command.Parameters.Add(DatabaseHelper.CreateParameter($"@AccountNumber", SqlDbType.NVarChar, client.AccountNumber));
            command.Parameters.Add(DatabaseHelper.CreateParameter($"@AccountBalance", SqlDbType.Decimal, client.AccountBalance));
            command.Parameters.Add(DatabaseHelper.CreateParameter($"@PinCodeHashed", SqlDbType.NVarChar, client.PinCodeHashed));
            command.Parameters.Add(DatabaseHelper.CreateParameter($"@Salt", SqlDbType.NVarChar, client.Salt));

            //Output Parameters :-
            var outputId = DatabaseHelper.CreateOutputParameter("@NewClientID", SqlDbType.Int);
            command.Parameters.Add(outputId);

            await command.ExecuteNonQueryAsync();
            return (int)(outputId.Value ?? 0);
        }


        //Update :- 
        public static async Task<bool> UpdatePinCodeAsync(string accountNumber, string currentPinCodeHashed, string newPinCodeHashed, string newSalt)
        {
            await using var connection = await DatabaseHelper.GetOpenConnectionAsync();
            await using var command = new SqlCommand(Clients_UpdatePinCodeByAccountNumberAndPinCode, connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            //Input Parameters :-
            command.Parameters.Add(DatabaseHelper.CreateParameter($"@AccountNumber", SqlDbType.NVarChar, accountNumber));
            command.Parameters.Add(DatabaseHelper.CreateParameter($"@CurrentPinCodeHashed", SqlDbType.NVarChar, currentPinCodeHashed));
            command.Parameters.Add(DatabaseHelper.CreateParameter($"@NewPinCodeHashed", SqlDbType.NVarChar, newPinCodeHashed));
            command.Parameters.Add(DatabaseHelper.CreateParameter($"@NewSalt", SqlDbType.NVarChar, newSalt));

            //Output Parameters :-
            var result = DatabaseHelper.CreateOutputParameter("@result", SqlDbType.Bit);
            command.Parameters.Add(result);

            await command.ExecuteNonQueryAsync();
            return (bool)(result.Value ?? false);
        }


        //Delete :-
        public static async Task<bool> DeleteAsync(int clientId)
        {
            await using var connection = await DatabaseHelper.GetOpenConnectionAsync();
            await using var command = new SqlCommand(Clients_DeleteById, connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            //Input Parameters :-
            command.Parameters.Add(DatabaseHelper.CreateParameter($"@ClientID", SqlDbType.Int, clientId));

            //Output Parameters :-
            var result = DatabaseHelper.CreateOutputParameter("@result", SqlDbType.Bit);
            command.Parameters.Add(result);

            await command.ExecuteNonQueryAsync();
            return (bool)(result.Value ?? false);
        }


        //Get Total Clients Balance :-
        public static async Task<decimal> GetTotalClientsBalanceAsync()
        {
            await using var connection = await DatabaseHelper.GetOpenConnectionAsync();
            await using var command = new SqlCommand(Clients_GetTotalBalances, connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            var result = DatabaseHelper.CreateDecimalOutputParameter("@ClientsTotalBalance", SqlDbType.Decimal, 18, 2);
            command.Parameters.Add(result);

            await command.ExecuteNonQueryAsync();
            return (decimal)(result.Value ?? 0);
        }


        //Deposit :-
        public static async Task<bool> DepositAsync(string accountNumber, string pinCodeHashed, decimal amount)
        {
            await using var connection = await DatabaseHelper.GetOpenConnectionAsync();
            await using var command = new SqlCommand(Clients_DepositByAccountNumberAndPinCode, connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            //Input Parameters :-
            command.Parameters.Add(DatabaseHelper.CreateParameter($"@AccountNumber", SqlDbType.NVarChar, accountNumber));
            command.Parameters.Add(DatabaseHelper.CreateParameter($"@PinCodeHashed", SqlDbType.NVarChar, pinCodeHashed));
            command.Parameters.Add(DatabaseHelper.CreateParameter($"@DepositAmount", SqlDbType.Decimal, amount));

            //Output Parameters :-
            var result = DatabaseHelper.CreateOutputParameter($"@result", SqlDbType.Bit);
            command.Parameters.Add(result);

            await command.ExecuteNonQueryAsync();
            return (bool)(result.Value ?? false);
        }


        //Withdraw :-
        public static async Task<bool> WithdrawAsync(string accountNumber, string pinCodeHashed, decimal amount)
        {
            await using var connection = await DatabaseHelper.GetOpenConnectionAsync();
            await using var command = new SqlCommand(Clients_WithdrawByAccountNumberAndPinCode, connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            //Input Parameters :-
            command.Parameters.Add(DatabaseHelper.CreateParameter($"@AccountNumber", SqlDbType.NVarChar, accountNumber));
            command.Parameters.Add(DatabaseHelper.CreateParameter($"@PinCodeHashed", SqlDbType.NVarChar, pinCodeHashed));
            command.Parameters.Add(DatabaseHelper.CreateParameter($"@WithdrawAmount", SqlDbType.Decimal, amount));

            //Output Parameters :-
            var result = DatabaseHelper.CreateOutputParameter($"@result", SqlDbType.Bit);
            command.Parameters.Add(result);

            await command.ExecuteNonQueryAsync();
            return (bool)(result.Value ?? false);
        }


        //Has Transfer Record :-
        public static async Task<bool> HasTransferRecordByIdAsync(int clientId)
        {
            await using var connection = await DatabaseHelper.GetOpenConnectionAsync();
            await using var command = new SqlCommand(Clients_HasTransferRecordById, connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            //Input Parameters :-
            command.Parameters.Add(DatabaseHelper.CreateParameter($"@ClientID", SqlDbType.Int, clientId));

            //Output Parameters :-
            var result = DatabaseHelper.CreateOutputParameter($"@result", SqlDbType.Bit);
            command.Parameters.Add(result);

            await command.ExecuteNonQueryAsync();
            return (bool)(result.Value ?? false);
        }
    }
}