using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using iqrasys.api.Dtos;
using iqrasys.api.Models;
using Microsoft.EntityFrameworkCore;

namespace iqrasys.api.Data
{
    public class IqraRepository : IIqraRepository
    {
        private DataContext _context { get; set; }
        public IqraRepository(DataContext context)
        {
            _context = context;
        }

        public void Add<T>(T entity) where T : class
        {
            _context.Add(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            _context.Remove(entity);
        }
        public async Task<bool> SaveAll()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<User> GetUserAsync(string userId)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Id == userId);

            return user;
        }

        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            try
            {
                var users = await _context.Set<User>().FromSqlRaw("EXEC Get_Users").ToListAsync();

                return users;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<IReadOnlyList<ArchiveUser>> GetArchiveUsersAsync()
        {
            return await _context.ArchiveUsers
                                .OrderByDescending(u => u.RemoveDate)
                                .ToListAsync();
        }

        public async Task<IReadOnlyList<Solution>> GetSolutionsAsync(bool isTrashed = false)
        {
            return await _context.Solutions
                                .Where(s => s.IsTrashed == isTrashed)
                                .OrderByDescending(s => s.Order)
                                .ToListAsync();
        }

        public async Task<Solution> GetSolutionAsync(Guid id)
        {
            return await _context.Solutions.FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<IReadOnlyList<Message>> GetMessagesAsync(bool isTrashed = false)
        {
            return await _context.Messages
                                .Where(m => m.IsTrashed == isTrashed)
                                .OrderByDescending(u => u.PostDate)
                                .ToListAsync();
        }

        public async Task<Message> GetMessageAsync(Guid id)
        {
            return await _context.Messages.FirstOrDefaultAsync(m => m.Id == id);
        }
    }
}