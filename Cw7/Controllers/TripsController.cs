using Microsoft.AspNetCore.Mvc;
using Cw7.Services;

namespace Cw7.Controllers;

[ApiController]
[Route("[controller]")]
public class TripsController : ControllerBase
{
    private readonly IDatabaseService _databaseService;

    public TripsController(IDatabaseService databaseService)
    {
        _databaseService = databaseService;
    }

    [HttpGet]
    public async Task<IActionResult> GetTrips()
    {
        var trips = await _databaseService.GetTrips();
        return Ok(trips);
    }

    [HttpDelete]
    public async Task<IActionResult> RemoveClient(int id)
    {
        var tripsCount = await _databaseService.GetClientTripsCount(id);
        if (tripsCount == 0)
        {
            await _databaseService.RemoveClient(id);
            return Ok();
        }
        else
        {
            return Problem("Client cannot be removed");
        }
    }

    [HttpPost("{idTrip}/clients")]
    public async Task<IActionResult> AssignClientToTrip(int idTrip, ClientToTripDTO clientToTripDTO)
    {
        var doesClientExists = await _databaseService.DoesClientExists(clientToTripDTO);
        if (!doesClientExists)
        {
            await _databaseService.AddClient(clientToTripDTO);
        }

        var doesClientHasTrip = await _databaseService.DoesClientHasTrip(clientToTripDTO);
        if (!doesClientHasTrip)
        {
            return BadRequest("Client already has this trip");
        }
        var doesTripExists = await _databaseService.DoesTripExists(clientToTripDTO);
        if (!doesTripExists)
        {
            return NotFound("Trip does not exists");
        }

        await _databaseService.AssignClientToTrip(clientToTripDTO);
        return Ok();
    }
}

