using System;
using System.Threading.Tasks;
using AutoMapper;
using iqrasys.api.Data;
using iqrasys.api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace iqrasys.api.Controllers
{
    public class RequestsController : ApiController
    {
        private readonly IIqraRepository _repo;
        private readonly IMapper _mapper;
        public RequestsController(IIqraRepository repo, IMapper mapper)
        {
            _mapper = mapper;
            _repo = repo;
        }

        [HttpGet]
        public async Task<IActionResult> GetRequests([FromQuery] bool isTrashed = false)
        {
            var requests = await _repo.GetRequestsAsync(isTrashed);

            return Ok(requests);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRequest(Guid id)
        {
            var request = await _repo.GetRequestAsync(id);

            if(request == null) return NotFound("Request not found.");
            
            return Ok(request);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Postrequest(Request request)
        {
            if(string.IsNullOrEmpty(request.Name)) return BadRequest("Name is required");

            if(string.IsNullOrEmpty(request.Phone)) return BadRequest("Phone number is required");

            if(request.Text.Length == 0) return BadRequest("Request text is required");

            _repo.Add(request);

            if(await _repo.SaveAll())
                return CreatedAtAction(nameof(GetRequest), new {id = request.Id}, request);

            throw new Exception("Request send failed!");
        }

        [HttpPut("Trash/{id}")]
        public async Task<IActionResult> TrashRequest(Guid id)
        {
            var request = await _repo.GetRequestAsync(id);

            if(request == null) return NotFound("Request not found");

            if(request.IsTrashed) return BadRequest("Request already removed");

            request.IsTrashed = true;

            if(await _repo.SaveAll()) return NoContent();

            throw new Exception("Request remove failed!");
        }

        [HttpPut("Restore/{id}")]
        public async Task<IActionResult> RestoreRequest(Guid id)
        {
            var request = await _repo.GetRequestAsync(id);

            if(request == null) return NotFound("Request not found.");

            if(!request.IsTrashed) return BadRequest("Request already storeded.");

            request.IsTrashed = false;

            if(await _repo.SaveAll()) return Ok(request);

            throw new Exception("Request restore failed!");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRequest(Guid id)
        {
            var request = await _repo.GetRequestAsync(id);

            if(request == null) return NotFound("Request not found");

            if(!request.IsTrashed) return BadRequest("Move the request to the trash first");

           _repo.Delete(request);

            if(await _repo.SaveAll()) return NoContent();

            throw new Exception("Request delete failed!");
        }
    }
}