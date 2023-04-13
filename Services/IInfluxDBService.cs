using InfluxDB.Client;

namespace InfluxApp.Services;

public interface IInfluxDBService
{
	void Write(Action<WriteApi> action);
	Task<T> QueryAsync<T>(Func<QueryApi, Task<T>> action);
}