namespace UpCare.DTOs.StaffDTOs
{
    public class DoctorRegisterDto : RegisterDto
    {
        #region Doctor Properties
        public string Speciality { get; set; }
        public bool IsSurgeon { get; set; }
        public decimal ConsultationPrice { get; set; }
        public decimal AppointmentPrice { get; set; }
        public string? Address { get; set; }
        public string? AdminId { get; set; }
        #endregion
    }
}
