namespace BankSystem.Domain.Models
{
    public class ClientTransferRecord
    {
        public int TransferID { get; set; } = 0;
        public DateTime DateTime { get; set; } = new DateTime();
        public int FromClientID { get; set; } = 0;
        public int ToClientID { get; set; } = 0;
        public decimal TransferAmount { get; set; } = 0;
        public decimal FromBalanceAfterTransfer { get; set; } = 0;
        public decimal ToBalanceAfterTransfer { get; set; } = 0;
        public int ByUserID { get; set; } = 0;


        //For Create :-
        public ClientTransferRecord(int fromClientID, int toClientID, decimal transferAmount, int byUserID)
        {
            this.FromClientID = fromClientID;
            this.ToClientID = toClientID;
            this.TransferAmount = transferAmount;
            this.ByUserID = byUserID;
        }
    }
}