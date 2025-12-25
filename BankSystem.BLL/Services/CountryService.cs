using BankSystem.BLL.Helpers;
using BankSystem.BLL.Validations;
using BankSystem.DAL.Data;
using BankSystem.Domain.Models;
using BankSystem.DTOs.ClientDTOs;
using BankSystem.DTOs.CountryDTOs;
using BankSystem.DTOs.PersonDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem.BLL.Services
{
    public class CountryService
    {
        //Private Methods :-
        private static async Task<Country> GetCountryInfo(int countryId)
        {
            await CountryValidation.ValidateCountryIdAsync(countryId);
            Country? country = await CountryData.GetAsync(countryId);

            if (country == null)
                throw new ArgumentException($"Country with Id [{countryId}] not found.");

            return country;
        }
        private static async Task<Country> GetCountryInfo(string countryName)
        {
            await CountryValidation.ValidateCountryNameExistsAsync(countryName);
            Country? country = await CountryData.GetAsync(countryName);

            if (country == null)
                throw new ArgumentException($"Country with name [{countryName}] not found.");

            return country;
        }


        //Get All Countries :-
        public static async Task<List<CountryReadDetailsDTO>> GetAllAsync(int pageNumber, int pageSize)
        {
            CountryValidation.ValidateForGetAllCountries(pageNumber, pageSize);
            return await CountryData.GetAllAsync(pageNumber, pageSize);
        }


        //Country Exists :-
        public static async Task<bool> ExistsAsync(int countryId)
        {
            CountryValidation.ValidateCountryId(countryId);
            return await CountryData.ExistsAsync(countryId);
        }
        public static async Task<bool> ExistsAsync(string countryName)
        {
            CountryValidation.ValidateCountryName(countryName);
            return await CountryData.ExistsAsync(countryName);
        }


        //Get Country Info :-
        public static async Task<CountryReadBasicDTO> GetAsync(int countryId)
        {
            Country country = await GetCountryInfo(countryId);

            return new CountryReadBasicDTO
            (
                country.CountryID,
                country.CountryName,
                country.CurrencyID
            );
        }
        public static async Task<CountryReadBasicDTO> GetAsync(string countryName)
        {
           
            Country country = await GetCountryInfo(countryName);

            return new CountryReadBasicDTO
            (
                country.CountryID,
                country.CountryName,
                country.CurrencyID
            );
        }
        public static async Task<List<CountryReadDetailsDTO>> GetCountriesByCurrencyIdAsync(int currencyId)
        {
            await CountryValidation.ValidateCurrencyIdAsync(currencyId);
            return await CountryData.GetByCurrencyIdAsync(currencyId);
        }


        //Create :-
        public static async Task<CountryReadBasicDTO> CreateAsync(CountryCreateDTO dto)
        {
            Country country = new Country
            (
                dto.CountryName,
                dto.CurrencyID
            );

            await CountryValidation.ValidateForCreateAsync(country);
            country.CountryID = await CountryData.CreateAsync(country);

            if (country.CountryID <= 0)
                throw new InvalidOperationException("Failed to create the country.");

            return new CountryReadBasicDTO
            (
                country.CountryID,
                country.CountryName,
                country.CurrencyID
            );
        }


        //Update :-
        public static async Task<CountryReadBasicDTO> UpdateAsync(int countryId, CountryUpdateDTO dto)
        {
            Country country = new Country
            (
                dto.CountryName,
                dto.CurrencyID
            );

            await CountryValidation.ValidateForUpdateAsync(countryId, country);

            if (!await CountryData.UpdateAsync(countryId, country))
                throw new InvalidOperationException($"Failed to update the country.");

            return new CountryReadBasicDTO
            (
                countryId,
                country.CountryName,
                country.CurrencyID
            );
        }


        //Delete :-
        public static async Task<bool> DeleteAsync(int countryId)
        {
            await CountryValidation.ValidateForDeleteAsync(countryId);
            return await CountryData.DeleteAsync(countryId);
        }
        public static async Task<bool> DeleteAsync(string countryName)
        {
            Country country = await GetCountryInfo(countryName);

            await CountryValidation.ValidateForDeleteAsync(country);
            return await CountryData.DeleteAsync(country.CountryID);
        }


        //Does Country Exist :-
        public static async Task<bool> HasPeople(int countryId)
        {
            await CountryValidation.ValidateCountryIdAsync(countryId);
            return await CountryData.HasPeople(countryId);
        }
        public static async Task<bool> HasPeople(string countryName)
        {
            Country country = await GetCountryInfo(countryName);
            return await CountryData.HasPeople(country.CountryID);
        }
    }
}