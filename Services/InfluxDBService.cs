using InfluxDB.Client;

namespace InfluxApp.Services;
/// <summary>
/// Influx service
/// </summary>
public class InfluxDBService
{
	private readonly string _url;
	private readonly string _token;

	/// <summary>
	/// Influx constructor
	/// </summary>
	/// <param name="configuration"></param>
	public InfluxDBService(IConfiguration configuration)
	{
		_url = configuration.GetValue<string>("InfluxDB:Url");
		_token = configuration.GetValue<string>("InfluxDB:Token");
	}
	
	/// <summary>
	/// Writes random data
	/// </summary>
	/// <param name="action"></param>
	public void Write(Action<WriteApi> action)
	{
		using var client = new InfluxDBClient(_url, _token);
		using var write = client.GetWriteApi();
		action(write);
	}
	
	/// <summary>
	/// Query random data
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="action"></param>
	/// <returns></returns>
	public async Task<T> QueryAsync<T>(Func<QueryApi, Task<T>> action)
	{
		using var client = new InfluxDBClient(_url, _token);
		var query = client.GetQueryApi();
		return await action(query);
	}
}