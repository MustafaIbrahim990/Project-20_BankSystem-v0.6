using BankSystem.BLL.Helpers;
using BankSystem.BLL.Validations;
using BankSystem.DAL.Data;
using BankSystem.Domain.Models;
using BankSystem.DTOs.ClientDTOs;
using BankSystem.DTOs.ClientTransferRecordDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem.BLL.Services
{
    public class ClientTransferRecordService
    {
        //Private Methods :-
        private static async Task<ClientTransferRecordReadDetailsDTO> GetClientTransferRecordByIdAsync(int transferId)
        {
            await ClientTransferRecordValidation.ValidateTransferIdAsync(transferId);
            ClientTransferRecordReadDetailsDTO? clientTransferRecord = await ClientTransferRecordData.GetByIdAsync(transferId);

            if (clientTransferRecord == null)
                throw new ArgumentException($"Client Transfer Record with Id [{transferId}] not found.");

            return clientTransferRecord;
        }
        private static async Task<List<ClientTransferRecordReadDetailsDTO>> GetClientTransferRecordsByClientIdAsync(int clientId)
        {
            await ClientTransferRecordValidation.ValidateForClientHasLoginRecordsAsync(clientId);
            var dto = await ClientTransferRecordData.GetByClientIdAsync(clientId);

            if (dto == null)
                throw new ArgumentException($"There are no transfer record for Client Id [{clientId}] in the system.");

            return dto;
        }


        //Get All Client Transfer Records :-
        public static async Task<List<ClientTransferRecordReadDetailsDTO>> GetAllAsync(int pageNumber, int pageSize)
        {
            ClientTransferRecordValidation.ValidateForGetAllClients(pageNumber, pageSize);
            return await ClientTransferRecordData.GetAllAsync(pageNumber, pageSize);
        }


        //Does Transfer Record Exist :-
        public static async Task<bool> ExistsAsync(int transferId)
        {
            ClientTransferRecordValidation.ValidateTransferId(transferId);
            return await ClientTransferRecordData.ExistsByIdAsync(transferId);
        }


        //Get Transfer Record Info :-
        public static async Task<ClientTransferRecordReadDetailsDTO> GetByIdAsync(int transferId)
        {
            return await GetClientTransferRecordByIdAsync(transferId);
        }
        public static async Task<List<ClientTransferRecordReadDetailsDTO>> GetByClientIdAsync(int clientId)
        {
            return await GetClientTransferRecordsByClientIdAsync(clientId);
        }


        //Add New Transfer Record :-
        public static async Task<ClientTransferRecordReadDetailsDTO> CreateAsync(ClientTransferRecordCreateDTO newDto)
        {
            ClientTransferRecord transferRecord = new ClientTransferRecord
            (
                newDto.FromClientID,
                newDto.ToClientID,
                newDto.TransferAmount,
                newDto.ByUserID
            );

            await ClientTransferRecordValidation.ValidateForCreateAsync(transferRecord);
            transferRecord.TransferID = await ClientTransferRecordData.CreateAsync(transferRecord);

            if (transferRecord.TransferID <= 0)
                throw new InvalidOperationException("Failed to create the client transfer record.");

            ClientTransferRecordReadDetailsDTO dto = await GetClientTransferRecordByIdAsync(transferRecord.TransferID);
            return dto;
        }
    }
}