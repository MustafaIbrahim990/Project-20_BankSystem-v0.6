namespace BankSystem.Domain.Models
{
    public class Country
    {
        public int CountryID { get; set; } = 0;
        public string CountryName { get; set; } = string.Empty;
        public int CurrencyID { get; set; } = 0;


        //For Create Country :-
        public Country(string countryName, int currencyID)
        {
            this.CountryName = countryName;
            this.CurrencyID = currencyID;
        }

        //For Update and others :-
        public Country(int countryID, string countryName, int currencyID)
        {
            this.CountryID = countryID;
            this.CountryName = countryName;
            this.CurrencyID = currencyID;
        }
    }
}