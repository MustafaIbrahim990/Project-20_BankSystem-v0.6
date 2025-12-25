namespace BankSystem.DTOs.ClientDTOs
{
    public class WithdrawByClientIdDTO
    {
        public int ClientID { get; set; } = 0;
        public string PinCode { get; set; } = string.Empty;
        public decimal Amount { get; set; } = 0;

        public WithdrawByClientIdDTO(int clientId, string pinCode, decimal amount)
        {
            this.ClientID = clientId;
            this.PinCode = pinCode;
            this.Amount = amount;
        }
    }
}