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

app.MapGet("/heartbeat", () =>
{
    return "Running....";
})
.WithName("GetHeartbeat")
.WithOpenApi();

app.Run();