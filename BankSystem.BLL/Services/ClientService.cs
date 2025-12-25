using BankSystem.BLL.Helpers;
using BankSystem.BLL.Validations;
using BankSystem.DAL.Data;
using BankSystem.Domain.Models;
using BankSystem.DTOs.ClientDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem.BLL.Services
{
    public class ClientService
    {
        //Private Methods :-
        private static string GetClientPinCodeHashed(string pinCode, string salt, string? fieldName = null)
        {
            ClientValidation.ValidatePinCode(pinCode, fieldName);
            return SecurityHelper.GenerateHash(pinCode, salt);  
        }
        private static async Task<Client> GetClientInfoByIdAsync(int clientId)
        {
            await ClientValidation.ValidateClientIdAsync(clientId);
            Client? client = await ClientData.GetByIdAsync(clientId);

            if (client == null)
                throw new ArgumentException($"Client with Id [{clientId}] not found.");

            return client;
        }
        private static async Task<Client> GetClientInfoByIdAndPinCodeAsync(int clientId, string pinCode, string? fieldName = null)
        {
            Client foundClient = await GetClientInfoByIdAsync(clientId);
            string pinCodeHashed = GetClientPinCodeHashed(pinCode, foundClient.Salt, fieldName);

            //We Check if the client id and pin code valid or no :-
            await ClientValidation.ValidateClientIdAndPinCodeAsync(clientId, pinCodeHashed);

            Client client = await GetClientInfoByIdAsync(clientId);
            return client;
        }
        private static async Task<Client> GetClientInfoByAccountNumberAsync(string accountNumber)
        {
            await ClientValidation.ValidateAccountNumberExistsAsync(accountNumber);
            Client? client = await ClientData.GetByAccountNumberAsync(accountNumber);

            if (client == null)
                throw new ArgumentException($"Client with account number [{accountNumber}] not found.");

            return client;
        }
        private static async Task<Client> GetClientInfoByAccountNumberAndPinCodeAsync(string accountNumber, string pinCode, string? fieldName = null)
        {
            Client foundclient = await GetClientInfoByAccountNumberAsync(accountNumber);
            string pinCodeHashed = GetClientPinCodeHashed(pinCode, foundclient.Salt, fieldName);

            //We Check if the account number and pin code valid or no :-
            await ClientValidation.ValidateAccountNumberAndPinCodeAsync(accountNumber, pinCodeHashed);

            Client client = await GetClientInfoByAccountNumberAsync(accountNumber);
            return client;
        }
        private static async Task<Client> GetClientInfoByPersonId(int personId)
        {
            await ClientValidation.ValidatePersonIdAsync(personId);
            Client? client = await ClientData.GetByPersonIdAsync(personId);

            if (client == null)
                throw new ArgumentException($"Client with Person Id [{personId}] not found.");

            return client;
        }


        //Get All Clients :-
        public static async Task<List<ClientReadDetailsDTO>> GetAllAsync(int pageNumber, int pageSize)
        {
            ClientValidation.ValidateForGetAllClients(pageNumber, pageSize);
            return await ClientData.GetAllAsync(pageNumber, pageSize);
        }


        //Client Exists :-
        public static async Task<bool> ExistsByIdAsync(int clientId)
        {
            ClientValidation.ValidateClientId(clientId);
            return await ClientData.ExistsByIdAsync(clientId);
        }
        public static async Task<bool> ExistsByIdAndPinCodeAsync(int clientId, string pinCode)
        {
            Client client = await GetClientInfoByIdAsync(clientId);
            string pinCodeHashed = GetClientPinCodeHashed(pinCode, client.Salt);

            return await ClientData.ExistsByIdAndPinCodeAsync(clientId, pinCodeHashed);
        }
        public static async Task<bool> ExistsByAccountNumberAsync(string accountNumber)
        {
            ClientValidation.ValidateAccountNumber(accountNumber);
            return await ClientData.ExistsByAccountNumberAsync(accountNumber);
        }
        public static async Task<bool> ExistsByAccountNumberAndPinCodeAsync(string accountNumber, string pinCode)
        {
            Client client = await GetClientInfoByAccountNumberAsync(accountNumber);
            string pinCodeHashed = GetClientPinCodeHashed(pinCode, client.Salt);

            return await ClientData.ExistsByAccountNumberAndPinCodeAsync(accountNumber, pinCodeHashed);
        }
        public static async Task<bool> ExistsByPersonIdAsync(int personId)
        {
            ClientValidation.ValidatePersonId(personId);
            return await ClientData.ExistsByPersonIdAsync(personId);
        }


        //Get Client Info :-
        public static async Task<ClientReadBasicDTO> GetByIdAsync(int clientId)
        {
            Client client = await GetClientInfoByIdAsync(clientId);

            return new ClientReadBasicDTO
            (
                client.ClientID,
                client.PersonID,
                client.AccountNumber,
                client.AccountBalance
            );
        }
        public static async Task<ClientReadBasicDTO> GetByIdAndPinCodeAsync(int clientId, string pinCode)
        {
            Client client = await GetClientInfoByIdAndPinCodeAsync(clientId, pinCode);

            return new ClientReadBasicDTO
            (
                client.ClientID,
                client.PersonID,
                client.AccountNumber,
                client.AccountBalance
            );
        }
        public static async Task<ClientReadBasicDTO> GetByAccountNumberAsync(string accountNumber)
        {
            Client client = await GetClientInfoByAccountNumberAsync(accountNumber);

            return new ClientReadBasicDTO
            (
                client.ClientID,
                client.PersonID,
                client.AccountNumber,
                client.AccountBalance
            );
        }
        public static async Task<ClientReadBasicDTO> GetByAccountNumberAndPinCodeAsync(string accountNumber, string pinCode)
        {
            Client client = await GetClientInfoByAccountNumberAndPinCodeAsync(accountNumber, pinCode);

            return new ClientReadBasicDTO
            (
                client.ClientID,
                client.PersonID,
                client.AccountNumber,
                client.AccountBalance
            );
        }
        public static async Task<ClientReadBasicDTO> GetByPersonIdAsync(int personId)
        {
            Client client = await GetClientInfoByPersonId(personId);

            return new ClientReadBasicDTO
            (
                client.ClientID,
                client.PersonID,
                client.AccountNumber,
                client.AccountBalance
            );
        }


        //Create :-
        public static async Task<ClientReadBasicDTO> CreateAsync(ClientCreateDTO dto)
        {
            string salt = SecurityHelper.GenerateSalt();

            Client client = new Client
            (
                personID: dto.PersonID,
                accountNumber: dto.AccountNumber,
                accountBalance: dto.AccountBalance,
                pinCodeHashed: GetClientPinCodeHashed(dto.PinCode, salt),
                salt: salt
            );

            await ClientValidation.ValidateForCreateAsync(client);
            client.ClientID = await ClientData.CreateAsync(client);

            if (client.ClientID <= 0)
                throw new InvalidOperationException("Failed to create the client.");

            return new ClientReadBasicDTO
            (
                client.ClientID,
                dto.PersonID,
                dto.AccountNumber,
                dto.AccountBalance
            );
        }


        //Update PIN Code :-
        public static async Task<bool> UpdatePinCodeAsync(UpdatePinCodeByClientIdDTO dto)
        {
            Client foundClient = await GetClientInfoByIdAndPinCodeAsync(dto.ClientID, dto.CurrentPinCode, "Current PIN Code");
            string currentPinCodeHashed = foundClient.PinCodeHashed;

            string newSalt = SecurityHelper.GenerateSalt();

            Client client = new Client
            (
                clientID: dto.ClientID,
                newPinCodeHashed: GetClientPinCodeHashed(dto.NewPinCode, newSalt, "New PIN Code"),
                newSalt: newSalt
            );

            return await ClientData.UpdatePinCodeAsync(foundClient.AccountNumber, currentPinCodeHashed, client.PinCodeHashed, client.Salt);
        }
        public static async Task<bool> UpdatePinCodeAsync(UpdatePinCodeByAccountNumberDTO dto)
        {
            Client foundClient = await GetClientInfoByAccountNumberAndPinCodeAsync(dto.AccountNumber, dto.CurrentPinCode, "Current PIN Code");
            string currentPinCodeHashed = foundClient.PinCodeHashed;

            await ClientValidation.ValidateAccountNumberAndPinCodeAsync(dto.AccountNumber, currentPinCodeHashed);
            string newSalt = SecurityHelper.GenerateSalt();

            Client client = new Client
            (
                accountNumber: dto.AccountNumber,
                newPinCodeHashed: GetClientPinCodeHashed(dto.NewPinCode, newSalt, "New PIN Code"),
                newSalt: newSalt
            );

            return await ClientData.UpdatePinCodeAsync(client.AccountNumber, currentPinCodeHashed, client.PinCodeHashed, client.Salt);
        }


        //Delete :-
        public static async Task<bool> DeleteAsync(int clientId, string pinCode)
        {
            Client client = await GetClientInfoByIdAndPinCodeAsync(clientId, pinCode);

            await ClientValidation.ValidateForDeleteAsync(clientId, client.PinCodeHashed);
            return await ClientData.DeleteAsync(clientId);
        }
        public static async Task<bool> DeleteAsync(string accountNumber, string pinCode)
        {
            Client client = await GetClientInfoByAccountNumberAndPinCodeAsync(accountNumber, pinCode);

            await ClientValidation.ValidateForDeleteAsync(accountNumber, client.PinCodeHashed);
            return await ClientData.DeleteAsync(client.ClientID);
        }


        //Get Total Clients Balance :-
        public static async Task<decimal> GetTotalClientsBalanceAsync() => await ClientData.GetTotalClientsBalanceAsync();


        //Deposit :-
        public static async Task<bool> DepositAsync(DepositByClientIdDTO dto)
        {
            Client client = await GetClientInfoByIdAndPinCodeAsync(dto.ClientID, dto.PinCode);

            await ClientValidation.ValidateForDepositAsync(client.ClientID, client.PinCodeHashed, dto.Amount);
            return await ClientData.DepositAsync(client.AccountNumber, client.PinCodeHashed, dto.Amount);
        }
        public static async Task<bool> DepositAsync(DepositByAccountNumberDTO dto)
        {
            Client client = await GetClientInfoByAccountNumberAndPinCodeAsync(dto.AccountNumber, dto.PinCode);

            await ClientValidation.ValidateForDepositAsync(client.AccountNumber, client.PinCodeHashed, dto.Amount);
            return await ClientData.DepositAsync(client.AccountNumber, client.PinCodeHashed, dto.Amount);
        }


        //Withdraw :-
        public static async Task<bool> WithdrawAsync(WithdrawByClientIdDTO dto)
        {
            Client client = await GetClientInfoByIdAndPinCodeAsync(dto.ClientID, dto.PinCode);

            await ClientValidation.ValidateForWithdrawAsync(client.ClientID, client.PinCodeHashed, dto.Amount);
            return await ClientData.WithdrawAsync(client.AccountNumber, client.PinCodeHashed, dto.Amount);
        }
        public static async Task<bool> WithdrawAsync(WithdrawByAccountNumberDTO dto)
        {
            Client client = await GetClientInfoByAccountNumberAndPinCodeAsync(dto.AccountNumber, dto.PinCode);

            await ClientValidation.ValidateForWithdrawAsync(client.AccountNumber, client.PinCodeHashed, dto.Amount);
            return await ClientData.WithdrawAsync(client.AccountNumber, client.PinCodeHashed, dto.Amount);
        }


        //Is Linked To Transfer Record :-
        public static async Task<bool> HasTransferRecordByIdAsync(int clientId)
        {
            await ClientValidation.ValidateClientIdAsync(clientId);
            return await ClientData.HasTransferRecordByIdAsync(clientId);
        }
        public static async Task<bool> HasTransferRecordByAccountNumberAsync(string accountNumber)
        {
            Client client = await GetClientInfoByAccountNumberAsync(accountNumber);

            await ClientValidation.ValidateAccountNumberExistsAsync(accountNumber);
            return await ClientData.HasTransferRecordByIdAsync(client.ClientID);
        }


        //Is Client Credentials Valid :-
        public static async Task<bool> IsClientCredentialsValidAsync(int clientId, string pinCode)
        {
            Client client = await GetClientInfoByIdAndPinCodeAsync(clientId, pinCode);
            string pinCodeHashed = GetClientPinCodeHashed(pinCode, client.Salt);
            
            return await ClientData.ExistsByIdAndPinCodeAsync(clientId, pinCodeHashed);
        }
        public static async Task<bool> IsClientCredentialsValidAsync(string accountNumber, string pinCode)
        {
            Client client = await GetClientInfoByAccountNumberAndPinCodeAsync(accountNumber, pinCode);
            string pinCodeHashed = GetClientPinCodeHashed(pinCode, client.Salt);

            return await ClientData.ExistsByAccountNumberAndPinCodeAsync(accountNumber, pinCodeHashed);
        }


        //Verify Client Credentials :-
        public static async Task<ClientReadBasicDTO> VerifyClientCredentialsAsync(int clientId, string pinCode)
        {
            Client client = await GetClientInfoByIdAndPinCodeAsync(clientId, pinCode);

            return new ClientReadBasicDTO
            (
                client.ClientID,
                client.PersonID,
                client.AccountNumber,
                client.AccountBalance
            );
        }
        public static async Task<ClientReadBasicDTO> VerifyClientCredentialsAsync(string accountNumber, string pinCode)
        {
            Client client = await GetClientInfoByAccountNumberAndPinCodeAsync(accountNumber, pinCode);

            return new ClientReadBasicDTO
            (
                client.ClientID,
                client.PersonID,
                client.AccountNumber,
                client.AccountBalance
            );
        }
    }
}