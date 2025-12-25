using BankSystem.DAL.Helpers;
using BankSystem.Domain.Models;
using BankSystem.DTOs.ClientTransferRecordDTOs;
using BankSystem.DTOs.UserLoginRecordDTOs;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem.DAL.Data
{
    public class UserLoginRecordData
    {
        //Private Properties (Stored Procedure Name) :-
        private const string UserLoginRecords_GetAll = "UserLoginRecords.GetAll";
        
        private const string UserLoginRecords_GetById = "UserLoginRecords.GetById";
        private const string UserLoginRecords_GetByUserId = "UserLoginRecords.GetByUserId";

        private const string UserLoginRecords_ExistsById = "UserLoginRecords.ExistsById";
        private const string UserLoginRecords_AddNew = "UserLoginRecords.AddNew";


        //Get All User Login Records :-
        public static async Task<List<UserLoginRecordReadDetailsDTO>> GetAllAsync(int pageNumber, int pageSize)
        {
            var loginRecords = new List<UserLoginRecordReadDetailsDTO>();

            await using var connection = await DatabaseHelper.GetOpenConnectionAsync();
            await using var command = new SqlCommand(UserLoginRecords_GetAll, connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            //Input Parameters :-
            command.Parameters.Add(DatabaseHelper.CreateParameter($"@PageNumber", SqlDbType.Int, pageNumber));
            command.Parameters.Add(DatabaseHelper.CreateParameter($"@PageSize", SqlDbType.Int, pageSize));

            await using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                loginRecords.Add(new UserLoginRecordReadDetailsDTO
                (
                    reader.GetInt32("LoginID"),
                    DatabaseHelper.GetDateTimeOrNull(reader, "DateTime"),
                    DatabaseHelper.GetStringOrNull(reader, "FirstName"),
                    DatabaseHelper.GetStringOrNull(reader, "LastName"),
                    DatabaseHelper.GetStringOrNull(reader, "UserName"),
                    DatabaseHelper.GetStringOrNull(reader, "CountryName"),
                    DatabaseHelper.GetStringOrNull(reader, "Email"),
                    DatabaseHelper.GetStringOrNull(reader, "Phone"),
                    reader.GetInt32("PermissionLevel")
                ));
            }
            return loginRecords;
        }


        //User Login Record Exists :-
        public static async Task<bool> ExistsAsync(int loginId)
        {
            await using var connection = await DatabaseHelper.GetOpenConnectionAsync();
            await using var command = new SqlCommand(UserLoginRecords_ExistsById, connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            //Input Parameters :-
            command.Parameters.Add(DatabaseHelper.CreateParameter($"@LoginID", SqlDbType.Int, loginId));

            //Output Parameters :-
            var result = DatabaseHelper.CreateOutputParameter($"@result", SqlDbType.Bit);
            command.Parameters.Add(result);

            await command.ExecuteNonQueryAsync();
            return (bool)(result.Value ?? false);
        }


        //Get User Login Record By Id :-
        public static async Task<UserLoginRecord?> GetByIdAsync(int loginId)
        {
            await using var connection = await DatabaseHelper.GetOpenConnectionAsync();
            await using var command = new SqlCommand(UserLoginRecords_GetById, connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            //Input Parameters :-
            command.Parameters.Add(DatabaseHelper.CreateParameter($"@LoginID", SqlDbType.Int, loginId));
            await using var reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return new UserLoginRecord
                (
                   reader.GetInt32("LoginID"),
                    reader.GetInt32("UserID"),
                    reader.GetDateTime("DateTime")
                );
            }
            return null;
        }
        public static async Task<List<UserLoginRecordReadBasicDTO>?> GetByUserIdAsync(int userId)
        {
            var LoginRecords = new List<UserLoginRecordReadBasicDTO>();

            await using var connection = await DatabaseHelper.GetOpenConnectionAsync();
            await using var command = new SqlCommand(UserLoginRecords_GetByUserId, connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            //Input Parameters :-
            command.Parameters.Add(DatabaseHelper.CreateParameter($"@UserID", SqlDbType.Int, userId));
            await using var reader = await command.ExecuteReaderAsync();


            while (await reader.ReadAsync())
            {
                LoginRecords.Add(new UserLoginRecordReadBasicDTO
                (
                    reader.GetInt32("LoginID"),
                    reader.GetInt32("UserID"),
                    reader.GetDateTime("DateTime")
                ));
            }
            return (LoginRecords.Count > 0) ? LoginRecords : null;
        }


        //Create User Login Record :-
        public static async Task<int> CreateAsync(UserLoginRecord userLoginRecord)
        {
            await using var connection = await DatabaseHelper.GetOpenConnectionAsync();
            await using var command = new SqlCommand(UserLoginRecords_AddNew, connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            //Input Parameters :-
            command.Parameters.Add(DatabaseHelper.CreateParameter($"@UserID", SqlDbType.Int, userLoginRecord.UserID));

            //Output Parameters :-
            var outputId = DatabaseHelper.CreateOutputParameter("@NewLoginID", SqlDbType.Int);
            command.Parameters.Add(outputId);

            await command.ExecuteNonQueryAsync();
            return (int)(outputId.Value ?? 0);
        }
    }
}