namespace BankSystem.DTOs.ClientDTOs
{
    public class ClientReadBasicDTO
    {
        public int ClientID { get; } = 0;
        public int PersonID { get;} = 0;
        public string AccountNumber { get; } = string.Empty;
        public decimal AccountBalance { get; } = 0;

        public ClientReadBasicDTO(int clientID, int personID, string accountNumber, decimal accountBalance)
        {
            this.ClientID = clientID;
            this.PersonID = personID;
            this.AccountNumber = accountNumber;
            this.AccountBalance = accountBalance;
        }
    }
}