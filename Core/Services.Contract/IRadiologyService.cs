﻿using Core.Entities.UpCareEntities;

namespace Core.Services.Contract
{
    public interface IRadiologyService
    {
        Task<ICollection<Radiology>> GetAllAsync();
        Task Update(Radiology entity);
        Task<int> DeleteAsync(int id);
        Task<Radiology> AddAsync(Radiology entity);
        Task<Radiology> GetByIdAsync(int id);
        Task<Radiology> GetByName(string name);
    }
}
