namespace BankSystem.Domain.Models
{
    public class Person
    {
        public int PersonID { get; set; } = 0;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public int CountryID { get; set; } = 0;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;


        //Constructor :-
        public Person(string firstName, string lastName, int countryID, string email, string phone)
        {
            this.FirstName = firstName;
            this.LastName = lastName;
            this.CountryID = countryID;
            this.Email = email;
            this.Phone = phone;
        }
        public Person(int personID, string firstName, string lastName, int countryID, string email, string phone)
        {
            this.PersonID = personID;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.CountryID = countryID;
            this.Email = email;
            this.Phone = phone;
        }
    }
}