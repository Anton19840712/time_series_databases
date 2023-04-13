using Coravel.Invocable;
using InfluxApp.Services;
using InfluxDB.Client.Api.Domain;
using InfluxDB.Client.Writes;

namespace InfluxApp.Invocables;

/// <summary>
/// Produces messages
/// </summary>
public class MessageProducer : IInvocable
{
	private readonly InfluxDBService _service;
	private static readonly Random _random = new();

	/// <summary>
	/// Constructor for DI
	/// </summary>
	/// <param name="service"></param>
	public MessageProducer(InfluxDBService service)
	{
		_service = service;
	}

	/// <summary>
	/// Invokes write action
	/// </summary>
	/// <returns></returns>
	public Task Invoke()
	{
		_service.Write(write =>
		{
			var point = PointData.Measurement("altitude")
				.Tag("plane", "test-plane")
				.Field("value", _random.Next(1000, 5000))
				.Timestamp(DateTime.UtcNow, WritePrecision.Ns);

			write.WritePoint(point,"test-bucket", "organization");
		});

		return Task.CompletedTask;
	}
}