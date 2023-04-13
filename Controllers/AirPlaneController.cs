using InfluxApp.Models;
using InfluxApp.Services;
using Microsoft.AspNetCore.Mvc;
namespace InfluxApp.Controllers;

[ApiController]
[Route("airplane")]
public class AirPlaneController : ControllerBase
{
	private readonly InfluxDBService _service;

	public AirPlaneController(InfluxDBService service)
	{
		_service = service;
	}
	[HttpGet("altitudes")]
	[ProducesResponseType(typeof(IEnumerable<AltitudeModel>), 200)]
	[ProducesResponseType(typeof(ProblemDetails), 500)]
	public async Task<IEnumerable<AltitudeModel>> GetAltitudes()
	{
		var flux = "from(bucket:\"test-bucket\") |> range(start: 0) |> filter(fn: (r) => r._measurement == \"altitude\" and r._value > 3500)";

		var results = await _service.QueryAsync(async query =>
		{
			var tables = await query.QueryAsync(flux, "organization");
			return tables.SelectMany(table => table.Records.Select(record => new AltitudeModel
			{
				Time = record.GetTime().ToString(),
				Altitude = int.Parse(record.GetValue().ToString() ?? string.Empty)
			}));
		});

		return results;
	}
}