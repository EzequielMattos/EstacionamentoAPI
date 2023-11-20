using EstacionamentoAPI.Data;
using EstacionamentoAPI.Extensions;
using EstacionamentoAPI.Models;
using EstacionamentoAPI.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SecureIdentity.Password;

namespace EstacionamentoAPI.Controllers
{
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly UserManager<IdentityUser<int>> _userManager;
        public UserController(DataContext context, UserManager<IdentityUser<int>> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet("v1/users")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllAsync([FromQuery] int page = 1, [FromQuery] int pageSize = 25) 
        {
            try
            {
                if (page <= 0 || pageSize <= 0)
                    return BadRequest(new ResultViewModel<List<User>>("00X003 - Paginação inválida!"));

                var skip = (page - 1) * pageSize;
                var take = pageSize;

                var users = await _context.Users.AsNoTracking().Skip(skip).Take(take).ToListAsync();

                return Ok(new ResultViewModel<List<User>>(users));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<List<User>>("00X004 - Erro interno no servidor!"));
            }
        }

        [HttpGet("v1/users/{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAsync([FromRoute] int id)
        {
            try
            {
                var user = await _context.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

                if (user == null)
                    return NotFound(new ResultViewModel<User>("Nenhum usuário foi encontrado!"));

                return Ok(new ResultViewModel<User>(user));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<List<User>>("00X005 - Erro interno no servidor!"));
            }
        }

        [HttpPost("v1/users")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> PostAsync([FromBody] EditorUserViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new ResultViewModel<User>(ModelState.GetErrors()));

                var userEmail = _context.Users.AsNoTracking().FirstOrDefault(x => x.Email == model.Email);

                if (userEmail != null)
                    return StatusCode(400, new ResultViewModel<User>($"Esse e-mail já está cadastrado no sistema!"));

                var password = PasswordHasher.Hash(model.Password);

                var user = new User
                {
                    Name = model.Name,
                    Email = model.Email,
                    Password = password
                };

                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();

                //Adiciona o role ao usuario
                //TO DO

                return Created($"v1/user/{user.Id}",new ResultViewModel<dynamic>(new
                {
                    user.Name,
                    user.Email,
                }));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<List<User>>("00X006 - Erro interno no servidor!"));
            }
        }

        [HttpPut("v1/users/{id:int}")]
        [Authorize(Roles = "Customer, Admin")]
        public async Task<IActionResult> PutAsync([FromBody] EditorUserViewModel model, [FromRoute] int id)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new ResultViewModel<User>(ModelState.GetErrors()));

                var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);

                if (user == null)
                    return NotFound(new ResultViewModel<User>("Nenhum usuário foi encontrado!"));               

                var password = PasswordHasher.Hash(model.Password);

                user.Name = model.Name;
                user.Email = model.Email;
                user.Password = password;

                _context.Users.Update(user);
                await _context.SaveChangesAsync();

                return Ok(new ResultViewModel<dynamic>(new 
                {
                    user.Name,
                    user.Email,
                }));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<List<User>>("00X007 - Erro interno no servidor!"));
            }
        }

        [HttpDelete("v1/users/{id:int}")]
        [Authorize(Roles = "Customer, Admin")]
        public async Task<IActionResult> DeleteAsync([FromRoute] int id)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);

                if (user == null)
                    return NotFound(new ResultViewModel<User>("Nenhum usuário foi encontrado!"));

                _context.Users.Remove(user);
                await _context.SaveChangesAsync();

                return Ok(new ResultViewModel<User>(user));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResultViewModel<User>("00X008 - Falha interna no servidor."));
            }
        }
    }
}
