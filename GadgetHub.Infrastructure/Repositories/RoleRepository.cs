using GadgetHub.Domain.Entities;
using GadgetHub.Domain.Interfaces;
using GadgetHub.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GadgetHub.Infrastructure.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly MyDBcontext _context;
        private readonly ILogger<RoleRepository> _logger;

        public RoleRepository(MyDBcontext context, ILogger<RoleRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Role?> GetByIdAsync(int id)
        {
            return await _context.Roles
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<Role?> GetByNameAsync(string name)
        {
            return await _context.Roles
                .FirstOrDefaultAsync(r => r.Name == name);
        }

        public async Task<IEnumerable<Role>> GetAllAsync()
        {
            return await _context.Roles
                .OrderBy(r => r.Name)
                .ToListAsync();
        }

        public async Task<Role> AddAsync(Role role)
        {
            _context.Roles.Add(role);
            await _context.SaveChangesAsync();
            return role;
        }

        public async Task UpdateAsync(Role role)
        {
            _context.Roles.Update(role);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Role role)
        {
            _context.Roles.Remove(role);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Roles.AnyAsync(r => r.Id == id);
        }
    }
}