namespace BankSystem.DTOs.UserLoginRecordDTOs
{
    public class UserLoginRecordReadBasicDTO
    {
        public int LoginID { get; set; }
        public int UserID { get; set; }
        public DateTime DateTime { get; set; }

        public UserLoginRecordReadBasicDTO(int loginID, int userID, DateTime dateTime)
        {
            this.LoginID = loginID;
            this.UserID = userID;
            this.DateTime = dateTime;
        }
    }
}