using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using iqrasys.api.Data;
using iqrasys.api.Dtos;
using iqrasys.api.Models;
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

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var usersFromRepo = await _repo.GetUsersAsync();
            var users = _mapper.Map<IEnumerable<UserForReturnDto>>(usersFromRepo);

            return Ok(users);
        }

        [HttpDelete("{userId}")]
        public async Task<IActionResult> RemoveUser(Guid userId)
        {
            var user = await _repo.GetUserAsync(userId);

            var userForArchive = _mapper.Map<ArchiveUser>(user);

            _repo.Add<ArchiveUser>(userForArchive);

            if (!await _repo.SaveAll())
            {
                throw new Exception("User remove failed");
            }

            _repo.Delete(user);

            if (await _repo.SaveAll())
            {
                var userForReturn = _mapper.Map<UserForReturnDto>(user);
                return Ok(userForReturn);
            }
            _repo.Delete(userForArchive);
            throw new Exception("User remove failed");
        }
    }
}