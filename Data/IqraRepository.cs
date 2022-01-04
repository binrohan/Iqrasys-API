using System;
using System.Collections.Generic;
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

        public async Task<User> GetUserAsync(Guid userId)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => String.Equals(u.Id, userId));

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
    }
}