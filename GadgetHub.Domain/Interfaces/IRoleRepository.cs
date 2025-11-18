using GadgetHub.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace GadgetHub.Domain.Interfaces
{
    public interface IRoleRepository
    {
        Task<Role?> GetByIdAsync(int id);
        Task<Role?> GetByNameAsync(string name);
        Task<IEnumerable<Role>> GetAllAsync();
        Task<Role> AddAsync(Role role);
        Task UpdateAsync(Role role);
        Task DeleteAsync(Role role);
        Task<bool> ExistsAsync(int id);
    }
}
