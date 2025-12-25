namespace BankSystem.DTOs.ClientTransferRecordDTOs
{
    public class ClientTransferRecordCreateDTO
    {
        public int FromClientID { get; } = 0;
        public int ToClientID { get; } = 0;
        public decimal TransferAmount { get; } = 0;
        public int ByUserID { get; } = 0;

        public ClientTransferRecordCreateDTO(int fromClientID, int toClientID, decimal transferAmount, int byUserID)
        {
            this.FromClientID = fromClientID;
            this.ToClientID = toClientID;
            this.TransferAmount = transferAmount;
            this.ByUserID = byUserID;
        }
    }
}