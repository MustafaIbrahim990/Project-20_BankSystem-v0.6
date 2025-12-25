namespace BankSystem.DTOs.CountryDTOs
{
    public class CountryReadBasicDTO
    {
        //Properties :-
        public int CountryID { get; set; } = 0;
        public string CountryName { get; set; } = string.Empty;
        public int CurrencyID { get; set; } = 0;


        //Constructor :-
        public CountryReadBasicDTO(int countryID, string countryName, int currencyID)
        {
            this.CountryID = countryID;
            this.CountryName = countryName;
            this.CurrencyID = currencyID;
        }
    }
}