using System.Linq;

namespace BankSystem.BLL.Helpers
{
    public class ValidationHelper
    {
        //Is Value Intger :-
        public static bool IsInteger(object value)
        {
            //return long.TryParse((value).ToString(), out _);

            string? newValue = value.ToString();

            if (newValue == null)
                return false;

            return newValue.All(char.IsDigit);

            ////we use long because the phone number.
            //return long.TryParse((value).ToString(), out _);
        }
        public static void ValidateIntegerNumber(object value, string fieldName, string? message = null)
        {
            if (string.IsNullOrWhiteSpace(value.ToString()))
                throw new ArgumentException($"{fieldName} is required.");

            if (!IsInteger(value))
                throw new ArgumentException(message ?? $"{fieldName} must contain numbers only.");
        }
        public static bool IsPositiveInteger(int value)
        {
            return (value > 0);
        }
        public static void ValidatePositiveInteger(int value, string fieldName, string? message = null)
        {
            if (!IsInteger(value) || !IsPositiveInteger(value))
                throw new ArgumentException($"{fieldName} must be a valid number.");

            if (!IsPositiveInteger(value))
                throw new ArgumentException(message ?? $"{fieldName} must be a positive number.");
        }


        //Is Value Long for (Phone Number) :-
        public static bool IsLong(object value)
        {
            string? newValue = value.ToString();

            if (newValue == null)
                return false;

            return newValue.All(char.IsDigit);
        }
        public static void ValidateLongNumber(string value, string fieldName, string? message = null)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException($"{fieldName} is required.");

            if (!IsLong(value))
                throw new ArgumentException(message ?? $"{fieldName} must contain numbers only.");
        }
        

        //Is Deciaml :-
        public static bool IsDecimal(object value)
        {
            if (value == null)
                return false;

            return decimal.TryParse(value.ToString(), out _);
        }
        public static void ValidatePositiveDecimal(decimal value, string fieldName, string? message = null)
        {
            if (!IsDecimal(value))
                throw new ArgumentException($"{fieldName} must be a valid number.");

            if (value <= 0)
                throw new ArgumentException(message ?? $"{fieldName} must be a positive number.");
        }


        //Is Value Text :-
        public static bool IsText(object value)
        {
            string? newValue = value.ToString();

            if (newValue == null)
                return false;

            return (newValue.Any(char.IsLetter) || newValue.Any(char.IsWhiteSpace));
        }
        public static void ValidateText(string value, string fieldName, string? message = null)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException($"{fieldName} is required.");

            if (!IsText(value))
                throw new ArgumentException(message ?? $"{fieldName} must contain letters only.");
        }


        //Is Value Text and Number :-
        public static bool IsValueTextAndNumbers(object value)
        {
            string? newValue = value.ToString();

            if (newValue == null)
                return false;

            return newValue.Any(char.IsDigit) && newValue.Any(char.IsLetter);
        }
        public static void ValidateTextAndNumbers(string value, string fieldName, string? message = null)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException($"{fieldName} is required.");

            if (!IsValueTextAndNumbers(value))
                throw new ArgumentException(message ?? $"{fieldName} must contain letters and numbers.");
        }


        //Is Value String :-
        public static bool IsValueString(object value)
        {
            return !IsInteger(value);
        }
        public static void ValidateString(string value, string fieldName, string? message = null)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException($"{fieldName} is required.");

            //if (!IsValueString(value))
            //    throw new ArgumentException(message ?? $"{fieldName} must contain letters only.");
        }


        //Email :-
        public static void IsEmailValid(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email is required.");

            if (!IsValueTextAndNumbers(email))
                throw new ArgumentException("Email must contain letters and numbers");

            //This Feuture is not implemented yet :-
            if (!email.EndsWith("@gmail.com"))
                throw new ArgumentException("Invalid email format, should include @gmail.com.");
        }


        //Phone :-
        public static void IsPhoneValid(string phone)
        {
            ValidateLongNumber(phone, "Phone");

            if (phone.Length < 10)
                throw new ArgumentException("Phone Number must be at least 10 digits.");
        }
        

        //Password :-
        public static void ValidatePassword(string value, string fieldName)
        {
            ValidateIntegerNumber(value, fieldName);

            if (value.Length >= 8 && value.Length <= 14)  
                throw new ArgumentException($"{fieldName} length must be between 8 and 14 characters.");
        }


        //PIN Code :-
        public static void ValidatePINCode(string value, string fieldName)
        {
            ValidateIntegerNumber(value, fieldName);

            if (value.Length <= 0 || value.Length > 6)
                throw new ArgumentException($"{fieldName} must be be between 4 and 6 digits");
        }


        //Salt :-
        public static void ValidateSalt(string salt)
        {
            if (string.IsNullOrWhiteSpace(salt))
                throw new ArgumentException($"An error occurred while processing the password.");
        }


        //Permission Level :-
        public static void ValidatePermissionLevel(int value)
        {
            ValidateIntegerNumber(value, "user permissions", "Invalid user permissions.");
        }
    }
}