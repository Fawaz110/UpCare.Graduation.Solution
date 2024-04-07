namespace UpCare.DTOs.StaffDTOs
{
    public class ReceptionistRegisterDto:RegisterDto
    {
        #region Receptionist Properties
        public string? Address { get; set; }
        public string? AdminId { get; set; }
        #endregion
    }
}
