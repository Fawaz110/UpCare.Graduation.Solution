namespace UpCare.DTOs.StaffDTOs
{
    public class NurseRegisterDto:RegisterDto
    {
        #region Nurse Properties
        public string? Address { get; set; }
        public string? AdminId { get; set; }
        #endregion
    }
}
