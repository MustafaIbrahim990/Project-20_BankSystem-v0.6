namespace BankSystem.DTOs.PersonDTOs
{
    public class PersonReadDetailsDTO
    {
        //Properties :-
        public int PersonID { get; } = 0;
        public string? FirstName { get; } = string.Empty;
        public string? LastName { get; } = string.Empty;
        public string? CountryName { get; } = string.Empty;
        public string? CurrencyName { get; } = string.Empty;
        public string? CurrencyCode { get; } = string.Empty;
        public decimal? CurrencyRate { get; } = 0;
        public string? Email { get; } = string.Empty;
        public string? Phone { get; } = string.Empty;
        public string FullName
        {
            get { return ($"{this.FirstName} {this.LastName}").Trim(); }
        }

        //Constructor :-
        public PersonReadDetailsDTO(int personID, string? firstName, string? lastName, string? countryName, string? currencyName, string? currencyCode, decimal? currencyRate, string? email, string? phone)
        {
            this.PersonID = personID;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.CountryName = countryName;
            this.CurrencyName = currencyName;
            this.CurrencyCode = currencyCode;
            this.CurrencyRate = currencyRate;
            this.Email = email;
            this.Phone = phone;
        }
    }
}