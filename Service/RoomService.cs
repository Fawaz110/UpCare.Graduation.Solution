using Core.Entities.UpCareEntities;
using Core.Repositories.Contract;
using Core.Services.Contract;
using Core.UnitOfWork.Contract;
using Core.UpCareEntities;
using Core.UpCareUsers;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class RoomService : IRoomService
    {
        private readonly UserManager<Patient> _patientManager;
        private readonly UserManager<Doctor> _doctorManager;
        private readonly IPatientRoomRepository _patientRoomRepository;
        private readonly IUnitOfWork _unitOfWork;

        public RoomService(
            UserManager<Patient> patientManager,
            UserManager<Doctor> doctorManager,
            IPatientRoomRepository patientRoomRepository,
            IUnitOfWork unitOfWork)
        {
            _patientManager = patientManager;
            _doctorManager = doctorManager;
            _patientRoomRepository = patientRoomRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Room> AddRoomAsync(Room room)
        {
            room.AvailableBeds = room.NumberOfBeds;

            await _unitOfWork.Repository<Room>().Add(room);

            var result = await _unitOfWork.CompleteAsync();

            if (result <= 0)
                return null;

            return room;
        }

        public async Task<PatientBookRoom> BookRoomAsync(PatientBookRoom patientBookRoom)
        {
            var room = await _unitOfWork.Repository<Room>().GetByIdAsync(patientBookRoom.FK_RoomId);

            if (room.AvailableBeds < patientBookRoom.NumberOfRecievedBeds)
                return null;

            room.AvailableBeds -= patientBookRoom.NumberOfRecievedBeds;

            _unitOfWork.Repository<Room>().Update(room);

            var result = await _unitOfWork.CompleteAsync();

            if (result <= 0) 
                return null;

            await _patientRoomRepository.AddBookingRoomAsync(patientBookRoom);

            result = await _unitOfWork.CompleteAsync();

            if (result <= 0) return null;

            return patientBookRoom;
        }

        public async Task<List<PatientBookRoom>> GetAllPatientBookingAsync()
            => await _patientRoomRepository.GetAllPatientRoomsAsync();

        public async Task<PatientBookRoom> EndPatientRoomBooking(PatientBookRoom patientBookRoom)
        {
            var room = await _unitOfWork.Repository<Room>().GetByIdAsync(patientBookRoom.FK_RoomId);

            room.AvailableBeds += patientBookRoom.NumberOfRecievedBeds;

            _unitOfWork.Repository<Room>().Update(room);

            var result = await _unitOfWork.CompleteAsync();

            if (result <= 0) 
                return null;

            var record = await _patientRoomRepository.GetSpecificRecord(patientBookRoom);

            if (record is null) 
                return null;

            record.EndDate = DateTime.Now;

            _patientRoomRepository.UpdatePatintBookRoom(record);

            result = await _unitOfWork.CompleteAsync();

            if (result <= 0) 
                return null;

            return record;
        }

        public async Task<PatientBookRoom> GetSpecificBookingAsync(string patientId, int roomId)
            => await _patientRoomRepository.GetSpecificBookingAsync(patientId, roomId);
    }
}
