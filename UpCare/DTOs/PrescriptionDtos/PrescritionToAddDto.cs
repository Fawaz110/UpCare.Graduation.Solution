namespace UpCare.DTOs.PrescriptionDtos
{
    public class PrescritionToAddDto
    {
        public string Diagnosis { get; set; }
        public string Details { get; set; }
        public string Advice { get; set; }
        public string FK_DoctorId { get; set; }
        public string FK_PatientId { get; set; }
        public List<int> FK_MedicineIds { get; set; } = new List<int>();
        public List<int> FK_CheckupsIds { get; set; } = new List<int>();
        public List<int> FK_RadiologiesIds { get; set; } = new List<int>();
    }
}
