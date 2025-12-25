namespace BankSystem.DTOs.UserDTOs
{
    public class UserUpdateDTO
    {
        public string UserName { get; set; } = string.Empty;
        public int PermissionLevel { get; set; } = 0;

        public UserUpdateDTO(string userName, int permissionLevel)
        {
            this.UserName = userName;
            this.PermissionLevel = permissionLevel;
        }
    }
}