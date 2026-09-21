using EmployeeDirectory.Application.DTO.RequestDTOs;
using EmployeeDirectory.Application.DTO.ResponseDTOs;
using EmployeeDirectory.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Authorize(Policy = "EmployeeRead")]
[ApiController]
[Route("api/[controller]")]
public class LocationController : ControllerBase
{
    private readonly ILocationService _locationService;

    public LocationController(ILocationService locationService)
    {
        _locationService = locationService;
    }

    [HttpGet]
    public async Task<ActionResult<List<LocationResponseDTO>>> GetLocations()
    {
        return Ok(await _locationService.GetAllLocationsAsync());
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<LocationResponseDTO>> GetLocationById(int id)
    {
        return Ok(await _locationService.GetLocationByIdAsync(id));
    }

    [HttpGet("department/{departmentId:int}")]
    public async Task<ActionResult<LocationResponseDTO>> GetLocationByDepartmentId(int departmentId)
    {
        return Ok(await _locationService.GetLocationByDepartmentIdAsync(departmentId));
    }

    [HttpGet("name/{locationName}")]
    public async Task<ActionResult<int>> GetLocationIdByName(string locationName)
    {
        return Ok(await _locationService.GetLocationIdByNameAsync(locationName));
    }

    [Authorize(Policy = "AdminOnly")]
    [HttpPost]
    public async Task<ActionResult<LocationResponseDTO>> AddLocation(
        LocationRequestDTO request)
    {
        LocationResponseDTO location =
            await _locationService.AddLocationAsync(request);

        return CreatedAtAction(
            nameof(GetLocationById),
            new { id = location.LocationId },
            location);
    }

    [Authorize(Policy = "AdminOnly")]
    [HttpPut("{id:int}")]
    public async Task<ActionResult<LocationResponseDTO>> UpdateLocation(
        int id,
        LocationRequestDTO request)
    {
        return Ok(await _locationService.UpdateLocationAsync(id, request));
    }

    [Authorize(Policy = "AdminOnly")]
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteLocation(int id)
    {
        await _locationService.DeleteLocationAsync(id);

        return NoContent();
    }
}

