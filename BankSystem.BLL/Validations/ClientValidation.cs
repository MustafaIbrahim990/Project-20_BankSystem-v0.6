using BankSystem.BLL.Helpers;
using BankSystem.BLL.Services;
using BankSystem.DAL.Data;
using BankSystem.Domain.Models;
using BankSystem.DTOs.ClientDTOs;
using BankSystem.DTOs.UserDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem.BLL.Validations
{
    public class ClientValidation
    {
        //Client Id :-
        public static void ValidateClientId(int clientId, string? fieldName = null)
        {
            ValidationHelper.ValidatePositiveInteger(clientId, fieldName ?? "Client Id");
        }
        public static async Task ValidateClientIdAsync(int clientId, string? fieldName = null)
        {
            ValidateClientId(clientId, fieldName);

            if (!await ClientData.ExistsByIdAsync(clientId))
                throw new ArgumentException($"{fieldName ?? "Client"} with Id [{clientId}] not found.");
        }


        //Person Id :-
        public static void ValidatePersonId(int personId)
        {
            PersonValidation.ValidatePersonId(personId);
        }
        public static async Task ValidatePersonIdAsync(int personId)
        {
            await PersonValidation.ValidatePersonIdAsync(personId);
        }


        //User Id :-
        public static async Task ValidateUserIdAsync(int userId)
        {
            await UserValidation.ValidateUserIdAsync(userId); 
        }


        //Account Number :-
        public static void ValidateAccountNumber(string accountNumber)
        {
            ValidationHelper.ValidateTextAndNumbers(accountNumber, "Account Number");
        }
        public static async Task ValidateAccountNumberExistsAsync(string accountNumber)
        {
            ValidateAccountNumber(accountNumber);

            if (!await ClientData.ExistsByAccountNumberAsync(accountNumber))
                throw new ArgumentException($"Client with account number [{accountNumber}] not found.");
        }
        public static async Task ValidateAccountNumberForCreateAsync(string accountNumber)
        {
            ValidateAccountNumber(accountNumber);

            if (await ClientData.ExistsByAccountNumberAsync(accountNumber))
                throw new ArgumentException($"Client with account number [{accountNumber}] already exists.");
        }
        public static async Task ValidateAccountNumberForUpdateAsync(int clientId, string accountNumber)
        {
            ValidateAccountNumber(accountNumber);

            if (await ClientData.ExistsByAccountNumberForUpdateAsync(clientId, accountNumber))
                throw new ArgumentException($"Client with account number [{accountNumber}] already exists.");
        }


        //Amount :-
        public static void ValidateAmount(decimal amount, string? fieldName = null, string? message = null)
        {
            ValidationHelper.ValidatePositiveDecimal(amount, fieldName ?? "Amount", message ?? $"{fieldName} cannot be a negative number.");
        }


        //PIN Code :-
        public static void ValidatePinCode(string pinCode, string? fieldName = null)
        {
            ValidationHelper.ValidatePINCode(pinCode, fieldName ?? "PIN Code");
        }


        //Validate For Get All Clients :-
        public static void ValidateForGetAllClients(int pageNumber, int pageSize)
        {
            ValidationHelper.ValidatePositiveInteger(pageNumber, "Page number");
            ValidationHelper.ValidatePositiveInteger(pageSize, "Page size");
        }


        //Validate Client Id and PIN Code :-
        public static async Task ValidateClientIdAndPinCodeAsync(int clientId, string pinCodeHashed)
        {
            if (!await ClientData.ExistsByIdAndPinCodeAsync(clientId, pinCodeHashed))
                throw new ArgumentException($"Incorrect Client Id Or PIN Code");
        }
        public static async Task ValidateAccountNumberAndPinCodeAsync(string accountNumber, string pinCodeHashed)
        {
            if (!await ClientData.ExistsByAccountNumberAndPinCodeAsync(accountNumber, pinCodeHashed))
                throw new ArgumentException($"Incorrect Account Number Or PIN Code");
        }


        //Validate For Create :-
        public static async Task ValidateForCreateAsync(Client client)
        {
            await ValidatePersonIdAsync(client.PersonID);
            await ValidateAccountNumberForCreateAsync(client.AccountNumber);
            ValidateAmount(client.AccountBalance, "Account Balance");
        }


        //Validate For Delete :-
        public static async Task ValidateForDeleteAsync(int clientId, string pinCodeHashed)
        {
            if (await ClientData.HasTransferRecordByIdAsync(clientId))
                throw new InvalidOperationException("Cannot delete client : this client linked to an active transfer record.");
        }
        public static async Task ValidateForDeleteAsync(string accountNumber, string pinCodeHashed)
        {
            Client? client = await ClientData.GetByAccountNumberAsync(accountNumber);

            if (client == null)
                throw new InvalidOperationException($"Client with account number [{accountNumber}] not found.");

            if (await ClientData.HasTransferRecordByIdAsync(client.ClientID))
                throw new InvalidOperationException("Cannot delete client : this client linked to an active transfer record.");
        }


        //Validate For Deposit :-
        public static async Task ValidateForDepositAsync(int clientId, string pinCodeHashed, decimal amount)
        {
            await ValidateClientIdAndPinCodeAsync(clientId, pinCodeHashed);
            ValidateAmount(amount, "Deposit Amount");
        }
        public static async Task ValidateForDepositAsync(string accountNumber, string pinCodeHashed, decimal amount)
        {
            await ValidateAccountNumberAndPinCodeAsync(accountNumber, pinCodeHashed);
            ValidateAmount(amount, "Deposit Amount");
        }


        //Validate For Withdraw :-
        public static async Task ValidateForWithdrawAsync(int clientId, string pinCodeHashed, decimal amount)
        {
            await ValidateClientIdAndPinCodeAsync(clientId, pinCodeHashed);
            ValidateAmount(amount, "Withdraw Amount");

            Client? client = await ClientData.GetByIdAsync(clientId);

            if (client == null)
                throw new ArgumentException($"Client with Id [{clientId}] not found.");

            if (client.AccountBalance < amount)
                throw new InvalidOperationException("Insufficient balance to complete the withdrawal.");
        }
        public static async Task ValidateForWithdrawAsync(string accountNumber, string pinCodeHashed, decimal amount)
        {
            await ValidateAccountNumberAndPinCodeAsync(accountNumber, pinCodeHashed);
            ValidateAmount(amount, "Withdraw Amount");

            Client? client = await ClientData.GetByAccountNumberAsync(accountNumber);

            if (client == null)
                throw new ArgumentException($"Client with account number [{accountNumber}] not found.");

            if (client.AccountBalance < amount)
                throw new InvalidOperationException("Insufficient balance to complete the withdrawal.");
        }
    }
}