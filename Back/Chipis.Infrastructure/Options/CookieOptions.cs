namespace Chipis.Infrastructure.Options
{
    public class CookieOptions
    {
        public string RefreshToken { get; set; } = string.Empty;
        public int CookieLifetimeDays { get; set; }
    }
}
