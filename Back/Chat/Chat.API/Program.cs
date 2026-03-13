using Chipis.Application.Services;
using Chipis.Core.Abstractions;
using Chipis.DataAccess;
using Chipis.DataAccess.Repositories;
using Microsoft.AspNetCore.Authentication.Negotiate;

namespace Chipis.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            builder.Services.AddControllers();
            builder.Services.AddOpenApi();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<ChipisDbContext>();

            builder.Services.AddScoped<IUsersService, UsersService>();
            builder.Services.AddScoped<IUsersRepository, UsersRepository>();
            //builder.Services.AddScoped<ICha, UsersRepository>();

            builder.Services.AddAuthentication(NegotiateDefaults.AuthenticationScheme)
                .AddNegotiate();

            builder.Services.AddAuthorization(options =>
            {
                options.FallbackPolicy = options.DefaultPolicy;
            });

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();

                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
