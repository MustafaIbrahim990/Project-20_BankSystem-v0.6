using BankSystem.BLL.Helpers;
using BankSystem.BLL.Validations;
using BankSystem.DAL.Data;
using BankSystem.Domain.Models;
using BankSystem.DTOs.ClientDTOs;
using BankSystem.DTOs.ClientTransferRecordDTOs;
using BankSystem.DTOs.UserLoginRecordDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem.BLL.Services
{
    public class UserLoginRecordService
    {
        //Private Methods :-
        private static async Task<UserLoginRecord> GetUserLoginRecordInfo(int loginId)
        {
            await UserLoginRecordValidation.ValidateLoginIdAsync(loginId);
            UserLoginRecord? userLoginRecord = await UserLoginRecordData.GetByIdAsync(loginId);

            if (userLoginRecord == null)
                throw new ArgumentException($"User login record with id [{loginId}] not found.");

            return userLoginRecord;
        }
        private static async Task<List<UserLoginRecordReadBasicDTO>> GetUserLoginRecordsByUserIdInfo(int userId)
        {
            await UserLoginRecordValidation.ValidateUserHasLoginRecordsAsync(userId);
            var userLoginRecords = await UserLoginRecordData.GetByUserIdAsync(userId);

            if (userLoginRecords == null)
                throw new ArgumentException($"There are no login records for User Id [{userId}] in the system.");

            return userLoginRecords;
        }


        //Get All User Login Records :-
        public static async Task<List<UserLoginRecordReadDetailsDTO>> GetAllAsync(int pageNumber, int pageSize)
        {
            UserLoginRecordValidation.ValidateForGetAllUserLoginRecords(pageNumber, pageSize);
            return await UserLoginRecordData.GetAllAsync(pageNumber, pageSize);
        }


        //Does User Login Record Exist :-
        public static async Task<bool> ExistsAsync(int loginId)
        {
            UserLoginRecordValidation.ValidateLoginId(loginId);
            return await UserLoginRecordData.ExistsAsync(loginId);
        }


        //Get User Login Record Info :-
        public static async Task<UserLoginRecordReadBasicDTO> GetByIdAsync(int loginId)
        {
            UserLoginRecord loginRecord = await GetUserLoginRecordInfo(loginId);

            return new UserLoginRecordReadBasicDTO
            (
                loginRecord.LoginID,
                loginRecord.UserID,
                loginRecord.DateTime
            );
        }
        public static async Task<List<UserLoginRecordReadBasicDTO>> GetByUserIdAsync(int userId)
        {
            return await GetUserLoginRecordsByUserIdInfo(userId);
        }
    }
}