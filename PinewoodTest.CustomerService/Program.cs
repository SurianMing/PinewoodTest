using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using PinewoodTest.CustomerService.BusinessLogic;
using PinewoodTest.CustomerService.Data;
using PinewoodTest.CustomerService.Data.Context;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<CustomerContext>(options =>
    options.UseSqlite(new SqliteConnectionStringBuilder
    {
        DataSource = "data.db",
        Mode = SqliteOpenMode.ReadWriteCreate
    }.ConnectionString)
);

builder.Services.AddScoped<ICustomerLogic, CustomerLogic>();
builder.Services.AddScoped<CustomerRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using (var serviceScope = app.Services.GetService<IServiceScopeFactory>()!.CreateScope())
{
    var context = serviceScope.ServiceProvider.GetRequiredService<CustomerContext>();

    context.Database.Migrate();
}

app.MapControllers();

// app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherForecast", () =>
{
    var forecast =  Enumerable.Range(1, 5).Select(index =>
        new
        {
            When = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Where = Random.Shared.Next(-20, 55),
            Which = summaries[Random.Shared.Next(summaries.Length)]
        })
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast")
.WithOpenApi();

app.Run();