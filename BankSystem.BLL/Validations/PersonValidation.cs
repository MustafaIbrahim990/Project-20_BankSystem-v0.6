using BankSystem.BLL.Helpers;
using BankSystem.DAL.Data;
using BankSystem.Domain.Models;
using System;

namespace BankSystem.BLL.Validations
{
    public class PersonValidation
    {
        //Person Id :-
        public static void ValidatePersonId(int personId)
        {
            ValidationHelper.ValidatePositiveInteger(personId, "Person ID");
        }
        public static async Task ValidatePersonIdAsync(int personId)
        {
            ValidatePersonId(personId);

            if (!await PersonData.ExistsByIdAsync(personId))
                throw new ArgumentException($"Person with Id [{personId}] not found.");
        }


        //Country Id :-
        public static void ValidateCountryId(int countryId)
        {
            ValidationHelper.ValidatePositiveInteger(countryId, "Country ID");
        }
        public static async Task ValidateCountryIdAsync(int countryId)
        {
            ValidateCountryId(countryId);

            if (!await CountryData.ExistsAsync(countryId))
                throw new ArgumentException($"Country with Id [{countryId}] not found.");
        }


        //Email :-
        public static async Task ValidateEmailAsync(string email)
        {
            ValidationHelper.IsEmailValid(email);
            
            if (await PersonData.ExistsByEmailAsync(email))
                throw new ArgumentException($"This email [{email}] is already in use.");
        }
        public static async Task ValidateEmailAsync(int personId, string email)
        {
            ValidationHelper.IsEmailValid(email);

            if (await PersonData.ExistsByEmailForUpdateAsync(personId, email)) 
                throw new ArgumentException($"This email [{email}] is already in use.");
        }


        //Phone :-
        public static async Task ValidatePhoneAsync(string phone)
        {
            ValidationHelper.IsPhoneValid(phone);

            if (await PersonData.ExistsByPhoneAsync(phone))
                throw new ArgumentException($"This phone number [{phone}] is already in use.");
        }
        public static async Task ValidatePhoneAsync(int personId, string phone)
        {
            ValidationHelper.IsPhoneValid(phone);

            if (await PersonData.ExistsByPhoneForUpdateAsync(personId, phone)) 
                throw new ArgumentException($"This phone number [{phone}] is already in use.");
        }


        //FirstName & LastName :-
        public static void ValidateFirstName(string firstname)
        {
            ValidationHelper.ValidateText(firstname, "First Name");
        }
        public static void ValidateLastName(string lastname)
        {
            ValidationHelper.ValidateText(lastname, "Last Name");
        }


        //Validate For Get All People :-
        public static void ValidateForGetAllPeople(int pageNumber, int pageSize)
        {
            ValidationHelper.ValidatePositiveInteger(pageNumber, "Page number");
            ValidationHelper.ValidatePositiveInteger(pageSize, "Page size");
        }


        //Validate Basic Person Data :-
        public static async Task ValidateBasicPersonDataAsync(Person person)
        {
            ValidateFirstName(person.FirstName);
            ValidateLastName(person.LastName);
            await ValidateCountryIdAsync(person.CountryID);
        }


        //Validate For Create :-
        public static async Task ValidateForCreateAsync(Person person)
        {
            await ValidateBasicPersonDataAsync(person);
            await ValidateEmailAsync(person.Email);
            await ValidatePhoneAsync(person.Phone);
        }


        //Validate For Update :-
        public static async Task ValidateForUpdateAsync(int personId, Person person)
        {
            await ValidatePersonIdAsync(personId);
            await ValidateBasicPersonDataAsync(person);
            await ValidateEmailAsync(personId, person.Email);
            await ValidatePhoneAsync(personId, person.Phone);
        }


        //Validate For Delete :-
        public static async Task ValidateForDeleteAsync(int personId)
        {
            await ValidatePersonIdAsync(personId);

            if (await PersonData.IsClientAsync(personId))
                throw new InvalidOperationException($"Cannot delete person : this person linked to an active client account.");

            if (await PersonData.IsUserAsync(personId))
                throw new InvalidOperationException($"Cannot delete person : this person linked to an active user account.");
        }
    }
}