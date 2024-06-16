using Core.Entities.UpCareEntities;
using Core.Services.Contract;
using Core.UnitOfWork.Contract;
using Core.UpCareEntities;
using Core.UpCareUsers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UpCare.DTOs;
using UpCare.DTOs.RoomDtos;
using UpCare.Errors;

namespace UpCare.Controllers
{
    public class RoomController : BaseApiController
    {
        private readonly UserManager<Receptionist> _receptionistManager;
        private readonly UserManager<Doctor> _doctorManager;
        private readonly UserManager<Patient> _patientManager;
        private readonly IRoomService _roomService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<RoomController> _logger;

        public RoomController(
            UserManager<Receptionist> receptionistManager,
            UserManager<Doctor> doctorManager,
            UserManager<Patient> patientManager,
            IRoomService roomService,
            IUnitOfWork unitOfWork,
            ILogger<RoomController> logger)
        {
            _receptionistManager = receptionistManager;
            _doctorManager = doctorManager;
            _patientManager = patientManager;
            _roomService = roomService;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        [HttpGet("all")] // GET: /api/room/all
        public async Task<ActionResult<List<RoomDto>>> GetAll()
        {
            var rooms = await _unitOfWork.Repository<Room>().GetAllAsync();

            if (rooms.Count == 0) 
                return NotFound(new ApiResponse(404, "no data found"));
            
            var listToReturn = new List<RoomDto>();

            foreach (var room in rooms)
            {
                var item = new RoomDto
                {
                    Id = room.Id,
                    AvailableBedsNumber = room.AvailableBeds,
                    NumberOfBeds = room.NumberOfBeds,
                    PricePerNight = room.PricePerNight,
                    Receptionist = await _receptionistManager.FindByIdAsync(room.FK_ReceptionistId),
                    PatientBooking = await GetPatientsInRoom(room.Id)
                };

                listToReturn.Add(item);
            }

            return Ok(listToReturn);
            
        }

        [HttpGet("{id}")] // GET: /api/room/{id}
        public async Task<ActionResult<RoomDto>> GetById(int id)
        {
            var room = await _unitOfWork.Repository<Room>().GetByIdAsync(id);

            if (room is null) return NotFound(new ApiResponse(404, "no data found"));

            var receptionist = await _receptionistManager.FindByIdAsync(room.FK_ReceptionistId);

            var booking = await _roomService.GetAllPatientBookingAsync();

            var patintsIds = booking.Where(x => x.FK_RoomId == id).Select(x => x.FK_PatientId).Distinct().ToList();

            var patients = await GetPatientsInRoom(room.Id);

            var roomToReturn = new RoomDto
            {
                Id = room.Id,
                NumberOfBeds = room.NumberOfBeds,
                AvailableBedsNumber = room.AvailableBeds,
                PricePerNight = room.PricePerNight,
                Receptionist = receptionist,
                PatientBooking = patients
            };

            return Ok(roomToReturn);
        }
        
        [HttpPost("add")] // POST: /api/room/add
        public async Task<ActionResult<SucceededToAdd>> AddRoom([FromBody] Room room)
        {
            var receptionist = await _receptionistManager.FindByIdAsync(room.FK_ReceptionistId);

            if (receptionist is null)
                return NotFound(new ApiResponse(404, "no receptionist matches found"));
            
            var record = await _roomService.AddRoomAsync(room);

            if (record is null)
                return BadRequest(new ApiResponse(400, "an error occured during adding data"));

            return Ok(new SucceededToAdd
            {
                Message = "success",
                Data = new RoomDto{
                    Id = record.Id,
                    NumberOfBeds = record.NumberOfBeds,
                    PricePerNight = record.PricePerNight,
                    AvailableBedsNumber = record.AvailableBeds,
                    Receptionist = await _receptionistManager.FindByIdAsync(record.FK_ReceptionistId),
                    PatientBooking = new List<PatientBookingDto>()
                }
            });
        }

        [HttpPost("book")] // POST: /api/room/book
        public async Task<ActionResult<SucceededToAdd>> BookRoom([FromBody] PatientBookRoom model)
        {
            var patient = await _patientManager.FindByIdAsync(model.FK_PatientId);

            if (patient is null) return BadRequest(new ApiResponse(400, "invalid data entered"));

            var doctor = await _doctorManager.FindByIdAsync(model.FK_DoctorId);

            if (doctor is null) return BadRequest(new ApiResponse(400, "invalid data entered"));

            var room = await _unitOfWork.Repository<Room>().GetByIdAsync(model.FK_RoomId);

            if (room is null) return BadRequest(new ApiResponse(400, "invalid data entered"));

            var result = await _roomService.BookRoomAsync(model);

            if (result is null) 
                return BadRequest(new ApiResponse(400, "an error occured while adding data"));


            var patients = await GetPatientsInRoom(room.Id);

            return Ok(new SucceededToAdd
            {
                Message = "success",
                Data = new RoomDto
                {
                    Id = room.Id,
                    NumberOfBeds = room.NumberOfBeds,
                    PricePerNight = room.PricePerNight,
                    AvailableBedsNumber = room.AvailableBeds,
                    Receptionist = await _receptionistManager.FindByIdAsync(room.FK_ReceptionistId),
                    PatientBooking = patients
                }
            });
        }

        [HttpPost("end/booking")] // POST: /api/room/end/booking
        public async Task<ActionResult<SucceededToAdd>> EndBooking([FromBody] PatientBookRoom model)
        {
            var patient = await _patientManager.FindByIdAsync(model.FK_PatientId);

            if (patient is null)
                return NotFound(new ApiResponse(404, "no data matches found"));

            var doctor = await _doctorManager.FindByIdAsync(model.FK_DoctorId);

            if (doctor is null)
                return NotFound(new ApiResponse(404, "no data matches found"));

            var room = await _unitOfWork.Repository<Room>().GetByIdAsync(model.FK_RoomId);

            if (room is null) 
                return NotFound(new ApiResponse(404, "no data matches found"));

            var result = await _roomService.EndPatientRoomBooking(model);

            if (result is null)
                return BadRequest(new ApiResponse(400, "an error occured during update data"));

            var objectToReturn = new RoomDto
            {
                AvailableBedsNumber = room.AvailableBeds,
                Id = room.Id,
                NumberOfBeds = room.NumberOfBeds,
                PricePerNight = room.PricePerNight,
                Receptionist = await _receptionistManager.FindByIdAsync(room.FK_ReceptionistId),
                PatientBooking = await GetPatientsInRoom(room.Id)
            };

            return Ok(new SucceededToAdd
            {
                Message = "success",
                Data = objectToReturn
            });
        }

        [HttpPost("update")] // POST: /api/room/update
        public async Task<ActionResult<SucceededToAdd>> UpdateRoom([FromBody]Room model)
        {
            var room = await _unitOfWork.Repository<Room>().GetByIdAsync(model.Id);


            if (room is null)
                return NotFound(new ApiResponse(404, "no data matches found"));

            if ((model.NumberOfBeds - room.NumberOfBeds) < 0)
                return BadRequest(new ApiResponse(400, "can't set that number of bed right now"));

            room.PricePerNight = model.PricePerNight;
            room.AvailableBeds += (model.NumberOfBeds - room.NumberOfBeds);
            room.NumberOfBeds = model.NumberOfBeds;

            _unitOfWork.Repository<Room>().Update(room);

            var result = await _unitOfWork.CompleteAsync();

            if (result <= 0)
                return BadRequest(new ApiResponse(400, "bad request"));

            return Ok(new RoomDto
            {
                Id = room.Id,
                AvailableBedsNumber = room.AvailableBeds,
                NumberOfBeds = room.NumberOfBeds,
                PricePerNight = room.PricePerNight,
                Receptionist = await _receptionistManager.FindByIdAsync(room.FK_ReceptionistId),
                PatientBooking = await GetPatientsInRoom(room.Id)
            });
        }

        [HttpDelete("delete/{id}")] // DELETE: /api/room/delete
        public async Task<ActionResult<SucceededToAdd>> DeleteRoom(int id)
        {
            var room = await _unitOfWork.Repository<Room>().GetByIdAsync(id);

            if (room is null) 
                return NotFound(new ApiResponse(404, "no data matches found"));

            _unitOfWork.Repository<Room>().Delete(room);

            // delete patients book room records where room id
            
            await _unitOfWork.CompleteAsync();

            return Ok(new ApiResponse(200, "room deleted successfully"));
        }

        // Method to get patients in specific room that no ended there booking
        private async Task<List<PatientBookingDto>> GetPatientsInRoom(int roomId)
        {
            List<PatientBookingDto> patients = new List<PatientBookingDto>();

            var allRecords = await _roomService.GetAllPatientBookingAsync();

            var patientsIds = allRecords.Where(x => x.EndDate == DateTime.MinValue && x.FK_RoomId == roomId).Select(x => x.FK_PatientId).Distinct().ToList();

            foreach (var id in patientsIds)
            {
                var itemToAdd = new PatientBookingDto
                {
                    Patient = await _patientManager.FindByIdAsync(id),
                    RoomInfo = await _roomService.GetSpecificBookingAsync(id, roomId)
                };

                if (!(patients.Contains(itemToAdd)) && itemToAdd.RoomInfo.EndDate == DateTime.MinValue)
                    patients.Add(itemToAdd);
            }

            return patients;
        }
    }
}
