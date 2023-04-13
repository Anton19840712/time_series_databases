using InfluxDB.Client;

namespace InfluxApp.Services;

public class InfluxDBService : IInfluxDBService
{
	private readonly string _url;
	private readonly string _token;
	
	public InfluxDBService(IConfiguration configuration)
	{
		_url = configuration.GetValue<string>("InfluxDB:Url");
		_token = configuration.GetValue<string>("InfluxDB:Token");
	}

	public void Write(Action<WriteApi> action)
	{
		using var client = new InfluxDBClient(_url, _token);
		using var write = client.GetWriteApi();
		action(write);
	}
	
	public async Task<T> QueryAsync<T>(Func<QueryApi, Task<T>> action)
	{
		using var client = new InfluxDBClient(_url, _token);
		var query = client.GetQueryApi();
		return await action(query);
	}
}