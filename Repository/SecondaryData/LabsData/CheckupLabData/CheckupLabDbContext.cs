using Core.UpCareUsers;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.SecondaryData.LabsData.CheckupLabData
{
    public class CheckupLabDbContext : IdentityDbContext<CheckupLab>
    {
        public CheckupLabDbContext(DbContextOptions<CheckupLabDbContext> options) : base(options)
        {
        }
    }
}
