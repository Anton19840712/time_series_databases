using InfluxApp.Models;
using InfluxApp.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace InfluxApp.Controllers;

/// <summary>
/// Airplanes altitudes
/// </summary>
[ApiController]
[Route("airplaneAltitudes")]
[SwaggerTag("Get airplane altitudes. Very weird airplane API :)")]
public class AirPlaneController : ControllerBase
{
	private readonly InfluxDBService _service;
	/// <summary>
	/// Constructor for Dependency Injection
	/// </summary>
	/// <param name="service"></param>
	public AirPlaneController(InfluxDBService service)
	{
		_service = service;
	}

	/// <summary>
	/// Get a list of altitudes for airplanes with an altitude greater than 3500 from InfluxDB.
	/// </summary>
	/// <returns>A list of <see cref="AltitudeModel"/> objects.</returns>
	[HttpGet("altitudes")]
	[SwaggerOperation(
		Summary = "Get a list of altitudes for airplanes with an altitude greater than 3500 from InfluxDB.",
		Description = "This endpoint will return altitudes of airplanes.",
		OperationId = "Get",
		Tags = new[] { "AirplaneAltitudes" })]

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