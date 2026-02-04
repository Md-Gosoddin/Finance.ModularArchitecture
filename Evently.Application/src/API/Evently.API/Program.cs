using System.Reflection;
using BuildingBlock.Application;
using BuildingBlock.InfraStructure;
using BuildingBlock.Presentation.Endpoint;
using Evently.API.Extensions;
using Evently.Module.User.InfraStructure;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddSwaggerDocumentation();
builder.Services.AddSwaggerGen(options =>
{
    options.CustomSchemaIds(t => t.FullName?.Replace("+", "."));
});

builder.Configuration.AddModuleConfiguration(["users", "events", "ticketing", "attendance"]);
string databaseConnectionString = builder.Configuration.GetConnectionString("Database")!;
builder.Services.AddInfrastructure(databaseConnectionString);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHealthChecks().AddNpgSql(databaseConnectionString);

Assembly[] moduleApplicationAssemblies = [
    Evently.Module.User.Application.AssemblyReference.Assembly,
    ];
builder.Services.AddApplication(moduleApplicationAssemblies);

builder.Services.AddUsersModule(builder.Configuration);


WebApplication app = builder.Build();
app.UseHttpsRedirection();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.ApplyMigrations();

}
app.MapEndpoints();

app.Run();
