namespace BankSystem.Domain.Models
{
    public class Client
    {
        public int ClientID { get; set; } = 0;
        public int PersonID { get; set; } = 0;
        public string AccountNumber { get; set; } = string.Empty;
        public decimal AccountBalance { get; set; } = 0;
        public string PinCodeHashed { get; set; } = string.Empty;
        public string Salt { get; set; } = string.Empty;


        //For Create :-
        public Client(int personID, string accountNumber, decimal accountBalance, string pinCodeHashed, string salt)
        {
            this.PersonID = personID;
            this.AccountNumber = accountNumber;
            this.AccountBalance = accountBalance;
            this.PinCodeHashed = pinCodeHashed;
            this.Salt = salt;
        }


        //For Update and others :-
        public Client(int clientID, int personID, string accountNumber, decimal accountBalance, string pinCodeHashed, string salt)
            : this(personID, accountNumber, accountBalance, pinCodeHashed, salt)
        {
            this.ClientID = clientID;
        }


        //For Change PIN Code :-
        public Client(int clientID, string newPinCodeHashed, string newSalt)
        {
            this.ClientID = clientID;
            this.PinCodeHashed = newPinCodeHashed;
            this.Salt = newSalt;
        }
        public Client(string accountNumber, string newPinCodeHashed, string newSalt)
        {
            this.AccountNumber = accountNumber;
            this.PinCodeHashed = newPinCodeHashed;
            this.Salt = newSalt;
        }
    }
}