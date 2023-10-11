using Maui.ServerAPI.Data;
using Maui.Shared.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Maui.ServerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly AppDbContext _db;

        public StudentsController(AppDbContext db)
        {
            _db = db;
        }

        [HttpGet]

        public async Task<ActionResult<List<Student>>> Get()
        {

            return await _db.Students.Include(s => s.Addresses).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Student>> GetById(int id)
        {
            var result = await _db.Students.Include(x => x.Addresses).FirstOrDefaultAsync(x => x.Id == id);
            if (result == null)
            {
                return NotFound();
            }

            return result;
        }

        [HttpPost]

        public async Task<ActionResult<int>> Post(Student student)
        {
            _db.Students.Add(student);
            await _db.SaveChangesAsync();
            return student.Id;
        }

        [HttpPut]
        public async Task<ActionResult> Put(Student student)
        {
            _db.Entry(student).State = EntityState.Modified;
            foreach (var address in student.Addresses)
            {
                if (address.Id != 0)
                {
                    _db.Entry(address).State = EntityState.Modified;
                }
                else
                {
                    _db.Entry(address).State = EntityState.Added;
                }
            }
            var idsOfadd = student.Addresses.Select(x => x.Id).ToList();
            var addToDelete = await _db
                .Addresses
                .Where(x => !idsOfadd.Contains(x.Id) && x.StudentId == student.Id).ToListAsync();
            _db.RemoveRange(addToDelete);
            await _db.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<bool> Delete(int id)
        {
            var delStudent = await _db.Students.FindAsync(id);
            if (delStudent == null)
            {
                return false;
            }
            _db.Remove(delStudent);
            await _db.SaveChangesAsync();
            return true;
        }

    }
}
