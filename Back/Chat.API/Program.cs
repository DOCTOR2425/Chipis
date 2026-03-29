using Chipis.API.Filters;
using Chipis.Application.Abstractions;
using Chipis.Application.Services;
using Chipis.DataAccess;
using Chipis.DataAccess.Repositories;
using Chipis.Infrastructure;
using Chipis.Infrastructure.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Serilog.Events;
using System.Text;

namespace Chipis.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
                .WriteTo.Console(
                    restrictedToMinimumLevel: LogEventLevel.Information
                )
                .WriteTo.File(
                    path: "Logs/log-.txt",
                    rollingInterval: RollingInterval.Day,
                    retainedFileCountLimit: 7,
                    restrictedToMinimumLevel: LogEventLevel.Information,
                    shared: true,
                    encoding: System.Text.Encoding.UTF8
                )
                .CreateLogger();

            builder.Host.UseSerilog();

            builder.Services.Configure<JwtOptions>(
                builder.Configuration.GetSection("Jwt"));
            builder.Services.Configure<Infrastructure.Options.CookieOptions>(
                builder.Configuration.GetSection("Cookie"));

            builder.Services.AddControllers(options =>
            {
                options.Filters.Add<ApiExceptionFilter>();
            });
            builder.Services.AddOpenApi();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<ChipisDbContext>();

            builder.Services.AddSignalR();

            RegisterServices(builder);

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(opt =>
                {
                    opt.TokenValidationParameters = new()
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
                    };
                });

            builder.Services.AddAuthorization();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseCors(x =>
            {
                x.WithOrigins("http://localhost:3000")
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();

                x.WithOrigins("https://localhost:7078")
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
            });

            app.UseSerilogRequestLogging();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.MapHub<ChatHub>("/api/chat");

            app.Run();
        }

        private static void RegisterServices(WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IChatsService, ChatsService>();
            builder.Services.AddScoped<IMessagesService, MessagesService>();
            builder.Services.AddScoped<IUsersService, UsersService>();

            builder.Services.AddScoped<IChatMembersRepository, ChatMembersRepository>();
            builder.Services.AddScoped<IChatsRepository, ChatsRepository>();
            builder.Services.AddScoped<IMessagesRepository, MessagesRepository>();
            builder.Services.AddScoped<IRefreshTokensRepository, RefreshTokensRepository>();
            builder.Services.AddScoped<IUsersRepository, UsersRepository>();

            builder.Services.AddScoped<IJwtProvider, JwtProvider>();
            builder.Services.AddScoped<IHashProvider, HashProvider>();
        }
    }
}
