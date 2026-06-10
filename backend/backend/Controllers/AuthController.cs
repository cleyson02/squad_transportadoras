using backend.Dtos;
using backend.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly TokenService _tokenService;

        public AuthController(UserManager<IdentityUser> userManager, TokenService tokenService)
        {
            _userManager = userManager;
            _tokenService = tokenService;
        }

        // Login: valida usuario/senha e devolve um token JWT.
        [HttpPost("login")]
        public async Task<ActionResult<LoginRespostaDto>> Login([FromBody] LoginDto dto)
        {
            var usuario = await _userManager.FindByNameAsync(dto.Usuario);

            if (usuario == null || !await _userManager.CheckPasswordAsync(usuario, dto.Senha))
                return Unauthorized("Usuário ou senha inválidos.");

            var (token, expiraEm) = _tokenService.GerarToken(usuario);

            return Ok(new LoginRespostaDto
            {
                Token = token,
                Usuario = usuario.UserName ?? string.Empty,
                ExpiraEm = expiraEm
            });
        }
    }
}
