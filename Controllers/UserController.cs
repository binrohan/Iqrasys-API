using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using iqrasys.api.Data;
using iqrasys.api.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace iqrasys.api.Controllers
{
    public class UserController : ApiController
    {
        private readonly IMapper _mapper;
        private readonly IIqraRepository _repo;
        public UserController(IIqraRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<IActionResult> GetUsers()
        {
            var usersFromRepo = await _repo.GetUsers();
            var users = _mapper.Map<IEnumerable<UserForReturnDto>>(usersFromRepo);

            return Ok(users);
        }
    }
}