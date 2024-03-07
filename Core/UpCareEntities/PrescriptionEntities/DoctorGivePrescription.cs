namespace Core.UpCareEntities.PrescriptionEntities
{
    public class DoctorGivePrescription
    {
        public string FK_PatientId { get; set; }
        public string FK_DoctorId { get; set; }
        public int FK_PrescriptionId { get; set; }
    }
}