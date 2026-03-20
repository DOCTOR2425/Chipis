namespace Chipis.Application.Abstractions
{
    public interface IHashProvider
    {
        string Generate(string text);
        bool Verify(string text, string textHash);
    }
}