using System.Collections.Generic;
using System.Threading.Tasks;
using iqrasys.api.Models;

namespace iqrasys.api.Data
{
    public interface IIqraRepository
    {
         Task<IEnumerable<User>> GetUsers();
    }
}