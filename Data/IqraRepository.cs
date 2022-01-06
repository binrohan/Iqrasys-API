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

        #region POCO
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
        #endregion POCO

        #region User
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
        #endregion User

        #region Solution
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
        #endregion Solution

        #region Message
        public async Task<IReadOnlyList<Message>> GetMessagesAsync(bool isTrashed = false)
        {
            return await _context.Messages
                                .Where(m => m.IsTrashed == isTrashed)
                                .OrderByDescending(u => u.MessageDate)
                                .ToListAsync();
        }

        public async Task<Message> GetMessageAsync(Guid id)
        {
            return await _context.Messages.FirstOrDefaultAsync(m => m.Id == id);
        }
        #endregion Request

        #region Request
        public async Task<IReadOnlyList<Request>> GetRequestsAsync(bool isTrashed = false)
        {
            return await _context.Requests
                                .Where(r => r.IsTrashed == isTrashed)
                                .OrderByDescending(r => r.RequestDate)
                                .ToListAsync();
        }
        public async Task<Request> GetRequestAsync(Guid id)
        {
            return await _context.Requests
                                .Include(r => r.Solution)
                                .FirstOrDefaultAsync(r => r.Id == id);
        }
        #endregion Request
    
        #region Application
        public async Task<IReadOnlyList<Application>> GetApplicationsAsync(bool isTrashed = false)
        {
            var applications = await _context.Applications
                                        .Where(a => a.IsTrashed == isTrashed)
                                        .ToListAsync();

            return applications;
        }

        public async Task<Application> GetApplicationAsync(Guid id)
        {
            var application = await _context.Applications
                                            .Where(a => a.Id == id)
                                            .FirstOrDefaultAsync();
            return application;
        }
        #endregion Application

        #region Quick Request
        public async Task<IReadOnlyList<QuickRequest>> GetQuickRequestsAsync(bool isTrashed = false)
        {
            var applications = await _context.QuickRequests
                                        .Where(a => a.IsTrashed == isTrashed)
                                        .ToListAsync();

            return applications;
        }

        public async Task<QuickRequest> GetQuickRequestAsync(Guid id)
        {
            var request = await _context.QuickRequests
                                            .Where(q => q.Id == id)
                                            .OrderBy(q => q.RequestDate)
                                            .FirstOrDefaultAsync();
            return request;
        }

        public async Task<QuickRequest> GetQuickRequestByPhoneAsync(string phone)
        {
            var request = await _context.QuickRequests
                                            .Where(q => q.Phone == phone)
                                            .FirstOrDefaultAsync();
            return request;
        }
        #endregion Application
    }
}