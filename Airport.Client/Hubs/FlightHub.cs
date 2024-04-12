using Airport.Client.DTOs;
using Microsoft.AspNetCore.SignalR;

namespace Airport.Client.Hubs
{
    public class FlightHub : Hub
    {
        public async Task SendFlight(FlightDTO flight)
        {
            await Clients.All.SendAsync("ReceiveFlight", flight);
        }
    }
}
