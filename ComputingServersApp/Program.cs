using ComputingServers.Infrastructure;
using ComputingServersApp.Extensions;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddLoggerService(builder);

Infrastructure.Configure(builder.Services, builder.Configuration);

builder.Services.AddSwaggerGen(options =>
{
	options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
	{
		Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
		Name = "Authorization",
		In = ParameterLocation.Header,
		Type = SecuritySchemeType.Http,
		Scheme = "bearer"
	});

	options.OperationFilter<SecurityRequirementsOperationFilter>();

	options.CustomSchemaIds(t => t.FullName?.Replace("+", "."));
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
	await app.ApplyMigrationsAsync();
}


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
