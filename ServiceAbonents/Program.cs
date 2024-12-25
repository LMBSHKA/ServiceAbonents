using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using ServiceAbonents.Data;
using ServiceAbonents.Debiting;
using ServiceAbonents.Models;
using ServiceAbonents.RabbitMq;
using System.IdentityModel.Tokens.Jwt;
using Volo.Abp.Data;


internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        //Connect Db
        builder.Services.AddDbContext<AppDbContext>(opt =>
        {
            opt.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
        });

        //очистка БД
        var cleaner = new DatabaseCleaner(builder.Configuration.GetConnectionString("DefaultConnection"));
        cleaner.ClearDatabase();

        // Add services to the container.
        builder.Services.AddHostedService<RabbitMqListener>();

        builder.Services.AddScoped<ISender, RabbitMqSender>();
        builder.Services.AddScoped<IDebiting, Debiting>();
        builder.Services.AddScoped<IUpdateBalance, UpdateBalance>();
        builder.Services.AddScoped<IAbonentRepo, AbonentRepo>();
        builder.Services.AddScoped<IRemainRepo, RemainRepo>();
        builder.Services.AddControllers().AddNewtonsoftJson();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        
        //PrepDb.PrepPopulation(app);
        
        //app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
        
    }
}

public class DatabaseCleaner
{
    private readonly string _connectionString;

    public DatabaseCleaner(string connectionString)
    {
        _connectionString = connectionString;
    }
    public void ClearDatabase()
    {
        var serviceProvider = new ServiceCollection()
           .AddDbContext<AppDbContext>(options => options.UseNpgsql(_connectionString))
           .BuildServiceProvider();

        using var context = serviceProvider.GetRequiredService<AppDbContext>();

        // Удаляем существующую базу данных (если есть)
        context.Database.EnsureDeleted();
    }
}