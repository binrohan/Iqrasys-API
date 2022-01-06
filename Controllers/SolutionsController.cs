using System;
using System.Threading.Tasks;
using AutoMapper;
using iqrasys.api.Data;
using iqrasys.api.Models;
using Microsoft.AspNetCore.Mvc;

namespace iqrasys.api.Controllers
{
    public class SolutionsController : ApiController
    {
        private readonly IIqraRepository _repo;
        private readonly IMapper _mapper;
        public SolutionsController(IIqraRepository repo, IMapper mapper)
        {
            _mapper = mapper;
            _repo = repo;
        }

        [HttpGet]
        public async Task<IActionResult> GetSolutions([FromQuery] bool isTrashed = false)
        {
            var solutions = await _repo.GetSolutionsAsync(isTrashed);

            return Ok(solutions);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSolution(Guid id)
        {
            var solution = await _repo.GetSolutionAsync(id);

            if(solution == null){
                return NotFound("Solution not found");
            }

            return Ok(solution);
        }

        [HttpPost]
        public async Task<IActionResult> AddSolution(Solution solution)
        {
            if(solution.Order == null)
            {
                solution.Order = 0;
            }

            _repo.Add(solution);

            if(await _repo.SaveAll()){
                return CreatedAtAction(nameof(GetSolution), new {id = solution.Id}, solution);
            }

            throw new Exception("Add solution failed.");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> RemoveSolution(Guid id)
        {
            var solution = await _repo.GetSolutionAsync(id);

            if(solution == null){
                return NotFound("Solution not found.");
            }

            if(solution.IsTrashed){
                return BadRequest("Solution already deleted!");
            }

            solution.IsTrashed = true;

            if(await _repo.SaveAll()){
                return NoContent();
            }  

            throw new Exception("Remove solution failed.");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSolution(Guid id)
        {
            var solution = await _repo.GetSolutionAsync(id);

            if(solution == null){
                return NotFound("Solution not found.");
            }

            if(!solution.IsTrashed){
                return BadRequest("Solution can't be deleted!");
            }

            _repo.Delete(solution);

            if(await _repo.SaveAll()){
                return NoContent();
            }

            throw new Exception("Delete solution failed.");
        }
    }
}