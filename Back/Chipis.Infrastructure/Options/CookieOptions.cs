namespace Chipis.Infrastructure.Options
{
    public class CookieOptions
    {
        public string RefreshTokenName { get; set; } = string.Empty;
        public int CookieLifetimeDays { get; set; }
    }
}
