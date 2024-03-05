﻿namespace Core.UpCareEntities
{
    public class NurseCare
    {
        public string FK_NurseId {  get; set; }
        public string FK_PatientId { get; set; }
        public string FK_RoomId { get; set; }
        public string? Suger { get; set; }
        public string? BloodPresure { get; set; }
        public DateTime DateTime { get; set; } = DateTime.UtcNow;
        public string Notes { get; set; } = "No notes left. (Patient case is stable)";
    }
}
