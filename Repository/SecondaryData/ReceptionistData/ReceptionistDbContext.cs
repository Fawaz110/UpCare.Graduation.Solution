using Core.UpCareUsers;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.SecondaryData.ReceptionistData
{
    public class ReceptionistDbContext : IdentityDbContext<Receptionist>
    {
        public ReceptionistDbContext(DbContextOptions<ReceptionistDbContext> options) : base(options)
        {
        }
    }
}
