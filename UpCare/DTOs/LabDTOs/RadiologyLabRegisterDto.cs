namespace UpCare.DTOs.LabDTOs
{
    public class RadiologyLabRegisterDto:RegisterDto
    {
        #region RadiologyLab Properties
        public string Location { get; set; }
        public string? AdminId { get; set; }
        #endregion
    }
}
