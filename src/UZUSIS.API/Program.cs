using System.Reflection;
using UZUSIS.Application.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddUserSecrets(Assembly.GetExecutingAssembly(), true,true);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder
    .Services
    .ConfigureApplication(builder.Configuration);

builder
    .Services
    .ConfigurarDependencias();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();
app.UseHttpsRedirection();

app.Run();

