namespace EventPlus.WebAPI.Utils
{
    public static class CriptografarUsuario
    {
        private const int WorkFactor = 12;

        public static string CriptografarSenha(string senhaPura)
        {
            if (string.IsNullOrWhiteSpace(senhaPura))
                throw new ArgumentException("Senha não pode ser vazia.", nameof(senhaPura));

            return BCrypt.Net.BCrypt.HashPassword(senhaPura, workFactor: WorkFactor);
        }

        public static bool VerificarSenha(string senhaPura, string senhaHash)
        {
            if (string.IsNullOrWhiteSpace(senhaPura) || string.IsNullOrWhiteSpace(senhaHash))
                return false;

            try
            {
                return BCrypt.Net.BCrypt.Verify(senhaPura, senhaHash);
            }
            catch (BCrypt.Net.SaltParseException)
            {
                return false;
            }
        }
    }
}