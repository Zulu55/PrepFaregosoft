using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PrepFaregosoft.Api.Data;
using PrepFaregosoft.Api.Data.Entities;
using PrepFaregosoft.Api.Models;
using System.Linq;
using System.Threading.Tasks;

namespace PrepFaregosoft.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly DataContext _context;

        public UsersController(DataContext context)
        {
            _context = context;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, User user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<User>> PostUser(RegisterUserModel model)
        {
            User currentUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == model.Email);
            if (currentUser != null)
            {
                return BadRequest("Email ya resgistrado por otro usuario.");
            }

            _context.Users.Add(new()
            {
                Email = model.Email,
                FirstName = model.FirstName,
                IsActive = true,
                IsBlock = false,
                LastName = model.LastName,
                Password = model.Password
            });
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPost, Route("Login")]
        public async Task<ActionResult<User>> Login(LoginRequestModel model)
        {
            User user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == model.Email &&
                                          u.Password == model.Password);
            if (user == null)
            {
                return BadRequest("Email o contraseña incorrectos.");
            }

            if (!user.IsActive)
            {
                return BadRequest("Usuario no activo en el sistema.");
            }

            if (user.IsBlock)
            {
                return BadRequest("Usuario bloqueado.");
            }

            return Ok(user);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            User user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            user.IsActive = false;
            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
