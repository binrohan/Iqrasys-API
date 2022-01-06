using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using iqrasys.api.Models;

namespace iqrasys.api.Data
{
    public interface IIqraRepository
    {
        void Add<T>(T entity) where T : class;
        void Delete<T>(T entity) where T : class;
        Task<bool> SaveAll();

        Task<User> GetUserAsync(string userId);
        Task<IEnumerable<User>> GetUsersAsync();
        Task<IReadOnlyList<ArchiveUser>> GetArchiveUsersAsync();

        Task<IReadOnlyList<Solution>> GetSolutionsAsync(bool isTrashed = false);
        Task<Solution> GetSolutionAsync(Guid id);

        Task<IReadOnlyList<Message>> GetMessagesAsync(bool isTrashed = false);
        Task<Message> GetMessageAsync(Guid id);

        #region Request
        Task<IReadOnlyList<Request>> GetRequestsAsync(bool isTrashed = false);
        Task<Request> GetRequestAsync(Guid id);
        #endregion Request
    }
}