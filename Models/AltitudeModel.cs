namespace InfluxApp.Models;

/// <summary>
/// Model to push and pull data
/// </summary>
public class AltitudeModel
{
	/// <summary>
	/// Time of getting data
	/// </summary>
	public string Time { get; init; }
	/// <summary>
	/// Meters over
	/// </summary>
	public int Altitude { get; init; }
	/// <summary>
	/// Customer text message format
	/// </summary>
	public string DisplayText => $"Plane was at altitude {Altitude} ft. at {Time}.";
}