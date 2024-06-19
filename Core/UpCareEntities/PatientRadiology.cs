namespace Core.UpCareEntities
{
    public class PatientRadiology
    {
        public string FK_PatientId { get; set; }
        public int FK_RadiologyId { get; set; }
        public string Result { get; set; }
        public DateTime DateTime { get; set; } = DateTime.UtcNow;
    }
}
