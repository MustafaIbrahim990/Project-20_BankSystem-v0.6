using BankSystem.BLL.Helpers;
using BankSystem.BLL.Validations;
using BankSystem.DAL.Data;
using BankSystem.Domain.Models;
using BankSystem.DTOs.UserDTOs;

namespace BankSystem.BLL.Services
{
    public class UserService
    {
        //Private Methods :-
        private static string GetUserPasswordHashed(string password, string salt, string? fieldName = null)
        {
            UserValidation.ValidatePassword(password, fieldName);
            return SecurityHelper.GenerateHash(password, salt);
        }
        private static async Task<User> GetUserInfoAsync(int userId)
        {
            await UserValidation.ValidateUserIdAsync(userId);
            User? user = await UserData.GetAsync(userId);

            if (user == null)
                throw new ArgumentException($"User with Id [{userId}] not found.");

            return user;
        }
        private static async Task<User> GetUserInfoByPersonIdAsync(int personId)
        {
            await UserValidation.ValidatePersonIdAsync(personId);
            User? user = await UserData.GetByPersonIdAsync(personId);

            if (user == null)
                throw new ArgumentException($"User with Person Id [{personId}] not found.");

            return user;
        }
        private static async Task<User> GetUserInfoAsync(string username)
        {
            await UserValidation.ValidateUsernameExistsAsync(username);
            User? user = await UserData.GetAsync(username);

            if (user == null)
                throw new ArgumentException("Incorrect Username Or Password.");

            return user;
        }
        private static async Task<User> GetUserInfoAsync(string username, string passwordHashed)
        {
            await UserValidation.ValidateUsernameAndPasswordAsync(username, passwordHashed);
            User? user = await UserData.GetAsync(username, passwordHashed);

            if (user == null)
                throw new ArgumentException("Incorrect Username Or Password.");

            return user;
        }


        //Get All Users :-
        public static async Task<List<UserReadDetailsDTO>> GetAllAsync(int pageNumber, int pageSize)
        {
            UserValidation.ValidateForGetAllUsers(pageNumber, pageSize);
            return await UserData.GetAllAsync(pageNumber, pageSize);
        }


        //User Exists :-
        public static async Task<bool> ExistsAsync(int userId)
        {
            UserValidation.ValidateUserId(userId);
            return await UserData.ExistsByIdAsync(userId);
        }
        public static async Task<bool> ExistsAsync(string username)
        {
            UserValidation.ValidateUsername(username);
            return await UserData.ExistsByUsernameAsync(username);
        }
        public static async Task<bool> ExistsAsync(string username, string password)
        {
            User user = await GetUserInfoAsync(username);
            string passwordHashed = GetUserPasswordHashed(password, user.Salt);

            return await UserData.ExistsByUsernameAndPasswordAsync(username, passwordHashed);
        }
        

        //Get User Info :-
        public static async Task<UserReadBasicDTO> GetAsync(int userId)
        {
            User user = await GetUserInfoAsync(userId);

            return new UserReadBasicDTO
            (
                user.UserID,
                user.PersonID,
                user.UserName,
                user.PermissionLevel
            );
        }
        public static async Task<UserReadBasicDTO> GetByPersonIdAsync(int personId)
        {
            User user = await GetUserInfoByPersonIdAsync(personId);

            return new UserReadBasicDTO
            (
                user.UserID,
                user.PersonID,
                user.UserName,
                user.PermissionLevel
            );
        }
        public static async Task<UserReadBasicDTO> GetAsync(string username, string password)
        {
            User user = await GetUserInfoAsync(username);
            string passwordHashed = GetUserPasswordHashed(password, user.Salt);

            await UserValidation.ValidateUsernameAndPasswordAsync(username, passwordHashed);

            return new UserReadBasicDTO
            (
                user.UserID,
                user.PersonID,
                user.UserName,
                user.PermissionLevel
            );
        }


        //Create :-
        public static async Task<UserReadBasicDTO> CreateAsync(UserCreateDTO dto)
        {
            string salt = SecurityHelper.GenerateSalt();

            User user = new User
            (
                personID: dto.PersonID,
                userName: dto.UserName,
                passwordHashed: GetUserPasswordHashed(dto.Password, salt),
                salt: salt,
                permissionLevel: dto.PermissionLevel
            );

            await UserValidation.ValidateForCreateAsync(user);
            user.UserID = await UserData.CreateAsync(user);

            if (user.UserID <= 0)
                throw new InvalidOperationException("Failed to create the user.");

            return new UserReadBasicDTO
            (
                user.UserID,
                user.PersonID,
                user.UserName,
                user.PermissionLevel
            );
        }


        //Update :-
        public static async Task<UserReadBasicDTO> UpdateAsync(int userId, UserUpdateDTO dto)
        {
            User user = new User
            (
                dto.UserName,
                dto.PermissionLevel
            );

            await UserValidation.ValidateForUpdateAsync(userId, user);

            if (!await UserData.UpdateAsync(userId, user))
                throw new InvalidOperationException($"Failed to updated the user.");

            User userAfterUpdate = await GetUserInfoAsync(userId);

            if (userAfterUpdate == null)
                throw new InvalidOperationException($"Failed to load the updated user information.");

            return new UserReadBasicDTO
            (
               userAfterUpdate.UserID,
               userAfterUpdate.PersonID,
               userAfterUpdate.UserName,
               userAfterUpdate.PermissionLevel
            );
        }


        //Update Password :-
        public static async Task<bool> UpdatePasswordAsync(UpdatePasswordDTO dto)
        {
            User founUser = await GetUserInfoAsync(dto.UserName);
            string currentPasswordHashed = GetUserPasswordHashed(dto.CurrentPassword, founUser.Salt, "Current Password");

            await UserValidation.ValidateUsernameAndPasswordAsync(dto.UserName, currentPasswordHashed);

            string newSalt = SecurityHelper.GenerateSalt();
            string newPasswordHashed = GetUserPasswordHashed(dto.NewPassword, newSalt, "New Password");

            User user = new User
            (
                userName: dto.UserName,
                newPasswordHashed: newPasswordHashed,
                newSalt: newSalt
            );

            return await UserData.UpdatePasswordAsync(user.UserName, currentPasswordHashed, newPasswordHashed, newSalt);
        }


        //Delete :-  
        public static async Task<bool> DeleteAsync(int userId)
        {
            await UserValidation.ValidateForDeleteAsync(userId);
            return await UserData.DeleteAsync(userId);
        }
        public static async Task<bool> DeleteAsync(string username, string password)
        {
            User user = await GetUserInfoAsync(username);
            string passwordHashed = GetUserPasswordHashed(password, user.Salt);

            await UserValidation.ValidateUsernameAndPasswordAsync(username, passwordHashed);
            await UserValidation.ValidateForDeleteAsync(username, passwordHashed);

            return await UserData.DeleteAsync(user.UserID);
        }


        //Verify User Credentials :-  
        public static async Task<UserReadBasicDTO> VerifyUserCredentialsAsync(string username, string password)
        {
            User foundUser = await GetUserInfoAsync(username);
            string passwordHashed = GetUserPasswordHashed(password, foundUser.Salt);

            User user = await GetUserInfoAsync(username, passwordHashed);

            return new UserReadBasicDTO
            (
                user.UserID,
                user.PersonID,
                user.UserName,
                user.PermissionLevel
            );
        }


        //Has Login Records :-    
        public static async Task<bool> HasLoginRecords(int userId)
        {
            await UserValidation.ValidateUserIdAsync(userId);
            return await UserData.HasLoginRecordsById(userId);
        }
        public static async Task<bool> HasLoginRecords(string username, string password)
        {
            User user = await GetUserInfoAsync(username);
            string passwordHashed = GetUserPasswordHashed(password, user.Salt);

            await UserValidation.ValidateUsernameAndPasswordAsync(username, passwordHashed);  
            return await UserData.HasLoginRecordsById(user.UserID);
        }


        //Has Client Transfer Records :-    
        public static async Task<bool> HasTransferRecords(int userId)
        {
            await UserValidation.ValidateUserIdAsync(userId);
            return await UserData.HasClientTransferRecordById(userId);
        }
        public static async Task<bool> HasTransferRecords(string username, string password)
        {
            User user = await GetUserInfoAsync(username);
            string passwordHashed = GetUserPasswordHashed(password, user.Salt);

            await UserValidation.ValidateUsernameAndPasswordAsync(username, passwordHashed);
            return await UserData.HasClientTransferRecordById(user.UserID);
        }
    }
}