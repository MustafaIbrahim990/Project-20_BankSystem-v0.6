namespace BankSystem.DTOs.CurrencyDTOs
{
    public class CurrencyReadDTO
    {
        //Properties :-
        public int CurrencyID { get; set; } = 0;
        public string CurrencyName { get; set; } = string.Empty;
        public string CurrencyCode { get; set; } = string.Empty;
        public decimal CurrencyRate { get; set; } = 0;


        //Constructor :-
        public CurrencyReadDTO(int currencyID, string currencyName, string currencyCode, decimal currencyRate)
        {
            this.CurrencyID = currencyID;
            this.CurrencyName = currencyName;
            this.CurrencyCode = currencyCode;
            this.CurrencyRate = currencyRate;
        }
    }
}