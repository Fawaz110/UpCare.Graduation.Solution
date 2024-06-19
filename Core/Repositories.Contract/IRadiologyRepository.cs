using Core.Entities.UpCareEntities;
using Core.UpCareEntities;

namespace Core.Repositories.Contract
{
    public interface IRadiologyRepository : IGenericRepository<Radiology>
    {
        Task<Radiology> GetByNameAsync(string name);
        Task AddPatientResult(PatientRadiology patientCheckup);
        Task<List<PatientRadiology>> GetAllResults();
    }
}
