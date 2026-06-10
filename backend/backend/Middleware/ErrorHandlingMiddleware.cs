using System.Net;
using System.Text.Json;
using backend.Services;

namespace backend.Middleware
{
    // Tratamento de erros centralizado.
    // Regras de negocio viram HTTP 400; erros inesperados viram HTTP 500.
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;

        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (RegraDeNegocioException ex)
            {
                _logger.LogWarning(ex, "Regra de negócio violada: {Mensagem}", ex.Message);
                await EscreverResposta(context, HttpStatusCode.BadRequest, ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado ao processar a requisição.");
                await EscreverResposta(context, HttpStatusCode.InternalServerError,
                    "Ocorreu um erro inesperado. Tente novamente.");
            }
        }

        private static async Task EscreverResposta(HttpContext context, HttpStatusCode status, string mensagem)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)status;
            var corpo = JsonSerializer.Serialize(new { mensagem });
            await context.Response.WriteAsync(corpo);
        }
    }
}
