namespace BankSystem.DTOs.CurrencyDTOs
{
    public class CurrencyCreateDTO
    {
        //Properties :-
        public string CurrencyName { get; set; } = string.Empty;
        public string CurrencyCode { get; set; } = string.Empty;
        public decimal CurrencyRate { get; set; } = 0;


        //Constructor :-
        public CurrencyCreateDTO(string currencyName, string currencyCode, decimal currencyRate)
        {
            this.CurrencyName = currencyName;
            this.CurrencyCode = currencyCode;
            this.CurrencyRate = currencyRate;
        }
    }
}