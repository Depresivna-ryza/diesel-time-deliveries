using DieselTimeDeliveries;
using Warehouse;
using Wolverine;
using Wolverine.Http;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddWolverineHttp();

builder.Host.UseWolverine(opts =>
{
    // Surely plenty of other configuration for Wolverine...

    // This *temporary* line of code will write out a full report about why or
    // why not Wolverine is finding this handler and its candidate handler messages
    // Console.WriteLine(opts.DescribeHandlerMatch(typeof(MyMissingMessageHandler)));
});

var dbConnectionString = Environment.GetEnvironmentVariable(EnvConstants.DbConnectionString);

if (dbConnectionString == null)
{
    throw new Exception("kokot");
}

builder.Services.WarehouseInstall(dbConnectionString);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseSwagger();

app.MapControllers();
app.MapWolverineEndpoints();

app.Run();