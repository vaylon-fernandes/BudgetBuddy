using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;
using webapi.Data;
using webapi.Helpers;
//using webapi.Services;
using WebApi.Helpers;


var builder = WebApplication.CreateBuilder(args);



// Add services to the container.
builder.Services.AddCors();
builder.Services.AddControllers();
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());

    options.JsonSerializerOptions.DefaultIgnoreCondition =
        JsonIgnoreCondition.WhenWritingNull;
});
// configure strongly typed settings object
builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));

// configure DI for application services
//builder.Services.AddScoped<IUserService, UserService>();

string conString = @"server=localhost;port=3306;user=root; password=root123;database=testDb2";
builder.Services.AddDbContext<ApiDbContext>(options => options.UseMySQL(conString));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => c.SwaggerDoc("v1", new OpenApiInfo { Title = "JWTAuthentication", Version = "v1" }));

var app = builder.Build();

app.UseAuthentication();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();

    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "JWTAuthentication v1"));
}
else
{
    app.UseHsts();
}



// global cors policy
app.UseCors(x => x
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());

// custom jwt auth middleware
app.UseMiddleware<JwtMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
