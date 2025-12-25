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
    public class CurrencyValidation
    {
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


        //Currency Name :-
        public static void ValidateCurrencyName(string currencyName)
        {
            ValidationHelper.ValidateText(currencyName, "Currency name");
        }
        public static async Task ValidateCurrencyNameExistsAsync(string currencyName)
        {
            ValidateCurrencyName(currencyName);

            if (!await CurrencyData.ExistsByNameAsync(currencyName))
                throw new ArgumentException($"Currency with name [{currencyName}] not found.");
        }
        public static async Task ValidateCurrencyNameForCreateAsync(string currencyName)
        {
            ValidateCurrencyName(currencyName);

            if (await CurrencyData.ExistsByNameAsync(currencyName))
                throw new ArgumentException($"Currency with name [{currencyName}] already exists.");
        }
        public static async Task ValidateCurrencyNameForUpdateAsync(int currencyId, string currencyName)
        {
            ValidateCurrencyName(currencyName);

            if (await CurrencyData.ExistsByNameForUpdateAsync(currencyId, currencyName))
                throw new ArgumentException($"Currency with name [{currencyName}] already exists.");
        }


        //Currency Code :-
        public static void ValidateCurrencyCode(string currencyCode)
        {
            ValidationHelper.ValidateText(currencyCode, "Currency code");
        }
        public static async Task ValidateCurrencyCodeExistsAsync(string currencyCode)
        {
            ValidateCurrencyCode(currencyCode);

            if (!await CurrencyData.ExistsByCodeAsync(currencyCode))
                throw new ArgumentException($"Currency with code [{currencyCode}] not found.");
        }
        public static async Task ValidateCurrencyCodeForCreateAsync(string currencyCode)
        {
            ValidateCurrencyCode(currencyCode);

            if (await CurrencyData.ExistsByCodeAsync(currencyCode))
                throw new ArgumentException($"Currency with code [{currencyCode}] already exists.");
        }
        public static async Task ValidateCurrencyCodeForUpdateAsync(int currencyId, string currencyCode)
        {
            ValidateCurrencyCode(currencyCode);

            if (await CurrencyData.ExistsByCodeForUpdateAsync(currencyId, currencyCode))
                throw new ArgumentException($"Currency with code [{currencyCode}] already exists.");
        }


        //Currency Rate :-
        public static void ValidateCurrencyRate(decimal currencyRate, string? fieldName = null)
        {
            ValidationHelper.ValidatePositiveDecimal(currencyRate, fieldName ?? "Currency rate");
        }


        //Validate For Get All Currencies :-
        public static void ValidateForGetAllCurrencies(int pageNumber, int pageSize)
        {
            ValidationHelper.ValidatePositiveInteger(pageNumber, "Page number");
            ValidationHelper.ValidatePositiveInteger(pageSize, "Page size");
        }


        //Validate For Create :-
        public static async Task ValidateForCreateAsync(Currency currency)
        {
            await ValidateCurrencyNameForCreateAsync(currency.CurrencyName);
            await ValidateCurrencyCodeForCreateAsync(currency.CurrencyCode);
            ValidateCurrencyRate(currency.CurrencyRate);
        }


        //Validate For Update :-
        public static async Task ValidateForUpdateAsync(int currencyId, Currency currency)
        {
            await ValidateCurrencyIdAsync(currencyId);
            await ValidateCurrencyNameForUpdateAsync(currencyId, currency.CurrencyName);
            await ValidateCurrencyCodeForUpdateAsync(currencyId, currency.CurrencyCode);
            ValidateCurrencyRate(currency.CurrencyRate);
        }
        public static async Task ValidateForUpdateCurrencyRateAsync(int currencyId, decimal newCurrencyRate)
        {
            await ValidateCurrencyIdAsync(currencyId);
            ValidateCurrencyRate(newCurrencyRate, "New currency rate");
        }


        //Validate For Delete :-
        public static async Task ValidateForDeleteAsync(int currencyId)
        {
            await ValidateCurrencyIdAsync(currencyId);

            if (await CurrencyData.HasCountriesById(currencyId))
                throw new InvalidOperationException("Cannot delete currency : this currency linked to an active country.");
        }
        public static async Task ValidateForDeleteAsync(Currency currency)
        {
            if (await CurrencyData.HasCountriesById(currency.CurrencyID))
                throw new InvalidOperationException("Cannot delete currency : this currency linked to an active country.");
        }
    }
}