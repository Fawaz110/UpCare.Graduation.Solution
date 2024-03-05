using Core.UpCareUsers;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Repository.SecondaryData.NurseData
{
    public class NurseDbContext : IdentityDbContext<Nurse>
    {
        public NurseDbContext(DbContextOptions<NurseDbContext> options) : base(options)
        {
        }
    }
}
