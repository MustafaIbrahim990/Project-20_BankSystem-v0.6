namespace BankSystem.DTOs.UserDTOs
{
    public class UserReadDetailsDTO
    {
        public int UserID { get; set; } = 0;
        public string? UserName { get; set; } = string.Empty;
        public string? FirstName { get; set; } = string.Empty;
        public string? LastName { get; set; } = string.Empty;
        public string? CountryName { get; set; } = string.Empty;
        public string? Phone { get; set; } = string.Empty;
        public string? Email { get; set; } = string.Empty;
        public int PermissionLevel { get; set; } = 0;
        public string FullName
        {
            get { return ((FirstName ?? "").Trim() + " " + (LastName ?? "").Trim()).Trim(); }
        }

        public UserReadDetailsDTO(int userID, string? userName, string? firstName, string? lastName, string? countryName, string? phone, string? email, int permissionLevel)
        {
            this.UserID = userID;
            this.UserName = userName;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.CountryName = countryName;
            this.Phone = phone;
            this.Email = email;
            this.PermissionLevel = permissionLevel;
        }
    }
}