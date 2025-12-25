using BankSystem.BLL.Helpers;
using BankSystem.DAL.Data;
using BankSystem.Domain.Models;
using BankSystem.DTOs.ClientDTOs;
using BankSystem.DTOs.CountryDTOs;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem.BLL.Validations
{
    public class CountryValidation
    {
        //Country Id :-
        public static void ValidateCountryId(int countryId)
        {
            ValidationHelper.ValidatePositiveInteger(countryId, "Country Id");
        }
        public static async Task ValidateCountryIdAsync(int countryId)
        {
            ValidateCountryId(countryId);

            if (!await CountryData.ExistsAsync(countryId))
                throw new ArgumentException($"Country with Id [{countryId}] not found.");
        }


        //Currency Id :-
        public static void ValidateCurrencyId(int currencyId)
        {
            ValidationHelper.ValidatePositiveInteger(currencyId, "Currency Id");
        }
        public static async Task ValidateCurrencyIdAsync(int currencyId)
        {
            ValidateCurrencyId(currencyId);

            if (!await CurrencyData.ExistsByIdAsync(currencyId))
                throw new ArgumentException($"Currency with Id [{currencyId}] not found.");
        }


        //Country Name :-
        public static void ValidateCountryName(string countryName)
        {
            ValidationHelper.ValidateText(countryName, "Country name");
        }
        public static async Task ValidateCountryNameExistsAsync(string countryName)
        {
            ValidateCountryName(countryName);

            if (!await CountryData.ExistsAsync(countryName))
                throw new ArgumentException($"Country with name [{countryName}] not found.");
        }
        public static async Task ValidateCountryNameExistsForCreateAsync(string countryName)
        {
            ValidateCountryName(countryName);

            if (await CountryData.ExistsAsync(countryName))
                throw new ArgumentException($"Country with name [{countryName}] already exists.");
        }
        public static async Task ValidateCountryNameExistsForUpdateAsync(int countryId, string countryName)
        {
            ValidateCountryName(countryName);

            if (await CountryData.ExistsAsync(countryId, countryName))
                throw new ArgumentException($"Country with name [{countryName}] already exists.");
        }


        //Validate For Get All Countries :-
        public static void ValidateForGetAllCountries(int pageNumber, int pageSize)
        {
            ValidationHelper.ValidatePositiveInteger(pageNumber, "Page number");
            ValidationHelper.ValidatePositiveInteger(pageSize, "Page size");
        }


        //Validate For Create :-
        public static async Task ValidateForCreateAsync(Country country)
        {
            await ValidateCountryNameExistsForCreateAsync(country.CountryName);
            await ValidateCurrencyIdAsync(country.CurrencyID);  
        }


        //Validate For Update :-
        public static async Task ValidateForUpdateAsync(int countryId, Country country)
        {
            await ValidateCountryIdAsync(countryId);
            await ValidateCountryNameExistsForUpdateAsync(countryId, country.CountryName);
            await ValidateCurrencyIdAsync(country.CurrencyID);
        }


        //Validate For Delete :-
        public static async Task ValidateForDeleteAsync(int countryId)
        {
            await ValidateCountryIdAsync(countryId);

            if (await CountryData.HasPeople(countryId))
                throw new InvalidOperationException("Cannot delete country : this country linked to an active person account.");
        }
        public static async Task ValidateForDeleteAsync(Country country)
        {
            if (await CountryData.HasPeople(country.CountryID))
                throw new InvalidOperationException("Cannot delete country : this country linked to an active person account.");
        }
    }
}