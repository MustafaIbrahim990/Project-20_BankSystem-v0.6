namespace BankSystem.DTOs.UserDTOs
{
    public class UpdatePasswordDTO
    {
        public string UserName { get; set; } = string.Empty;
        public string CurrentPassword { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
        
        
        public UpdatePasswordDTO(string username, string currentPassword, string newPassword)
        {
            this.UserName = username;
            this.CurrentPassword = currentPassword;
            this.NewPassword = newPassword;
        }
    }
}