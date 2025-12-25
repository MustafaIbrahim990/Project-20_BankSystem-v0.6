namespace BankSystem.DTOs.ClientTransferRecordDTOs
{
    public class ClientTransferRecordReadDetailsDTO
    {
        public int TransferID { get; } = 0;
        public DateTime? DateTime { get; } = new DateTime();
        public string? FromClientName { get; } = string.Empty;
        public string? ToClientName { get; } = string.Empty;
        public decimal? TransferAmount { get; } = 0;
        public decimal? FromBalanceAfterTransfer { get; } = 0;
        public decimal? ToBalanceAfterTransfer { get; } = 0;
        public string? ByUserName { get; } = string.Empty;

        public ClientTransferRecordReadDetailsDTO(int transferID, DateTime? dateTime, string? fromClientName, string? toClientName, decimal? transferAmount, decimal? fromBalanceAfterTransfer, decimal? toBalanceAfterTransfer, string? byUserName)
        {
            this.TransferID = transferID;
            this.DateTime = dateTime;
            this.FromClientName = fromClientName;
            this.ToClientName = toClientName;
            this.TransferAmount = transferAmount;
            this.FromBalanceAfterTransfer = fromBalanceAfterTransfer;
            this.ToBalanceAfterTransfer = toBalanceAfterTransfer;
            this.ByUserName = byUserName;
        }
    }
}