namespace BankSystem.DTOs.UserDTOs
{
    public class UserReadBasicDTO
    {
        public int UserID { get; set; } = 0;
        public int PersonID { get; set; } = 0;
        public string UserName { get; set; } = string.Empty;
        public int PermissionLevel { get; set; } = 0;

        public UserReadBasicDTO(int userID, int personID, string userName, int permissionLevel)
        {
            this.UserID = userID;
            this.PersonID = personID;
            this.UserName = userName;
            this.PermissionLevel = permissionLevel;
        }
    }
}