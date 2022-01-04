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

        public async Task<IEnumerable<User>> GetUsers()
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