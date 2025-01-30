using DieselTimeDeliveries;
using DieselTimeDeliveries.ServiceDefaults;
using Oakton.Resources;
using Warehouse;
using Warehouse.Application;
using Wolverine;
using Wolverine.Http;

var builder = WebApplication.CreateBuilder(args);
const string serviceName = "DieselTimeDeliveries";

builder.AddServiceDefaults(serviceName);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddWolverineHttp();
builder.Services.AddControllers();

builder.Host.UseWolverine(opts =>
{
    // Surely plenty of other configuration for Wolverine...

    // This *temporary* line of code will write out a full report about why or
    // why not Wolverine is finding this handler and its candidate handler messages
    // Console.WriteLine(opts.DescribeHandlerMatch(typeof(AddPackageHandler)));
    
    opts.ServiceName = serviceName;
    opts.Policies.MessageSuccessLogLevel(LogLevel.Debug);
    opts.Policies.LogMessageStarting(LogLevel.Information);
    opts.Policies.MessageExecutionLogLevel(LogLevel.Information);
});

var dbConnectionString = Environment.GetEnvironmentVariable(EnvConstants.DbConnectionString);

if (dbConnectionString == null)
{
    throw new Exception("kokot");
}
Console.WriteLine(dbConnectionString);
Console.WriteLine("--------------------------");
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
app.MapDefaultEndpoints();

app.Run();