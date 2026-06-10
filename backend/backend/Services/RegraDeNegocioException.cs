namespace backend.Services
{
    // Excecao usada para regras de negocio (ex.: status invalido).
    // O middleware de erros transforma isso em um HTTP 400 com mensagem clara.
    public class RegraDeNegocioException : Exception
    {
        public RegraDeNegocioException(string mensagem) : base(mensagem)
        {
        }
    }
}
