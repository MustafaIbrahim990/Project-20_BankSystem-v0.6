using BankSystem.BLL.Helpers;
using BankSystem.DAL.Data;
using BankSystem.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem.BLL.Validations
{
    public class ClientTransferRecordValidation
    {
        //Transfer Id :-
        public static void ValidateTransferId(int clientId)
        {
            ValidationHelper.ValidatePositiveInteger(clientId, "Transfer Id");
        }
        public static async Task ValidateTransferIdAsync(int transferId)
        {
            ValidateTransferId(transferId);

            if (!await ClientTransferRecordData.ExistsByIdAsync(transferId))
                throw new ArgumentException($"Transfer record with Id [{transferId}] not found.");
        }


        //Client Id :-
        public static async Task ValidateClientIdAsync(int clientId, string? fieldName = null)
        {
            await ClientValidation.ValidateClientIdAsync(clientId, fieldName);
        }


        //Amount :-
        public static void ValidateAmount(decimal amount, string? fieldName = null, string? message = null)
        {
            ValidationHelper.ValidatePositiveDecimal(amount, fieldName ?? "Amount", message ?? $"{fieldName} cannot be a negative number.");
        }


        //User Id :-
        public static async Task ValidateUserIdAsync(int userId)
        {
            await UserValidation.ValidateUserIdAsync(userId);
        }


        //Validate For Get All Client Transfer Records :-
        public static void ValidateForGetAllClients(int pageNumber, int pageSize)
        {
            ValidationHelper.ValidatePositiveInteger(pageNumber, "Page number");
            ValidationHelper.ValidatePositiveInteger(pageSize, "Page size");
        }


        //Validate For Create :-
        public static async Task ValidateForCreateAsync(ClientTransferRecord clientTransferRecord)
        {
            await ValidateClientIdAsync(clientTransferRecord.FromClientID, "From Client Id");
            await ValidateClientIdAsync(clientTransferRecord.ToClientID, "To Client Id");
            ValidateAmount(clientTransferRecord.TransferAmount, "Transfer Amount");
            await ValidateUserIdAsync(clientTransferRecord.ByUserID);

            if (clientTransferRecord.FromClientID == clientTransferRecord.ToClientID)
                throw new ArgumentException("Transfer cannot be made to same account.");

            Client? fromClient = await ClientData.GetByIdAsync(clientTransferRecord.FromClientID);

            if (fromClient == null)
                throw new ArgumentException($"Client with Id [{clientTransferRecord.FromClientID}] not found.");

            if (fromClient.AccountBalance < clientTransferRecord.TransferAmount)
                throw new InvalidOperationException("Insufficient balance in source account.");
        }


        //Validate For Create :-
        public static async Task ValidateForClientHasLoginRecordsAsync(int clientId)
        {
            await ValidateClientIdAsync(clientId);

            if (!await ClientData.HasTransferRecordByIdAsync(clientId))
                throw new InvalidOperationException($"There are no transfer records for Client Id [{clientId}] in the system."); 
        }
    }
}