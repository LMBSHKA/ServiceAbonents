using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore;
using Microsoft.EntityFrameworkCore;
using ServiceAbonents.Data;
using ServiceAbonents.Debiting;
using ServiceAbonents.RabbitMq;
using Microsoft.Extensions.Hosting;
using Quartz;
using Autofac.Core;
using Microsoft.Extensions.Configuration;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        var identityUrl = builder.Configuration.GetValue<string>("IdentityUrl");
        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme;

        }).AddJwtBearer(options =>
        {
            options.Authority = identityUrl;
            options.RequireHttpsMetadata = false;
            options.Audience = "abonent";
        });

        //Connect Db
        builder.Services.AddDbContext<AppDbContext>(opt =>
        {
            opt.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
        });

        //DB cleaner
        //var cleaner = new DatabaseCleaner(builder.Configuration.GetConnectionString("DefaultConnection"));
        //cleaner.ClearDatabase();

        // build quartz
        builder.Services.AddQuartz(q =>
        {
            var jobKey = new JobKey("deb");
            q.AddJob<MothlyDebiting>(opts => opts.WithIdentity(jobKey));

                q.AddTrigger(opts => opts
                    .ForJob(jobKey)
                    .WithIdentity("Debiting-trigger")
                    .WithCronSchedule(CronScheduleBuilder.DailyAtHourAndMinute(18, 30)));
        });

        // Add services to the container.
        builder.Services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);
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


//public class DatabaseCleaner
//{
//    private readonly string _connectionString;

//    public DatabaseCleaner(string connectionString)
//    {
//        _connectionString = connectionString;
//    }
//    public void ClearDatabase()
//    {
//        var serviceProvider = new ServiceCollection()
//           .AddDbContext<AppDbContext>(options => options.UseNpgsql(_connectionString))
//           .BuildServiceProvider();

//        using var context = serviceProvider.GetRequiredService<AppDbContext>();

//        // Удаляем существующую базу данных (если есть)
//        context.Database.EnsureDeleted();
//    }
//}