using MagellanTest.Repositories;
using MagellanTest.Services;
using Npgsql;

var builder = WebApplication.CreateBuilder(args);

//Get Database Connection String
var connectionString = builder.Configuration.GetConnectionString("PartPostgreSQLConnection");

// Add services to the container.

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IItemService, ItemService>();
builder.Services.AddScoped<ItemRepository>();

// Configure the PostgreSQL connection as a singleton
builder.Services.AddSingleton<NpgsqlConnection>(_ =>
{
    var connection = new NpgsqlConnection(connectionString);
    connection.Open();
    return connection;
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
