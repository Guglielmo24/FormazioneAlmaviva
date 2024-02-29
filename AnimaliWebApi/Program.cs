using Microsoft.EntityFrameworkCore;
using AnimaliWebApi.Models.DB;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<FormazioneDBContext>(options => options.UseSqlServer("Data Source=AC-GLANGELLA2\\SQLEXPRESS;Initial Catalog=FormazioneDB;Integrated Security=True;TrustServerCertificate=true;"));
builder.Services.AddHttpClient().AddEndpointsApiExplorer();
builder.Services.AddMediatR(configuration => configuration.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
