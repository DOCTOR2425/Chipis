using Chipis.Application.Abstractions;

namespace Chipis.Infrastructure
{
    public class HashProvider : IHashProvider
    {
        public string Generate(string text)
        {
            return BCrypt.Net.BCrypt.EnhancedHashPassword(text);
        }

        public bool Verify(string text, string textHash)
        {
            return BCrypt.Net.BCrypt.EnhancedVerify(text, textHash);
        }
    }
}
