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
    public class CarController : ControllerBase
    {
        private readonly DataContext _context;
        public CarController(DataContext context)
        {
            _context = context;
        }

        [HttpGet("v1/cars")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllAsync([FromQuery] int page = 1, [FromQuery] int pageSize = 25)
        {
            try
            {
                if (page <= 0 || pageSize <= 0)
                    return BadRequest(new ResultViewModel<List<Car>>("00X009 - Paginação inválida!"));

                var skip = (page - 1) * pageSize;
                var take = pageSize;

                var cars = await _context.Cars.AsNoTracking().Skip(skip).Take(take).ToListAsync();

                return Ok(new ResultViewModel<List<Car>>(cars));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<List<Car>>("00X010 - Erro interno no servidor!"));
            }
        }

        [HttpGet("v1/cars/{id:int}")]
        [Authorize(Roles = "Admin, Customer")]
        public async Task<IActionResult> GetAsync([FromRoute] int id)
        {
            try
            {
                var car = await _context.Cars.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

                if (car == null)
                    return NotFound(new ResultViewModel<Car>("Nenhum carro foi encontrado!"));

                return Ok(new ResultViewModel<Car>(car));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<List<Car>>("00X011 - Erro interno no servidor!"));
            }
        }

        [HttpPost("v1/cars")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> PostAsync([FromBody] EditorCarViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new ResultViewModel<Car>(ModelState.GetErrors()));

                var plateCarro = _context.Cars.AsNoTracking().FirstOrDefault(x => x.LicensePlate == model.LicensePlate);

                if (plateCarro != null)
                    return StatusCode(400, new ResultViewModel<Car>($"Já eiste um carro cadastrado com essa placa em nosso sistema!"));

                var car = new Car
                {
                    Model = model.Model,
                    Brand = model.Brand,
                    LicensePlate = model.LicensePlate,
                };

                await _context.Cars.AddAsync(car);
                await _context.SaveChangesAsync();

                //vincular carro a um customer
                //to do

                return Created($"v1/cars/{car.Id}", new ResultViewModel<dynamic>(new
                {
                    car.Model,
                    car.Brand,
                    car.LicensePlate
                }));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<List<Car>>("00X012 - Erro interno no servidor!"));
            }
        }

        [HttpPut("v1/cars/{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> PutAsync([FromBody] EditorCarViewModel model, [FromRoute] int id)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new ResultViewModel<Car>(ModelState.GetErrors()));

                var car = await _context.Cars.FirstOrDefaultAsync(x => x.Id == id);

                if (car == null)
                    return NotFound(new ResultViewModel<User>("Nenhum carro foi encontrado!"));

                car.Model = model.Model;
                car.Brand = model.Brand;
                car.LicensePlate = model.LicensePlate;

                _context.Cars.Update(car);
                await _context.SaveChangesAsync();

                return Ok(new ResultViewModel<dynamic>(new
                {
                    car.Model,
                    car.Brand,
                    car.LicensePlate
                }));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<List<User>>("00X013 - Erro interno no servidor!"));
            }
        }

        [HttpDelete("v1/cars/{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteAsync([FromRoute] int id)
        {
            try
            {
                var car = await _context.Cars.FirstOrDefaultAsync(x => x.Id == id);

                if (car == null)
                    return NotFound(new ResultViewModel<User>("Nenhum carro foi encontrado!"));

                _context.Cars.Remove(car);
                await _context.SaveChangesAsync();

                return Ok(new ResultViewModel<Car>(car));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResultViewModel<User>("00X014 - Falha interna no servidor."));
            }
        }
    }
}
