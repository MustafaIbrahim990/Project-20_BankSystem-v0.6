namespace BankSystem.DTOs.CountryDTOs
{
    public class CountryUpdateDTO
    {
        //Properties :-
        public string CountryName { get; set; } = string.Empty;
        public int CurrencyID { get; set; } = 0;


        //Constructor :-
        public CountryUpdateDTO(string countryName, int currencyID)
        {
            this.CountryName = countryName;
            this.CurrencyID = currencyID;
        }
    }
}