using BankSystem.DTOs.ClientDTOs;

namespace BankSystem.DTOs.ClientDTOs
{
    public class UpdatePinCodeByClientIdDTO
    {
        public int ClientID { get; set; } = 0;
        public string CurrentPinCode { get; set; } = string.Empty;
        public string NewPinCode { get; set; } = string.Empty;

        public UpdatePinCodeByClientIdDTO(int clientId, string currentPinCode, string newPinCode)
        {
            this.ClientID = clientId;
            this.CurrentPinCode = currentPinCode;
            this.NewPinCode = newPinCode;
        }
    }
}