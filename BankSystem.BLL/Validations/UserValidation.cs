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
    public class UserValidation
    {
       //User Id :
        public static void ValidateUserId(int userId)
        {
            ValidationHelper.ValidatePositiveInteger(userId, "User Id");
        }
        public static async Task ValidateUserIdAsync(int userId, string? message = null)
        {
            ValidateUserId(userId);

            if (!await UserData.ExistsByIdAsync(userId))
                throw new ArgumentException(message ?? $"User with Id [{userId}] not found.");
        }


        //Person Id :-
        public static async Task ValidatePersonIdAsync(int personId)
        {
            await PersonValidation.ValidatePersonIdAsync(personId);
        }


        //Username :-
        public static void ValidateUsername(string username)
        {
            ValidationHelper.ValidateTextAndNumbers(username, "Username");
        }
        public static async Task ValidateUsernameExistsAsync(string username)
        {
            ValidateUsername(username);

            if (!await UserData.ExistsByUsernameAsync(username))
                throw new ArgumentException($"User with username [{username}] not found.");
        }
        public static async Task ValidateUserNameForCreateAsync(string username)
        {
            ValidateUsername(username);

            if (await UserData.ExistsByUsernameAsync(username))
                throw new ArgumentException($"User with username [{username}] already exists.");
        }
        public static async Task ValidateUserNameForUpdateAsync(int userId, string username)
        {
            ValidateUserId(userId);
            ValidateUsername(username);

            if (await UserData.ExistsByUsernameForUpdateAsync(userId, username))
                throw new ArgumentException($"User with username [{username}] already exists.");
        }
        public static async Task ValidateUsernameAndPasswordAsync(string username, string password)
        {
            if (!await UserData.ExistsByUsernameAndPasswordAsync(username, password))
                throw new ArgumentException($"Incorrect Username Or Password");
        }


        //Password :-
        public static void ValidatePassword(string password, string? fieldName = null)
        {
            ValidationHelper.ValidatePassword(password, fieldName ?? "Password");
        }


        //Salt :-
        public static void ValidateSalt(string salt)
        {
            ValidationHelper.ValidateSalt(salt);
        }


        //Permission Level :-
        public static void ValidatePermissionLevel(int permissionLevel)
        {
            ValidationHelper.ValidatePermissionLevel(permissionLevel);
        }
       

        //Validate For Get All Users :-
        public static void ValidateForGetAllUsers(int pageNumber, int pageSize)
        {
            ValidationHelper.ValidatePositiveInteger(pageNumber, "Page number");
            ValidationHelper.ValidatePositiveInteger(pageSize, "Page size");
        }


        //Validate For Create :-
        public static async Task ValidateForCreateAsync(User user)
        {
            await ValidatePersonIdAsync(user.PersonID);
            await ValidateUserNameForCreateAsync(user.UserName);
            ValidatePermissionLevel(user.PermissionLevel);
        }


        //Validate For Update :-
        public static async Task ValidateForUpdateAsync(int userId, User user)
        {
            await ValidateUserIdAsync(userId);
            await ValidateUserNameForUpdateAsync(userId, user.UserName);
            ValidatePermissionLevel(user.PermissionLevel);
        }


        //Validate For Delete :-
        public static async Task ValidateForDeleteAsync(int userId)
        {
            await ValidateUserIdAsync(userId);

            if (await UserData.HasLoginRecordsById(userId))
                throw new InvalidOperationException("Cannot delete user : this user is linked to an active login records.");

            if (await UserData.HasClientTransferRecordById(userId))
                throw new InvalidOperationException("Cannot delete user : this user is linked to an active transfer records.");
        }
        public static async Task ValidateForDeleteAsync(string username, string passwordHashed)
        {
            User? user = await UserData.GetAsync(username, passwordHashed);

            if (user == null)
                throw new InvalidOperationException($"User with username [{username}] not found.");

            if (await UserData.HasLoginRecordsById(user.UserID)) 
                throw new InvalidOperationException("Cannot delete user : this user is linked to an active login records.");

            if (await UserData.HasClientTransferRecordById(user.UserID))
                throw new InvalidOperationException("Cannot delete user : this user is linked to an active transfer records.");
        }
    }
}