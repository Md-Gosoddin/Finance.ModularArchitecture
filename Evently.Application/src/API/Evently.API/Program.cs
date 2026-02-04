using BuildingBlock.InfraStructure;
using BuildingBlock.Presentation.Endpoint;
using Evently.API.Extensions;


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

WebApplication app = builder.Build();
app.UseHttpsRedirection();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.MapEndpoints();

app.Run();
