using EstacionamentoAPI.Data;
using EstacionamentoAPI.Extensions;
using EstacionamentoAPI.Models;
using EstacionamentoAPI.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SecureIdentity.Password;

namespace EstacionamentoAPI.Controllers
{
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly DataContext _context;
        public CustomerController(DataContext context)
        {
            _context = context;
        }

        [HttpGet("v1/customers")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllAsync([FromQuery] int page = 1, [FromQuery] int pageSize = 25)
        {
            try
            {
                if (page <= 0 || pageSize <= 0)
                    return BadRequest(new ResultViewModel<List<Customer>>("00X015 - Paginação inválida!"));

                var skip = (page - 1) * pageSize;
                var take = pageSize;

                var customers = await _context.Customers.AsNoTracking().Skip(skip).Take(take).ToListAsync();

                return Ok(new ResultViewModel<List<Customer>>(customers));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<List<Customer>>("00X016 - Erro interno no servidor!"));
            }
        }

        [HttpGet("v1/customers/{id:int}")]
        [Authorize(Roles = "Admin, Customer")]
        public async Task<IActionResult> GetAsync([FromRoute] int id)
        {
            try
            {
                var customer = await _context.Customers.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

                if (customer == null)
                    return NotFound(new ResultViewModel<Customer>("Nenhum cliente foi encontrado!"));

                return Ok(new ResultViewModel<Customer>(customer));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<List<Customer>>("00X017 - Erro interno no servidor!"));
            }
        }

        [HttpPost("v1/customers")]
        public async Task<IActionResult> PostAsync([FromBody] EditorCustomerViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new ResultViewModel<Customer>(ModelState.GetErrors()));

                var customerCPF = _context.Customers.AsNoTracking().FirstOrDefault(x => x.CPF.Replace(".", "-") == model.CPF.Replace(".", "-"));

                if (customerCPF != null)
                    return StatusCode(400, new ResultViewModel<Customer>($"Esse CPF já está cadastrado no sistema!"));

                var customer = new Customer
                {
                    Name = model.Name,
                    Email = model.Email,
                    CNH = model.CNH,
                    CPF = model.CPF,
                    Phone = model.Phone,
                    CreateDate = DateTime.Now,
                };

                await _context.Customers.AddAsync(customer);
                await _context.SaveChangesAsync();

                var password = PasswordHasher.Hash(Guid.NewGuid().ToString());

                var user = new User
                {
                    Name = model.Name,
                    Email = model.Email,
                    Password = password
                };

                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();

                var userRole = new UserRole
                {
                    IdUser = user.Id,
                    IdRole = 2
                };

                await _context.UsersRoles.AddAsync(userRole);
                await _context.SaveChangesAsync();

                return Created($"v1/customers/{customer.Id}", new ResultViewModel<dynamic>(new
                {
                    customer.Name,
                    customer.Email,
                    customer.CNH,
                    customer.CPF,
                    customer.Phone,
                }));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<List<Customer>>("00X018 - Erro interno no servidor!"));
            }
        }

        [HttpPut("v1/customers/{id:int}")]
        [Authorize(Roles = "Customer, Admin")]
        public async Task<IActionResult> PutAsync([FromBody] EditorCustomerViewModel model, [FromRoute] int id)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new ResultViewModel<Customer>(ModelState.GetErrors()));

                var customer = await _context.Customers.FirstOrDefaultAsync(x => x.Id == id);

                if (customer == null)
                    return NotFound(new ResultViewModel<User>("Nenhum usuário foi encontrado!"));

                var user = await _context.Users.FirstOrDefaultAsync(x => x.Email.ToLower() == customer.Email.ToLower());

                customer.Name = model.Name;
                customer.Email = model.Email;
                customer.CNH = model.CNH;
                customer.CPF = model.CPF;
                customer.Phone = model.Phone;

                _context.Customers.Update(customer);
                await _context.SaveChangesAsync();

                user.Name = model.Name;
                user.Email = model.Email;

                _context.Users.Update(user);
                await _context.SaveChangesAsync();

                return Ok(new ResultViewModel<dynamic>(new
                {
                    customer.Name,
                    customer.Email,
                    customer.CNH,
                    customer.CPF,
                    customer.Phone,
                }));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<List<User>>("00X019 - Erro interno no servidor!"));
            }
        }

        [HttpDelete("v1/customers/{id:int}")]
        [Authorize(Roles = "Customer, Admin")]
        public async Task<IActionResult> DeleteAsync([FromRoute] int id)
        {
            try
            {
                var customer = await _context.Customers.FirstOrDefaultAsync(x => x.Id == id);

                if (customer == null)
                    return NotFound(new ResultViewModel<Customer>("Nenhum cliente foi encontrado!"));

                _context.Customers.Remove(customer);
                await _context.SaveChangesAsync();

                return Ok(new ResultViewModel<Customer>(customer));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResultViewModel<Customer>("00X020 - Falha interna no servidor."));
            }
        }
    }
}
