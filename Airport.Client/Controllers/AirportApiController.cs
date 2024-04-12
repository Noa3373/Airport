using Airport.Client.Services;
using Airport.Data.Models;
using Microsoft.AspNetCore.Mvc;

namespace Airport.Client.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AirportApiController : ControllerBase
    {
        private readonly FlightService _flightService;

        public AirportApiController(FlightService flightService)
        {
            _flightService = flightService;
        }

        [HttpPatch]
        public async Task<IActionResult> PatchAirport([FromBody] Airplane airplane)
        {
            await _flightService.StartAirportSimulation(airplane);

            return Ok();
        }
    }
}
