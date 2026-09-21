using EmployeeDirectory.Application.DTO.RequestDTOs;
using EmployeeDirectory.Application.DTO.ResponseDTOs;

namespace EmployeeDirectory.Application.Interfaces;

public interface ILocationService
{
    Task<List<string>> GetAllAsync();

    Task<List<LocationResponseDTO>> GetAllLocationsAsync();

    Task<LocationResponseDTO?> GetLocationByIdAsync(int locationId);

    Task<LocationResponseDTO> GetLocationByDepartmentIdAsync(int departmentId);

    Task<int> GetLocationIdByNameAsync(string locationName);

    Task<LocationResponseDTO> AddLocationAsync(LocationRequestDTO request);

    Task<LocationResponseDTO> UpdateLocationAsync(
        int locationId,
        LocationRequestDTO request);

    Task DeleteLocationAsync(int locationId);
}

