namespace BankSystem.Domain.Models
{
    public class Currency
    {
        //Properties :-
        public int CurrencyID { get; set; } = 0;
        public string CurrencyName { get; set; } = string.Empty;
        public string CurrencyCode { get; set; } = string.Empty;
        public decimal CurrencyRate { get; set; } = 0;


        //for Add New and Update :-
        public Currency(string currencyName, string currencyCode, decimal currencyRate)
        {
            this.CurrencyName = currencyName;
            this.CurrencyCode = currencyCode;
            this.CurrencyRate = currencyRate;
        }


        //for Read :-
        public Currency(int currencyID, string currencyName, string currencyCode, decimal currencyRate)
        {
            this.CurrencyID = currencyID;
            this.CurrencyName = currencyName;
            this.CurrencyCode = currencyCode;
            this.CurrencyRate = currencyRate;
        }


        //for Update Rate :-
        public Currency(decimal newCurrencyRate)
        {
            this.CurrencyRate = newCurrencyRate;
        }
    }
}