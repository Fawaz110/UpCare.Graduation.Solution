﻿using Core.Entities.UpCareEntities;
using Core.Repositories.Contract;
using Microsoft.EntityFrameworkCore;
using Repository.UpCareData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class RadiologyRepository : GenericRepository<Radiology>, IRadiologyRepository
    {
        private readonly UpCareDbContext _context;

        public RadiologyRepository(UpCareDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Radiology> GetByNameAsync(string name)
            => await _context.Radiologies.FirstOrDefaultAsync(x => x.Name == name);
    }
}
