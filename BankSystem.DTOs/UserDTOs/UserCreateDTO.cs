using System.Text.Json.Serialization;

namespace BankSystem.DTOs.UserDTOs
{
    public class UserCreateDTO
    {
        public int PersonID { get; set; } = 0;
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public int PermissionLevel { get; set; } = 0;

        public UserCreateDTO(int personID, string username, string password, int permissionLevel)
        {
            this.PersonID = personID;
            this.UserName = username;
            this.Password = password;
            this.PermissionLevel = permissionLevel;
        }
    }
}