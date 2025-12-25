namespace BankSystem.DTOs.PersonDTOs
{
    public class PersonReadBasicDTO
    {
        //Properties :-
        public int PersonID { get; } = 0;
        public string FirstName { get; } = string.Empty;
        public string LastName { get; } = string.Empty;
        public string Email { get; } = string.Empty;
        public string Phone { get; } = string.Empty;
        public int CountryID { get; } = 0;


        //Constructor :-
        public PersonReadBasicDTO(int personID, string firstName, string lastName, int countryID, string email, string phone)
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