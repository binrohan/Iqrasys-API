using System;
using System.Threading.Tasks;
using AutoMapper;
using iqrasys.api.Data;
using iqrasys.api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace iqrasys.api.Controllers
{
    [AllowAnonymous]
     public class QuickRequestsController : ApiController
    {
        private readonly IIqraRepository _repo;
        private readonly IMapper _mapper;
        public QuickRequestsController(IIqraRepository repo, IMapper mapper)
        {
            _mapper = mapper;
            _repo = repo;
        }

        [HttpGet]
        public async Task<IActionResult> GetQuickRequests([FromQuery] bool isTrashed = false)
        {
            var requests = await _repo.GetQuickRequestsAsync(isTrashed);

            return Ok(requests);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetQuickRequest(Guid id)
        {
            var request = await _repo.GetQuickRequestAsync(id);

            if(request == null)
                return NotFound("Quick request not found.");
            
            return Ok(request);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> PostQuickRequest(QuickRequest request)
        {
            if(string.IsNullOrEmpty(request.Phone)) return BadRequest("Phone number required");

            var existingRequest =  await _repo.GetQuickRequestByPhoneAsync(request.Phone);

            if(existingRequest != null && !existingRequest.IsSeen)
                return BadRequest("Already requested!");

            _repo.Add(request);

            if(await _repo.SaveAll())
                return CreatedAtAction(nameof(GetQuickRequest), new {id = request.Id}, request);

            throw new Exception("Quick request send failed!");
        }

        [HttpPut("Seen/{id}")]
        public async Task<IActionResult> ToggleSeenStatus(Guid id)
        {
            var request = await _repo.GetQuickRequestAsync(id);

            if(request == null) return NotFound("Quick request not found");

            if(request.IsTrashed) return BadRequest("Quick request moved to trash");

            request.IsSeen = !request.IsSeen;

            if(await _repo.SaveAll()) return NoContent();

            throw new Exception("Quick request mark as seen failed!");
        }

        [HttpPut("trash/{id}")]
        public async Task<IActionResult> RemoveQuickRequest(Guid id)
        {
            var request = await _repo.GetQuickRequestAsync(id);

            if(request == null) return NotFound("Quick request not found");

            if(request.IsTrashed) return BadRequest("Quick request already removed");

            request.IsTrashed = true;

            if(await _repo.SaveAll()) return NoContent();

            throw new Exception("Quick request remove failed!");
        }

        [HttpPut("Restore/{id}")]
        public async Task<IActionResult> RestoreQuickRequest(Guid id)
        {
            var request = await _repo.GetQuickRequestAsync(id);

            if(request == null) return NotFound("Quick request not found");

            if(!request.IsTrashed) return BadRequest("Quick request already storeded");

            request.IsTrashed = false;

            if(await _repo.SaveAll()) return Ok(request);

            throw new Exception("Quick request restore failed!");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteQuickRequest(Guid id)
        {
            var request = await _repo.GetQuickRequestAsync(id);

            if(request == null) return NotFound("Quick request not found");

            if(!request.IsTrashed) return BadRequest("Move the request to the trash first");

           _repo.Delete(request);

            if(await _repo.SaveAll()) return NoContent();

            throw new Exception("Quick request delete failed!");
        }
    }
}