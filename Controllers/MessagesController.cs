using System;
using System.Threading.Tasks;
using AutoMapper;
using iqrasys.api.Data;
using iqrasys.api.Models;
using iqrasys.api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace iqrasys.api.Controllers
{
    public class MessagesController : ApiController
    {
        private readonly IIqraRepository _repo;
        private readonly IMapper _mapper;
        private readonly IMailService _mail;
        public MessagesController(
            IIqraRepository repo,
            IMapper mapper,
            IMailService mail)
        {
            _mail = mail;
            _mapper = mapper;
            _repo = repo;
        }

        [HttpGet]
        public async Task<IActionResult> GetMessages([FromQuery] bool isTrashed = false)
        {
            var messages = await _repo.GetMessagesAsync(isTrashed);

            return Ok(messages);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetMessage(Guid id)
        {
            var message = await _repo.GetMessageAsync(id);

            if (message == null)
                return NotFound("Message not found.");

            return Ok(message);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> PostMessage(Message message)
        {
            if (string.IsNullOrEmpty(message.Name)) return BadRequest("Phone number required");

            if (string.IsNullOrEmpty(message.Phone)) return BadRequest("Phone number required");

            if (string.IsNullOrEmpty(message.Text)) return BadRequest("Message text required");

            // if(!string.IsNullOrEmpty(message.Email)){
            //     var mail = _mapper.Map<MailRequest>(message);

            //     await _mail.SendEmailAsync(mail);
            // }

            _repo.Add(message);

            if (await _repo.SaveAll())
                return CreatedAtAction(nameof(GetMessage), new { id = message.Id }, message);

            throw new Exception("Message send failed!");
        }

        [HttpPut("trash/{id}")]
        public async Task<IActionResult> RemoveMessage(Guid id)
        {
            var message = await _repo.GetMessageAsync(id);

            if (message == null) return NotFound("Message not found");

            if (message.IsTrashed) return BadRequest("Message already removed");

            message.IsTrashed = true;

            if (await _repo.SaveAll()) return NoContent();

            throw new Exception("Message remove failed!");
        }

        [HttpPut("Restore/{id}")]
        public async Task<IActionResult> RestoreMessage(Guid id)
        {
            var message = await _repo.GetMessageAsync(id);

            if (message == null) return NotFound("Message not found");

            if (!message.IsTrashed) return BadRequest("Message already storeded");

            message.IsTrashed = false;

            if (await _repo.SaveAll()) return Ok(message);

            throw new Exception("Message restore failed!");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMessage(Guid id)
        {
            var message = await _repo.GetMessageAsync(id);

            if (message == null) return NotFound("Message not found");

            if (!message.IsTrashed) return BadRequest("Move the message to the trash first");

            _repo.Delete(message);

            if (await _repo.SaveAll()) return NoContent();

            throw new Exception("Message delete failed!");
        }
    }
}