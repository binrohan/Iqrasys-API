using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using iqrasys.api.Models;

namespace iqrasys.api.Data
{
    public interface IIqraRepository
    {
        void Add<T>(T entity) where T: class;
        void Delete<T>(T entity) where T: class;
        Task<bool> SaveAll();

         Task<User> GetUserAsync(Guid userId);
         Task<IEnumerable<User>> GetUsersAsync();
    }
}