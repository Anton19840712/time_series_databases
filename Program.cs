using Coravel;
using InfluxApp.Invocables;
using InfluxApp.Services;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;
services.AddControllers();
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();
services.AddControllersWithViews();
services.AddTransient<InfluxDBService>();
services.AddTransient<MessageProducer>();
services.AddScheduler();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
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
