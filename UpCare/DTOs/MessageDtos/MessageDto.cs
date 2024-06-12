using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace UpCare.DTOs.MessageDtos
{
    public enum MessageType
    {
        FromPatientToDoctor,
        FromDoctorToPatient,
        FromAdminToDoctor,
        FromDoctorToAdmin,
    };
    public class MessageDto
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime DateTime { get; set; } = DateTime.UtcNow;
        public string SenderId { get; set; }
        public string ReceiverId { get; set; }
        public MessageType MessageType { get; set; }
    }
}
