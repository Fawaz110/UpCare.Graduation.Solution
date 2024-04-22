using Core.Repositories.Contract;
using Core.UpCareEntities;
using Microsoft.EntityFrameworkCore;
using Repository.UpCareData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly UpCareDbContext _context;

        public AppointmentRepository(
            UpCareDbContext context)
        {
            _context = context;
        }
        public async Task AddAppointmentAsync(PatientAppointment patientAppointment)
            => await _context.Set<PatientAppointment>().AddAsync(patientAppointment);

        public void DeleteAsync(PatientAppointment patientAppointment)
            => _context.Set<PatientAppointment>().Remove(patientAppointment);

        public async Task<List<PatientAppointment>> GetByDoctorIdAsync(string doctorId)
            => await _context.Set<PatientAppointment>().Where(pc => pc.FK_DoctorId == doctorId).ToListAsync();

        public async Task<List<PatientAppointment>> GetByPatientIdAsync(string patientId)
            => await _context.Set<PatientAppointment>().Where(pc => pc.FK_PatientId == patientId).ToListAsync();

        public async Task<List<PatientAppointment>> GetAppointmentsBetweenPatientAndDoctorAsync(string patientId, string doctorId)
            => await _context.Set<PatientAppointment>().Where(pc => pc.FK_DoctorId == doctorId
                                                                  && pc.FK_PatientId == patientId).ToListAsync();

        public async Task<PatientAppointment> GetNextAppointmentAsync(string patientId, string doctorId)
            => await _context.Set<PatientAppointment>().FirstOrDefaultAsync(pc => pc.FK_PatientId == patientId
                                                                                && pc.FK_DoctorId == doctorId
                                                                                && pc.DateTime > DateTime.UtcNow);

        public async Task<PatientAppointment> GetWithSpec(PatientAppointment appointment)
            => await _context.Set<PatientAppointment>().FirstOrDefaultAsync(x => x.FK_PatientId == appointment.FK_PatientId
                                                                               && x.FK_DoctorId == appointment.FK_DoctorId
                                                                               && x.DateTime == appointment.DateTime);
    }
}
