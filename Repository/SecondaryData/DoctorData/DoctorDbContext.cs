using Core.UpCareUsers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.SecondaryData.DoctorData
{
    public class DoctorDbContext : IdentityDbContext<Doctor>
    {
        public DoctorDbContext(DbContextOptions<DoctorDbContext> options) : base(options)
        {
        }
    }
}
