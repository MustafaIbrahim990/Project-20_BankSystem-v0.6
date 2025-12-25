namespace BankSystem.DTOs.CountryDTOs
{
    public class CountryCreateDTO
    {
        //Properties :-
        public string CountryName { get; set; } = string.Empty;
        public int CurrencyID { get; set; } = 0;


        //Constructor :-
        public CountryCreateDTO(string countryName, int currencyID)
        {
            this.CountryName = countryName;
            this.CurrencyID = currencyID;
        }
    }
}