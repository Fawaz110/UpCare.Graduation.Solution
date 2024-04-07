namespace UpCare.DTOs.LabDTOs
{
    public class CheckupLabRegisterDto:RegisterDto
    {
        #region ChekupLab Properties
        public string Location { get; set; }
        public string? AdminId { get; set; }
        #endregion
    }
}
