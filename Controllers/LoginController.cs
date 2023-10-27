using EstacionamentoAPI.Data;
using EstacionamentoAPI.Extensions;
using EstacionamentoAPI.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SecureIdentity.Password;

namespace EstacionamentoAPI.Controllers
{
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly DataContext _context;
        public LoginController(DataContext context) 
        { 
            _context = context;
        }

        [HttpPost("v1/users/login")]
        public async Task<IActionResult> Post(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.GetErrors());

            var user = await _context.Users.AsNoTracking().Include(x => x.Roles).FirstOrDefaultAsync(x => x.Email == model.Email);

            if (user == null)
                return StatusCode(401, new ResultViewModel<string>("Usuário ou senha inválidos!"));

            if (!PasswordHasher.Verify(user.Password, model.Senha))
                return StatusCode(401, new ResultViewModel<string>("Usuário ou senha inválidos!"));

            try
            {
                //geração do token e return;
                return Ok(); //apenas para nao dar erro de código enquanto crio as demais funções
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<string>("00X001 - Erro interno no servidor!"));
            }
        }

        [HttpPut("v1/users/change-password")]
        public async Task<IActionResult> ChancePassword(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.GetErrors());

            var user = await _context.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Email == model.Email);

            if (user == null)
                return StatusCode(401, new ResultViewModel<string>("O usuário não foi encontrado!"));

            try
            {
                var password = PasswordHasher.Hash(model.Senha);
                user.Password = password;

                _context.Users.Update(user);
                await _context.SaveChangesAsync();

                return Ok(new ResultViewModel<dynamic>(new {user}));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<string>("00X002 - Erro interno no servidor!"));
            }
        }
    }
}
