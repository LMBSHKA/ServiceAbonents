using Microsoft.EntityFrameworkCore;
using ServiceAbonents.Data;
using ServiceAbonents.Debiting;
using ServiceAbonents.RabbitMq;
using Quartz;
using Microsoft.OpenApi.Models;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Autofac.Core;
using MassTransit;
using ServiceAbonents.Dtos;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddCors(options =>
        {
            options.AddDefaultPolicy(builder =>
            {
                builder.SetIsOriginAllowed(origin => true)
                       .AllowAnyMethod()
                       .AllowAnyHeader()
                       .AllowCredentials();
            });
        });

        builder.Services.AddMassTransit(x =>
        {
            x.SetKebabCaseEndpointNameFormatter();
            x.AddConsumer<RabbitMqListenerAuth>();

            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host("amqps://akmeanzg:TMOCQxQAEWZjfE0Y7wH5v0TN_XTQ9Xfv@mouse.rmq5.cloudamqp.com/akmeanzg");
                cfg.ReceiveEndpoint("queue-name", x =>
                {
                    x.ConfigureConsumer<RabbitMqListenerAuth>(context);
                    x.Bind("exchange-name");
                });
                cfg.Message<IdForCartDto> (x => x.SetEntityName("Cart"));

                cfg.ClearSerialization();
                cfg.UseRawJsonSerializer();

            });

        });

        builder.Services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "Abonents API",
                Description = "управление пользоваителями ссылка на сайт - https://serviceabonents-2.onrender.com",
            });

            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please enter JWT with Bearer into field",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "Bearer"
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type=ReferenceType.SecurityScheme,
                            Id="Bearer"
                        }
                    },
                    new string[]{}
                }
             });

            options.CustomSchemaIds(type => type.ToString());
        });

        var identityUrl = builder.Configuration.GetValue<string>("JwtPrivateKey");

        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme;

        }).AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,

                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(identityUrl)),
            };
        });

        //Connect Db
        builder.Services.AddDbContext<AppDbContext>(opt =>
        {
            opt.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
        });

        // build quartz
        builder.Services.AddQuartz(q =>
        {
            var jobKey = new JobKey("deb");
            q.AddJob<MothlyDebiting>(opts => opts.WithIdentity(jobKey));

                q.AddTrigger(opts => opts
                    .ForJob(jobKey)
                    .WithIdentity("Debiting-trigger")
                    .WithCronSchedule(CronScheduleBuilder.DailyAtHourAndMinute(00, 00)));
        });

        // Add services to the container.
        builder.Services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);

        builder.Services.AddHostedService<RabbitMqListener>();

        builder.Services.AddScoped<ISwitchTarif, SwitchTarif>();
        builder.Services.AddScoped<ISender, RabbitMqSender>();
        builder.Services.AddScoped<IDebiting, Debiting>();
        builder.Services.AddScoped<IUpdateBalance, UpdateBalance>();
        builder.Services.AddScoped<IAbonentRepo, AbonentRepo>();
        builder.Services.AddScoped<IRemainRepo, RemainRepo>();
        builder.Services.AddControllers().AddNewtonsoftJson();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(options =>
        {
            var basePath = AppContext.BaseDirectory;

            var xmlPath = Path.Combine(basePath, "ServiceAbonents.xml");
            options.IncludeXmlComments(xmlPath);
        });

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.yaml", "v1");
            });
        }

        app.UseCors();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
        
    }
}