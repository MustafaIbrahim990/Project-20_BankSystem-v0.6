using BankSystem.DAL.Helpers;
using BankSystem.Domain.Models;
using BankSystem.DTOs.ClientTransferRecordDTOs;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem.DAL.Data
{
    public static class ClientTransferRecordData
    {
        //Private Properties (Stored Procedure Name) :-
        private const string ClientTransferRecords_GetAll = "ClientTransferRecords.GetAll";

        private const string ClientTransferRecords_GetById = "ClientTransferRecords.GetById";
        private const string ClientTransferRecords_GetByClientId = "ClientTransferRecords.GetByClientId";

        private const string ClientTransferRecords_ExistsById = "ClientTransferRecords.ExistsById";
        private const string ClientTransferRecords_AddNew = "ClientTransferRecords.AddNew";


        //Get All Client Transfer Records :-
        public static async Task<List<ClientTransferRecordReadDetailsDTO>> GetAllAsync(int pageNumber, int pageSize)
        {
            var transferRecords = new List<ClientTransferRecordReadDetailsDTO>();

            await using var connection = await DatabaseHelper.GetOpenConnectionAsync();
            await using var command = new SqlCommand(ClientTransferRecords_GetAll, connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            //Input Parameters :-
            command.Parameters.Add(DatabaseHelper.CreateParameter($"@PageNumber", SqlDbType.Int, pageNumber));
            command.Parameters.Add(DatabaseHelper.CreateParameter($"@PageSize", SqlDbType.Int, pageSize));

            await using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                transferRecords.Add(new ClientTransferRecordReadDetailsDTO
                (
                    reader.GetInt32("TransferID"),
                    DatabaseHelper.GetDateTimeOrNull(reader, "DateTime"),
                    DatabaseHelper.GetStringOrNull(reader, "FromClientName"),
                    DatabaseHelper.GetStringOrNull(reader, "ToClientName"),
                    DatabaseHelper.GetDecimalOrNull(reader, "TransferAmount"),
                    DatabaseHelper.GetDecimalOrNull(reader, "FromBalanceAfterTransfer"),
                    DatabaseHelper.GetDecimalOrNull(reader, "ToBalanceAfterTransfer"),
                    DatabaseHelper.GetStringOrNull(reader, "ByUserName")
                ));
            }
            return transferRecords;
        }


        //Transfer Record Exists :-
        public static async Task<bool> ExistsByIdAsync(int transferId)
        {
            await using var connection = await DatabaseHelper.GetOpenConnectionAsync();
            await using var command = new SqlCommand(ClientTransferRecords_ExistsById, connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            //Input Parameters :-
            command.Parameters.Add(DatabaseHelper.CreateParameter($"@TransferID", SqlDbType.Int, transferId));

            //Output Parameters :-
            var result = DatabaseHelper.CreateOutputParameter($"@result", SqlDbType.Bit);
            command.Parameters.Add(result);

            await command.ExecuteNonQueryAsync();
            return (bool)(result.Value ?? false);
        }


        //Get Transfer Record By Id :-
        public static async Task<ClientTransferRecordReadDetailsDTO?> GetByIdAsync(int transferId)
        {
            await using var connection = await DatabaseHelper.GetOpenConnectionAsync();
            await using var command = new SqlCommand(ClientTransferRecords_GetById, connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            //Input Parameters :-
            command.Parameters.Add(DatabaseHelper.CreateParameter($"@TransferID", SqlDbType.Int, transferId));
            await using var reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return new ClientTransferRecordReadDetailsDTO
                (
                    reader.GetInt32("TransferID"),
                    DatabaseHelper.GetDateTimeOrNull(reader, "DateTime"),
                    DatabaseHelper.GetStringOrNull(reader, "FromClientName"),
                    DatabaseHelper.GetStringOrNull(reader, "ToClientName"),
                    DatabaseHelper.GetDecimalOrNull(reader, "TransferAmount"),
                    DatabaseHelper.GetDecimalOrNull(reader, "FromBalanceAfterTransfer"),
                    DatabaseHelper.GetDecimalOrNull(reader, "ToBalanceAfterTransfer"),
                    DatabaseHelper.GetStringOrNull(reader, "ByUserName")
                );
            }
            return null;
        }
        public static async Task<List<ClientTransferRecordReadDetailsDTO>?> GetByClientIdAsync(int clientId)
        {
            var transferRecords = new List<ClientTransferRecordReadDetailsDTO>();

            await using var connection = await DatabaseHelper.GetOpenConnectionAsync();
            await using var command = new SqlCommand(ClientTransferRecords_GetByClientId, connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            //Input Parameters :-
            command.Parameters.Add(DatabaseHelper.CreateParameter($"@ClientID", SqlDbType.Int, clientId));

            await using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                transferRecords.Add(new ClientTransferRecordReadDetailsDTO
                (
                    reader.GetInt32("TransferID"),
                    DatabaseHelper.GetDateTimeOrNull(reader, "DateTime"),
                    DatabaseHelper.GetStringOrNull(reader, "FromClientName"),
                    DatabaseHelper.GetStringOrNull(reader, "ToClientName"),
                    DatabaseHelper.GetDecimalOrNull(reader, "TransferAmount"),
                    DatabaseHelper.GetDecimalOrNull(reader, "FromBalanceAfterTransfer"),
                    DatabaseHelper.GetDecimalOrNull(reader, "ToBalanceAfterTransfer"),
                    DatabaseHelper.GetStringOrNull(reader, "ByUserName")
                ));
            }
            return (transferRecords.Count > 0) ? transferRecords : null;
        }


        //Create Transfer :-
        public static async Task<int> CreateAsync(ClientTransferRecord clientTransferRecord)
        {
            await using var connection = await DatabaseHelper.GetOpenConnectionAsync();
            await using var command = new SqlCommand(ClientTransferRecords_AddNew, connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            //Input Parameters :-
            command.Parameters.Add(DatabaseHelper.CreateParameter($"@FromClientID", SqlDbType.Int, clientTransferRecord.FromClientID));
            command.Parameters.Add(DatabaseHelper.CreateParameter($"@ToClientID", SqlDbType.Int, clientTransferRecord.ToClientID));
            command.Parameters.Add(DatabaseHelper.CreateParameter($"@TransferAmount", SqlDbType.Decimal, clientTransferRecord.TransferAmount));
            command.Parameters.Add(DatabaseHelper.CreateParameter($"@ByUserID", SqlDbType.Int, clientTransferRecord.ByUserID));

            //Output Parameters :-
            var outputId = DatabaseHelper.CreateOutputParameter("@NewTransferID", SqlDbType.Int);
            command.Parameters.Add(outputId);

            await command.ExecuteNonQueryAsync();
            return (int)(outputId.Value ?? 0);
        }
    }
}