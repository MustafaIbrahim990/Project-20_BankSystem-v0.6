using BankSystem.BLL.Validations;
using BankSystem.DAL.Data;
using BankSystem.DAL.Helpers;
using BankSystem.Domain.Models;
using BankSystem.DTOs.CurrencyDTOs;
using Microsoft.Data.SqlClient;
using System.Data;

namespace BankSystem.BLL.Services
{
    public class CurrencyService
    {
        //Private Methods :-
        private static async Task<Currency> GetCurrencyInfo(int currencyId)
        {
            await CurrencyValidation.ValidateCurrencyIdAsync(currencyId);
            Currency? currency = await CurrencyData.GetAsync(currencyId);

            if (currency == null)
                throw new ArgumentException($"Currency with Id [{currencyId}] not found.");

            return currency;
        }
        private static async Task<Currency> GetCurrencyInfo(string currencyCode)
        {
            await CurrencyValidation.ValidateCurrencyCodeExistsAsync(currencyCode);
            Currency? currency = await CurrencyData.GetAsync(currencyCode);

            if (currency == null)
                throw new ArgumentException($"Currency with code [{currencyCode}] not found.");

            return currency;
        }


        //Get All Currencires :-
        public static async Task<List<CurrencyReadDTO>> GetAllAsync(int pageNumber, int pageSize)
        {
            CurrencyValidation.ValidateForGetAllCurrencies(pageNumber, pageSize);
            return await CurrencyData.GetAllAsync(pageNumber, pageSize);
        }


        //Currency Exists :-
        public static async Task<bool> ExistsByIdAsync(int currencyId)
        {
            CurrencyValidation.ValidateCurrencyId(currencyId);
            return await CurrencyData.ExistsByIdAsync(currencyId);
        }
        public static async Task<bool> ExistsByCodeAsync(string currencyCode)
        {
            CurrencyValidation.ValidateCurrencyCode(currencyCode);
            return await CurrencyData.ExistsByCodeAsync(currencyCode);
        }
        public static async Task<bool> ExistsByCodeForUpdateAsync(int currencyId, string currencyCode)
        {
            CurrencyValidation.ValidateCurrencyId(currencyId);
            CurrencyValidation.ValidateCurrencyCode(currencyCode);
            return await CurrencyData.ExistsByCodeForUpdateAsync(currencyId, currencyCode);
        }
        public static async Task<bool> ExistsByNameAsync(string currencyName)
        {
            CurrencyValidation.ValidateCurrencyName(currencyName);
            return await CurrencyData.ExistsByNameAsync(currencyName);
        }
        public static async Task<bool> ExistsByNameForUpdateAsync(int currencyId, string currencyName)
        {
            CurrencyValidation.ValidateCurrencyId(currencyId);
            CurrencyValidation.ValidateCurrencyName(currencyName);
            return await CurrencyData.ExistsByNameForUpdateAsync(currencyId, currencyName);
        }


        //Get Currency Info :-
        public static async Task<CurrencyReadDTO> GetAsync(int currencyId)
        {
            Currency currency = await GetCurrencyInfo(currencyId);

            return new CurrencyReadDTO
            (
                currency.CurrencyID,
                currency.CurrencyName,
                currency.CurrencyCode,
                currency.CurrencyRate
            );
        }
        public static async Task<CurrencyReadDTO> GetAsync(string currencyCode)
        {
            Currency? currency = await GetCurrencyInfo(currencyCode);

            return new CurrencyReadDTO
            (
                currency.CurrencyID,
                currency.CurrencyName,
                currency.CurrencyCode,
                currency.CurrencyRate
            );
        }


        //Create :-
        public static async Task<CurrencyReadDTO> CreateAsync(CurrencyCreateDTO dto)
        {
            Currency currency = new Currency
            (
                dto.CurrencyName,
                dto.CurrencyCode,
                dto.CurrencyRate
            );

            await CurrencyValidation.ValidateForCreateAsync(currency);
            currency.CurrencyID = await CurrencyData.CreateAsync(currency);

            if (currency.CurrencyID <= 0)
                throw new InvalidOperationException("Failed to create the currency.");

            return new CurrencyReadDTO
            (
                 currency.CurrencyID,
                 currency.CurrencyName,
                 currency.CurrencyCode,
                 currency.CurrencyRate
            );
        }


        //Update :-
        public static async Task<CurrencyReadDTO> UpdateAsync(int currencyId, CurrencyUpdateDTO dto)
        {
            Currency currency = new Currency
            (
                dto.CurrencyName,
                dto.CurrencyCode,
                dto.CurrencyRate
            );

            await CurrencyValidation.ValidateForUpdateAsync(currencyId, currency);

            if (!await CurrencyData.UpdateAsync(currencyId, currency))
                throw new InvalidOperationException($"Failed to updated the currency data.");

            return new CurrencyReadDTO
            (
                currencyId,
                currency.CurrencyName,
                currency.CurrencyCode,
                currency.CurrencyRate
            );
        }
        public static async Task<CurrencyReadDTO> UpdateRateAsync(int currencyId, decimal newCurrencyRate)
        {
            await CurrencyValidation.ValidateForUpdateCurrencyRateAsync(currencyId, newCurrencyRate);

            if (!await CurrencyData.UpdateRateAsync(currencyId, newCurrencyRate))
                throw new InvalidOperationException("Failed to updated the currency rate.");

            Currency currency = await GetCurrencyInfo(currencyId);

            return new CurrencyReadDTO
            (
                currency.CurrencyID,
                currency.CurrencyName,
                currency.CurrencyCode,
                currency.CurrencyRate
            );
        }


        //Delete :-
        public static async Task<bool> DeleteAsync(int currencyId)
        {
            await CurrencyValidation.ValidateForDeleteAsync(currencyId);
            return await CurrencyData.DeleteAsync(currencyId);
        }
        public static async Task<bool> DeleteAsync(string currencyCode)
        {
            Currency currency = await GetCurrencyInfo(currencyCode);

            await CurrencyValidation.ValidateForDeleteAsync(currency);
            return await CurrencyData.DeleteAsync(currency.CurrencyID);
        }


        //Has Countries :-
        public static async Task<bool> HasCountriesByIdAsync(int currencyId)
        {
            await CurrencyValidation.ValidateCurrencyIdAsync(currencyId);
            return await CurrencyData.HasCountriesById(currencyId);
        }
        public static async Task<bool> HasCountriesByCodeAsync(string currencyCode)
        {
            Currency currency = await GetCurrencyInfo(currencyCode);
            return await CurrencyData.HasCountriesById(currency.CurrencyID);
        }
    }
}