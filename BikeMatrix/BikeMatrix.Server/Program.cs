using BikeData;
using FluentValidation;
using UnitsOfWork;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddValidatorsFromAssemblyContaining<CreateBikeValidator>();
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssemblyContaining<GetBikesHandler>();
});
builder.Services.AddDbContextFactory<BikesContext>();

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

// Configure these two as desired
var useSwagger = true || app.Environment.IsDevelopment();
var useExceptionHandler = true || !app.Environment.IsDevelopment();

if (useSwagger)
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("/index.html");

app.Run();
