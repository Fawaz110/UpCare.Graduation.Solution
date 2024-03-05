using Core.UpCareUsers;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.SecondaryData.LabsData.RadiologyLabData
{
    public class RadiologyDbContext : IdentityDbContext<RadiologyLab>
    {
        public RadiologyDbContext(DbContextOptions<RadiologyDbContext> options) : base(options)
        {
        }
    }
}
