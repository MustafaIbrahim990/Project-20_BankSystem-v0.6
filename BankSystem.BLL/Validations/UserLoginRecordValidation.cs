using BankSystem.BLL.Helpers;
using BankSystem.DAL.Data;
using BankSystem.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem.BLL.Validations
{
    public class UserLoginRecordValidation
    {
        //Login Id :-
        public static void ValidateLoginId(int loginId)
        {
            ValidationHelper.ValidatePositiveInteger(loginId, "Login Id");
        }
        public static async Task ValidateLoginIdAsync(int loginId)
        {
            ValidateLoginId(loginId);

            if (!await UserLoginRecordData.ExistsAsync(loginId))
                throw new ArgumentException($"User login record with Id [{loginId}] not found.");
        }


        //User Id :-
        public static async Task ValidateUserIdAsync(int userId)
        {
            await UserValidation.ValidateUserIdAsync(userId);
        }
        public static async Task ValidateUserHasLoginRecordsAsync(int userId)
        {
            await ValidateUserIdAsync(userId);

            if (!await UserData.HasLoginRecordsById(userId))
                throw new ArgumentException($"There are no login records for User Id [{userId}] in the system.");
        }


        //Validate For Get All User Login Records :-
        public static void ValidateForGetAllUserLoginRecords(int pageNumber, int pageSize)
        {
            ValidationHelper.ValidatePositiveInteger(pageNumber, "Page number");
            ValidationHelper.ValidatePositiveInteger(pageSize, "Page size");
        }
    }
}