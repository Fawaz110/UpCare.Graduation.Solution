using Core.UpCareUsers;

namespace UpCare.DTOs.PatientDtos
{
    public class PatientHistoryDto
    {
        public Core.UpCareUsers.Patient PatientInfo { get; set; }

        public List<MedicineHistory> Medicines { get; set; }
        public List<CheckupHistory> Checkups { get; set; }
        public List<RadiologyHistory> Radiologies { get; set; }
        public List<ConversationDto> Conversations { get; set; }

    }
}
