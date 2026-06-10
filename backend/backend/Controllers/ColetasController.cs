using backend.Dtos;
using backend.Models;
using backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ColetasController : ControllerBase
    {
        private readonly IColetasService _coletasService;

        public ColetasController(IColetasService coletasService)
        {
            _coletasService = coletasService;
        }

        // Listagem com filtros (status, cliente, periodo) e paginacao.
        [HttpGet]
        public async Task<ActionResult<ResultadoPaginado<Coleta>>> GetColetas([FromQuery] ColetaFiltroDto filtro)
        {
            var resultado = await _coletasService.GetColetas(filtro);
            return Ok(resultado);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Coleta>> GetColeta(int id)
        {
            var coleta = await _coletasService.GetColeta(id);

            if (coleta == null)
                return NotFound("Coleta não encontrada.");

            return Ok(coleta);
        }

        [HttpPost]
        public async Task<ActionResult> CreateColeta([FromBody] CriarColetaDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var coleta = await _coletasService.CreateColeta(dto);
            return CreatedAtAction(nameof(GetColeta), new { id = coleta.Id }, coleta);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateColeta(int id, [FromBody] AtualizarColetaDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _coletasService.UpdateColeta(id, dto);
            return NoContent();
        }

        [HttpPatch("{id}/atribuir")]
        public async Task<ActionResult> AtribuirMotoristaVeiculo(int id, [FromBody] AtribuirColetaRequest request)
        {
            await _coletasService.AtribuirMotoristaVeiculo(id, request.MotoristaId, request.VeiculoId);
            return NoContent();
        }

        [HttpPatch("{id}/iniciar")]
        public async Task<ActionResult> IniciarColeta(int id)
        {
            await _coletasService.IniciarColeta(id);
            return NoContent();
        }

        [HttpPatch("{id}/concluir")]
        public async Task<ActionResult> ConcluirColeta(int id)
        {
            await _coletasService.ConcluirColeta(id);
            return NoContent();
        }

        [HttpPatch("{id}/cancelar")]
        public async Task<ActionResult> CancelarColeta(int id)
        {
            await _coletasService.CancelarColeta(id);
            return NoContent();
        }

        // Registra uma ocorrencia. O usuario responsavel vem do token JWT.
        [HttpPost("{id}/ocorrencias")]
        public async Task<ActionResult> RegistrarOcorrencia(int id, [FromBody] CriarOcorrenciaDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var usuario = User.Identity?.Name ?? "desconhecido";
            await _coletasService.RegistrarOcorrencia(id, dto.Descricao, usuario);
            return NoContent();
        }
    }

    public class AtribuirColetaRequest
    {
        public int MotoristaId { get; set; }
        public int VeiculoId { get; set; }
    }
}
