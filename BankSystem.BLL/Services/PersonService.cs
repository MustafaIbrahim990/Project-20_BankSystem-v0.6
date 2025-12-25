using BankSystem.Domain.Models;
using BankSystem.BLL.Validations;
using BankSystem.DAL.Data;
using BankSystem.DTOs.PersonDTOs;
using BankSystem.BLL.Helpers;

namespace BankSystem.BLL.Services
{
    public class PersonService
    {
        //Private Methods :-
        private static async Task<Person> GetPersonInfo(int personId)
        {
            await PersonValidation.ValidatePersonIdAsync(personId);
            Person? person = await PersonData.GetAsync(personId);

            if (person == null)
                throw new ArgumentException($"Person with Id [{personId}] not found.");

            return person;
        }


        //Get All People :-
        public static async Task<List<PersonReadDetailsDTO>> GetAllAsync(int pageNumber, int pageSize)
        {
            PersonValidation.ValidateForGetAllPeople(pageNumber, pageSize);
            return await PersonData.GetAllAsync(pageNumber, pageSize);
        }


        //Does Person Exist :-
        public static async Task<bool> ExistsAsync(int personId)
        {
            PersonValidation.ValidatePersonId(personId);
            return await PersonData.ExistsByIdAsync(personId);
        }


        //Get Person By Id :-
        public static async Task<PersonReadBasicDTO> GetAsync(int personId)
        {
            Person person = await GetPersonInfo(personId);

            return new PersonReadBasicDTO
            (
                person.PersonID,
                person.FirstName,
                person.LastName,
                person.CountryID,
                person.Email,
                person.Phone
            );
        }


        //Create Person :-
        public static async Task<PersonReadBasicDTO> CreateAsync(PersonCreateDTO dto)
        {
            Person person = new Person
            (
                dto.FirstName,
                dto.LastName,
                dto.CountryID,
                dto.Email,
                dto.Phone
            );

            await PersonValidation.ValidateForCreateAsync(person);
            person.PersonID = await PersonData.CreateAsync(person);

            if (person.PersonID <= 0)
                throw new InvalidOperationException("Failed to create the person.");

            return new PersonReadBasicDTO
            (
                person.PersonID,
                person.FirstName,
                person.LastName,
                person.CountryID,
                person.Email,
                person.Phone
            );
        }


        //Update Person :-
        public static async Task<PersonReadBasicDTO> UpdateAsync(int personId, PersonUpdateDTO dto)
        {
            Person person = new Person
            (
                dto.FirstName,
                dto.LastName,
                dto.CountryID,
                dto.Email,
                dto.Phone
            );

            await PersonValidation.ValidateForUpdateAsync(personId, person);

            if (!await PersonData.UpdateAsync(personId, person)) 
                throw new InvalidOperationException($"Failed to update the person.");

            return new PersonReadBasicDTO
            (
                personId,
                person.FirstName,
                person.LastName,
                person.CountryID,
                person.Email,
                person.Phone
            );
        }


        //Delete Person :-
        public static async Task<bool> DeleteAsync(int personId)
        {
            await PersonValidation.ValidateForDeleteAsync(personId);
            return await PersonData.DeleteAsync(personId);
        }


        //Is Email Unique :-
        public static async Task<bool> IsEmailUniqueAsync(string email)
        {
            ValidationHelper.IsEmailValid(email);
            return !await PersonData.ExistsByEmailAsync(email);
        }
        public static async Task<bool> IsEmailUniqueAsync(int personId, string email)
        {
            await PersonValidation.ValidatePersonIdAsync(personId);
            ValidationHelper.IsEmailValid(email);
            return !await PersonData.ExistsByEmailForUpdateAsync(personId, email);
        }


        //Is Phone Unique :-
        public static async Task<bool> IsPhoneUniqueAsync(string phone)
        {
            ValidationHelper.IsPhoneValid(phone);
            return !await PersonData.ExistsByPhoneAsync(phone);
        }
        public static async Task<bool> IsPhoneUniqueAsync(int personId, string phone)
        {
            await PersonValidation.ValidatePersonIdAsync(personId);
            ValidationHelper.IsPhoneValid(phone);
            return !await PersonData.ExistsByPhoneForUpdateAsync(personId, phone);
        }


        //Is Client :-
        public static async Task<bool> IsClientAsync(int personId)
        {
            await PersonValidation.ValidatePersonIdAsync(personId);
            return await PersonData.IsClientAsync(personId);
        }


        //Is User :-
        public static async Task<bool> IsUserAsync(int personId)
        {
            await PersonValidation.ValidatePersonIdAsync(personId);
            return await PersonData.IsUserAsync(personId);
        }
    }
}