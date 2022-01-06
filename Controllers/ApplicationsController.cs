using System;
using System.IO;
using System.Threading.Tasks;
using AutoMapper;
using iqrasys.api.Data;
using iqrasys.api.Dtos;
using iqrasys.api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace iqrasys.api.Controllers
{
    public class ApplicationsController : ApiController
    {
        private readonly IIqraRepository _repo;
        private readonly IMapper _mapper;
        public ApplicationsController(IIqraRepository repo, IMapper mapper)
        {
            _mapper = mapper;
            _repo = repo;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> PostApplication(
            [FromForm] ApplicationForCreateDto applicationForCreate)
        {
            var file = applicationForCreate.File;

            var folderName = Path.Combine("Resources", "Images");
            var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

            var fileName = Guid.NewGuid() + "-" + file.FileName;
            var fullPath = Path.Combine(pathToSave, fileName);
            var dbPath = Path.Combine(folderName, fileName);

            if (file.Length == 0) return BadRequest("File is corrupted.");

            try
            {
                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

            }
            catch (Exception ex)
            {
                throw new Exception("File post failed " + ex);
            }

            var application = new Application();
            application.FilePath = dbPath;

            _repo.Add(application);

            if (await _repo.SaveAll())
            {
                return Ok(new { file = dbPath });
            }

            throw new Exception("File post failed.");
        }

        [HttpGet]
        public async Task<IActionResult> GetApplications([FromQuery] bool isTrashed = false)
        {
            var application = await _repo.GetApplicationsAsync(isTrashed);

            return Ok(application);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetApplication(Guid id)
        {
            var application = await _repo.GetApplicationAsync(id);

            if (application == null) return NotFound("Application not found.");

            return Ok(application);
        }

        [HttpPut("trash/{id}")]
        public async Task<IActionResult> RemoveApplication(Guid id)
        {
            var application = await _repo.GetApplicationAsync(id);

            if (application == null) return NotFound("Application not found.");

            if (application.IsTrashed) return BadRequest("Application already removed.");

            application.IsTrashed = true;

            if (await _repo.SaveAll()) return NoContent();

            throw new Exception("Application remove failed!");
        }

        [HttpPut("Restore/{id}")]
        public async Task<IActionResult> RestoreApplication(Guid id)
        {
            var application = await _repo.GetApplicationAsync(id);

            if (application == null) return NotFound("Application not found");

            if (!application.IsTrashed) return BadRequest("Application already storeded");

            application.IsTrashed = false;

            if (await _repo.SaveAll()) return Ok(application);

            throw new Exception("Application restore failed!");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteApplication(Guid id)
        {
            var application = await _repo.GetApplicationAsync(id);

            if (application == null) return NotFound("Application not found");

            if (!application.IsTrashed) return BadRequest("Move the application to the trash first");

            var pathToDelete = Path.Combine(Directory.GetCurrentDirectory(), application.FilePath);

            try
            {
                if (System.IO.File.Exists(pathToDelete))
                {
                    System.IO.File.Delete(pathToDelete);
                }
            }
            catch (System.Exception)
            {
                throw new Exception("Application Delete failed!");;
            }

            _repo.Delete(application);

            if (await _repo.SaveAll()) return NoContent();

            throw new Exception("Application delete failed!");
        }
    }
}