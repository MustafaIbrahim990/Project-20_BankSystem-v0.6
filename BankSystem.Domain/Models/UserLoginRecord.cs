namespace BankSystem.Domain.Models
{
    public class UserLoginRecord
    {
        public int LoginID {  get; set; }
        public int UserID { get; set; }
        public DateTime DateTime { get; set; }

        public UserLoginRecord(int loginID, int userID, DateTime dateTime)
        {
            this.LoginID = loginID;
            this.UserID = userID;
            this.DateTime = dateTime;
        }
    }
}