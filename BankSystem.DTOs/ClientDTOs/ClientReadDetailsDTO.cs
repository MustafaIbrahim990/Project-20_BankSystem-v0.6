namespace BankSystem.DTOs.ClientDTOs
{
    public class ClientReadDetailsDTO
    {
        public int ClientID { get; } = 0;
        public string? FirstName { get; } = string.Empty;
        public string? LastName { get; } = string.Empty;
        public string? CountryName { get; } = string.Empty;
        public string? Email { get; } = string.Empty;
        public string? Phone { get; } = string.Empty;
        public string? AccountNumber { get; } = string.Empty;
        public decimal? AccountBalance { get; } = 0;
        public string FullName
        {
            get { return $"{FirstName ?? string.Empty} {LastName ?? string.Empty}".Trim(); }
        } 

        public ClientReadDetailsDTO(int clientID, string? firstName, string? lastName, string? countryName, string? email, string? phone, string? accountNumber, decimal? accountBalance)
        {
            this.ClientID = clientID;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.CountryName = countryName;
            this.Email = email;
            this.Phone = phone;
            this.AccountNumber = accountNumber;
            this.AccountBalance = accountBalance;
        }
    }
}