namespace BankSystem.DTOs.ClientDTOs
{
    public class ClientCreateDTO
    {
        public int PersonID { get; } = 0;
        public string AccountNumber { get; } = string.Empty;
        public decimal AccountBalance { get; } = 0;
        public string PinCode { get; } = string.Empty;

        public ClientCreateDTO(int personID, string accountNumber, decimal accountBalance, string pinCode)
        {
            this.PersonID = personID;
            this.AccountNumber = accountNumber;
            this.AccountBalance = accountBalance;
            this.PinCode = pinCode;
        }
    }
}