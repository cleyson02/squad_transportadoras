using backend.Models;
using backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ColetasController : ControllerBase
    {
        private readonly IColetasService _coletasService;

        public ColetasController(IColetasService coletasService)
        {
            _coletasService = coletasService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Coleta>>> GetColetas()
        {
            var coletas = await _coletasService.GetColetas();
            return Ok(coletas);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Coleta>> GetColeta(int id)
        {
            var coleta = await _coletasService.GetColeta(id);

            if (coleta == null)
                return NotFound("Coleta não encontrada.");

            return Ok(coleta);
        }

        [HttpGet("remetente/{remetente}")]
        public async Task<ActionResult<IEnumerable<Coleta>>> GetColetasByRemetente(string remetente)
        {
            var coletas = await _coletasService.GetColetasByRemetente(remetente);
            return Ok(coletas);
        }

        [HttpPost]
        public async Task<ActionResult> CreateColeta([FromBody] Coleta coleta)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _coletasService.CreateColeta(coleta);

            return CreatedAtAction(nameof(GetColeta), new { id = coleta.Id }, coleta);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateColeta(int id, [FromBody] Coleta coleta)
        {
            if (id != coleta.Id)
                return BadRequest("O ID da rota deve ser igual ao ID da coleta.");

            try
            {
                await _coletasService.UpdateColeta(coleta);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPatch("{id}/cancelar")]
        public async Task<ActionResult> CancelarColeta(int id)
        {
            try
            {
                await _coletasService.CancelarColeta(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPatch("{id}/atribuir")]
        public async Task<ActionResult> AtribuirMotoristaVeiculo(int id, [FromBody] AtribuirColetaRequest request)
        {
            try
            {
                await _coletasService.AtribuirMotoristaVeiculo(id, request.MotoristaId, request.VeiculoId);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPatch("{id}/concluir")]
        public async Task<ActionResult> ConcluirColeta(int id)
        {
            try
            {
                await _coletasService.ConcluirColeta(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }

    public class AtribuirColetaRequest
    {
        public int MotoristaId { get; set; }
        public int VeiculoId { get; set; }
    }
}