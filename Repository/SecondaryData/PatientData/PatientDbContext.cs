using Core.UpCareUsers;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Repository.SecondaryData.PatientData
{
    public class PatientDbContext : IdentityDbContext<Patient>
    {
        public PatientDbContext(DbContextOptions<PatientDbContext> options) : base(options)
        {
        }
    }
}
