using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ServiceAbonents.Data;
using ServiceAbonents.RabbitMq;
using System.IdentityModel.Tokens.Jwt;


internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        //настройки для токена
        //builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        //    .AddJwtBearer(options =>
        //    {
        //        options.TokenValidationParameters = new TokenValidationParameters
        //        {
        //            // указывает, будет ли валидироваться издатель при валидации токена
        //            ValidateIssuer = true,
        //            // строка, представляющая издателя
        //            ValidIssuer = AuthOptions.ISSUER,
        //            // будет ли валидироваться потребитель токена
        //            ValidateAudience = true,
        //            // установка потребителя токена
        //            ValidAudience = AuthOptions.AUDIENCE,
        //            // будет ли валидироваться время существования
        //            ValidateLifetime = true,
        //            // установка ключа безопасности
        //            IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
        //            // валидация ключа безопасности
        //            ValidateIssuerSigningKey = true,
        //        };
        //    });

        //Connect Db
        builder.Services.AddDbContext<AppDbContext>(opt =>
        {
            opt.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
        });

        
        // Add services to the container.
        builder.Services.AddHostedService<RabbitMqListener>();

        //builder.Services.AddScoped<IUpdateBalance, UpdateBalance>();
        builder.Services.AddScoped<IAbonentRepo, AbonentRepo>();
        builder.Services.AddScoped<IRemainRepo, RemainRepo>();
        builder.Services.AddControllers().AddNewtonsoftJson();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();
        
        //Генерация токена при авторизации пользоваетеля в этом примере генерица токен по имени пользователя
        //токен должен генерится в сервисе авторизации я его должен принимать через запрос от клиента
        //app.Map("/login/{username}", (string username) =>
        //{
        //    var claims = new List<Claim> { new Claim(ClaimTypes.Name, username) };
        //    var jwt = new JwtSecurityToken(
        //            issuer: AuthOptions.ISSUER,
        //            audience: AuthOptions.AUDIENCE,
        //            claims: claims,
        //            expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(2)), // время действия 2 минуты
        //            signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));

        //    return new JwtSecurityTokenHandler().WriteToken(jwt);
        //});

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

//ключ для шифровки токена и дешифровки
//public class AuthOptions
//{
//    public const string ISSUER = "MyAuthServer";
//    public const string AUDIENCE = "MyAuthClient";
//    const string KEY = "mysupersecret_secretsecretsecretkey!123";
//    public static SymmetricSecurityKey GetSymmetricSecurityKey() =>
//        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(KEY));
//}