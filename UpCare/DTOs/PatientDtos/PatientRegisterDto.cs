namespace UpCare.DTOs.Patient
{
    public class PatientRegisterDto : RegisterDto
    {
        #region Patient Properties
        public string Gender { get; set; }
        public string? Address { get; set; }
        public DateTime DateOfBirth  { get; set; }
        public string? BloodType { get; set; }
        public string? ReceptionistId { get; set; }
        #endregion
    }
}
