using AuthAPI.Data;
using AuthAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace AuthAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GraduatesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public GraduatesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Graduates
        [HttpGet]
        public async Task<IActionResult> GetAllGraduates()
        {
            var graduates = await _context.Graduates.Where(g => !g.IsDeleted).ToListAsync(); // Filter out deleted graduates
            return Ok(graduates);
        }

        // GET: api/Graduates/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetGraduate(Guid id)
        {
            var graduate = await _context.Graduates.FirstOrDefaultAsync(g => g.Id == id && !g.IsDeleted); // Fetch a single non-deleted graduate
            if (graduate == null)
                return NotFound(new { message = "Graduate not found" });

            return Ok(graduate);
        }

        // POST: api/Graduates
        [HttpPost]
        public async Task<IActionResult> AddGraduate([FromBody] Graduate graduate)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Ensure all required fields are provided
            graduate.CreatedAt = DateTime.UtcNow;
            graduate.UpdatedAt = DateTime.UtcNow;

            _context.Graduates.Add(graduate);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetGraduate), new { id = graduate.Id }, graduate); // Return the created graduate with its id
        }

        // PUT: api/Graduates/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateGraduate(Guid id, [FromBody] Graduate updatedGraduate)
        {
            var graduate = await _context.Graduates.FirstOrDefaultAsync(g => g.Id == id && !g.IsDeleted);
            if (graduate == null)
                return NotFound(new { message = "Graduate not found" });

            // Update fields
            graduate.FirstName = updatedGraduate.FirstName;
            graduate.LastName = updatedGraduate.LastName;
            graduate.Email = updatedGraduate.Email;
            graduate.Address = updatedGraduate.Address;
            graduate.PhoneNumber = updatedGraduate.PhoneNumber;
            graduate.Age = updatedGraduate.Age;
            graduate.DateOfBirth = updatedGraduate.DateOfBirth;
            graduate.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return Ok(new { message = "Graduate updated successfully", graduate });
        }

        // DELETE: api/Graduates/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGraduate(Guid id)
        {
            var graduate = await _context.Graduates.FirstOrDefaultAsync(g => g.Id == id && !g.IsDeleted);
            if (graduate == null)
                return NotFound(new { message = "Graduate not found" });

            // Perform soft delete by marking IsDeleted as true
            graduate.IsDeleted = true;
            graduate.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return Ok(new { message = "Graduate deleted successfully (soft delete)" });
        }
    }
}
