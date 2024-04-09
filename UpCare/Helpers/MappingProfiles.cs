using AutoMapper;
using Core.UpCareEntities;
using UpCare.DTOs;

namespace UpCare.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Medicine, MedicineDto>().ReverseMap();
        }
    }
}
