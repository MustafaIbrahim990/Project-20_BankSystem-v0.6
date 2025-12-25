namespace BankSystem.DTOs.CountryDTOs
{
    public class CountryReadDetailsDTO
    {
        //Properties :-
        public int CountryID { get; set; } = 0;
        public string? CountryName { get; set; } = string.Empty;
        public string? CurrencyName { get; set; } = string.Empty;
        public string? CurrencyCode { get; set; } = string.Empty;
        public decimal? CurrencyRate { get; set; } = 0;


        //Constructor :-
        public CountryReadDetailsDTO(int countryID, string? countryName, string? currencyName, string? currencyCode, decimal? currencyRate)
        {
            this.CountryID = countryID;
            this.CountryName = countryName;
            this.CurrencyName = currencyName;
            this.CurrencyCode = currencyCode;
            this.CurrencyRate = currencyRate;
        }
    }
}