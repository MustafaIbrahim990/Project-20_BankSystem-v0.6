namespace BankSystem.DTOs.UserLoginRecordDTOs
{
    public class UserLoginRecordReadDetailsDTO
    {
        public int LoginID { get; set; } = 0;
        public DateTime? DateTime { get; set; } = new DateTime();
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

        public UserLoginRecordReadDetailsDTO(int loginID, DateTime? dateTime, string? firstName, string? lastName, string? userName, string? countryName, string? phone, string? email, int permissionLevel)
        {
            this.LoginID = loginID;
            this.DateTime = dateTime;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.UserName = userName;
            this.CountryName = countryName;
            this.Phone = phone;
            this.Email = email;
            this.PermissionLevel = permissionLevel;
        }
    }
}