using Server.Repository;
using Server.Repository.Interface;

var builder = WebApplication.CreateBuilder(args);

string accountDbConnectionString = builder.Configuration.GetConnectionString("AccountDb");

builder.Services.AddScoped<IAuthRepository>(sp => new AuthRepository(accountDbConnectionString));

builder.Services.AddControllers();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();