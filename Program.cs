using Coravel;
using InfluxApp.Invocables;
using InfluxApp.Services;
using Microsoft.OpenApi.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;
services.AddControllers();
services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
	options.SwaggerDoc("v1",
		new OpenApiInfo
		{
			Title = "Swagger documentation",
			Version = "v1",
			Description = "Documentation",
			Contact = new OpenApiContact
			{
				Name = "Gridushko Anton",
				Email = "someemail@somedomain.com"
			}
		});
	var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
	var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
	options.IncludeXmlComments(xmlPath);
	options.EnableAnnotations();
});


services.AddControllersWithViews();
services.AddTransient<InfluxDBService>();
services.AddTransient<MessageProducer>();
services.AddScheduler();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI(options =>
		options.SwaggerEndpoint("/swagger/v1/swagger.json",
			"Swagger documentation v1"));
	app.UseReDoc(options =>
	{
		options.DocumentTitle = "Swagger Demo Documentation";
		options.SpecUrl = "/swagger/v1/swagger.json";
	});
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
((IApplicationBuilder)app).ApplicationServices.UseScheduler(scheduler =>
{
	scheduler
		.Schedule<MessageProducer>()
		.EveryFiveSeconds();
});
app.Run();
