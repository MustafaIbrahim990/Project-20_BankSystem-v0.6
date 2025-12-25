namespace BankSystem.DTOs.ClientDTOs
{
    public class WithdrawByAccountNumberDTO
    {
        public string AccountNumber { get; set; } = string.Empty;
        public string PinCode { get; set; } = string.Empty;
        public decimal Amount { get; set; } = 0;

        public WithdrawByAccountNumberDTO(string accountNumber, string pinCode, decimal amount)
        {
            this.AccountNumber = accountNumber;
            this.PinCode = pinCode;
            this.Amount = amount;
        }
    }
}