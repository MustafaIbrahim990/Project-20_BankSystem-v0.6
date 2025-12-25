namespace BankSystem.DTOs.ClientDTOs
{
    public class UpdatePinCodeByAccountNumberDTO
    {
        public string AccountNumber { get; set; } = string.Empty;
        public string CurrentPinCode { get; set; } = string.Empty;
        public string NewPinCode { get; set; } = string.Empty;

        public UpdatePinCodeByAccountNumberDTO(string accountNumber, string currentPinCode, string newPinCode)
        {
            this.AccountNumber = accountNumber;
            this.CurrentPinCode = currentPinCode;
            this.NewPinCode = newPinCode;
        }
    }
}