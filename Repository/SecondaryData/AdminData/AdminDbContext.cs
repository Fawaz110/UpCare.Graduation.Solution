using Core.UpCareUsers;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.SecondaryData.AdminData
{
    public class AdminDbContext : IdentityDbContext<Admin>
    {
        public AdminDbContext(DbContextOptions<AdminDbContext> options) : base(options)
        {
        }
    }
}
