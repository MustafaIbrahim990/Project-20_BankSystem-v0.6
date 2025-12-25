namespace BankSystem.Domain.Models
{
    public class User
    {
        public int UserID { get; set; } = 0;
        public int PersonID { get; set; } = 0;
        public string UserName { get; set; } = string.Empty;
        public string PasswordHashed { get; set; } = string.Empty;
        public string Salt { get; set; } = string.Empty;
        public int PermissionLevel { get; set; } = 0;

        public User(int userID, int personID, string userName, string passwordHashed, string salt, int permissionLevel)
        {
            this.UserID = userID;
            this.PersonID = personID;
            this.UserName = userName;
            this.PasswordHashed = passwordHashed;
            this.Salt = salt;
            this.PermissionLevel = permissionLevel;
        }
        public User(int personID, string userName, string passwordHashed, string salt, int permissionLevel)
        {
            this.PersonID = personID;
            this.UserName = userName;
            this.PasswordHashed = passwordHashed;
            this.Salt = salt;
            this.PermissionLevel = permissionLevel;
        }


        //For Update :-
        public User(string userName, int permissionLevel)
        {
            this.UserName = userName;
            this.PermissionLevel = permissionLevel;
        }


        //For Change Password :-
        public User(string userName, string newPasswordHashed, string newSalt)
        {
            this.UserName = userName;
            this.PasswordHashed = newPasswordHashed;
            this.Salt = newSalt;
        }
    }
}