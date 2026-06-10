using backend.Context;
using backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Controllers
{
    // Endpoints de apoio usados pelo frontend para preencher selects
    // (clientes, motoristas e veiculos). Apenas leitura.
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CadastrosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CadastrosController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("clientes")]
        public async Task<ActionResult<IEnumerable<Cliente>>> GetClientes()
        {
            var clientes = await _context.Clientes
                .Where(c => c.Ativo)
                .OrderBy(c => c.Nome)
                .ToListAsync();
            return Ok(clientes);
        }

        [HttpGet("motoristas")]
        public async Task<ActionResult<IEnumerable<Motorista>>> GetMotoristas()
        {
            var motoristas = await _context.Motoristas
                .Where(m => m.Ativo)
                .OrderBy(m => m.Nome)
                .ToListAsync();
            return Ok(motoristas);
        }

        [HttpGet("veiculos")]
        public async Task<ActionResult<IEnumerable<Veiculo>>> GetVeiculos()
        {
            var veiculos = await _context.Veiculos
                .Where(v => v.Ativo)
                .OrderBy(v => v.Placa)
                .ToListAsync();
            return Ok(veiculos);
        }
    }
}
