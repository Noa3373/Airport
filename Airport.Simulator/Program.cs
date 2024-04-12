using Airport.Data.Models;
using Airport.Data.Repository;
using System.Net.Http.Json;

internal class Program
{
    static private Random _random = new();
    static private HttpClient _client = new HttpClient { BaseAddress = new Uri("https://localhost:7089/") };
    static private Timer _timer;

    private static void Main(string[] args)
    {
        _client.Timeout = TimeSpan.FromHours(1);
        _timer = new Timer(GenerateAirplaneCallback, null, TimeSpan.FromSeconds(10), TimeSpan.FromSeconds(15));
        Console.ReadLine();
    }

    private static async void GenerateAirplaneCallback(object state)
    {
        var newAirplane = GenerateAirplane();
        await SendAirplaneToClient(newAirplane);
    }

    private static async Task SendAirplaneToClient(Airplane airplane)
    {
        var res = await _client.PatchAsJsonAsync("api/AirportApi", airplane);
        if (res.IsSuccessStatusCode)
        {
            await Console.Out.WriteLineAsync(airplane.ToString());
            SetRandomTimerInterval();
        }
    }

    private static Airplane GenerateAirplane()
    {
        var airplan = new Airplane
        {
            FlightNumber = GenerateFlightNumber(),
            Brand = GenerateBrand(),
        };

        airplan.ImgPath = GenerateImgPath(airplan);
        return airplan;
    }

    private static void SetRandomTimerInterval()
    {
        int randomInterval = _random.Next(15000, 25001);
        _timer.Change(TimeSpan.FromMilliseconds(randomInterval), TimeSpan.FromMilliseconds(-1));
    }

    private static string GenerateFlightNumber() => $"{(char)('A' + _random.Next(0, 26))}{(char)('A' + _random.Next(0, 26))}{_random.Next(1000, 10000)}";
    private static string GenerateBrand() => $"Sky-{_random.Next(16, 19)}";
    private static string GenerateImgPath(Airplane airplane) => $"/images/{airplane.Brand}.svg";
}