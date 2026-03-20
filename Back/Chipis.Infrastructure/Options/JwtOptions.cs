namespace Chipis.Infrastructure.Options
{
    public class JwtOptions
    {
        public string Key { get; set; } = string.Empty;
        public int AccessTokenLifetimeMinutes { get; set; }
        public int RefreshTokenLifetimeDays { get; set; }
    }
}
