using ID3.Finance.Binance.Extensions;
using ID3.Finance.Binance.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.ML;
using Quartz;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.



builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddInfrastructure();


builder.Services.AddDbContext<BinanceContext>(opt =>
{
    opt.UseNpgsql("Host=localhost;Port=5432;Database=BinanceDatabase3;Username=admin;Password=shaze");
});



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseAuthorization();



app.MapControllers();

app.Run();
