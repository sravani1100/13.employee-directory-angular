using AutoMapper;
using EmployeeDirectory.Application.Repositories.Interfaces;
using EmployeeDirectory.Application.DTO.RequestDTOs;
using EmployeeDirectory.Application.DTO.ResponseDTOs;
using EmployeeDirectory.Domain.Entities;
using EmployeeDirectory.Application.Exceptions;
using EmployeeDirectory.Application.Interfaces;
using Microsoft.Extensions.Logging;

namespace EmployeeDirectory.Application.Services;

public class LocationService : ILocationService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILocationRepository _locationRepository;
    private readonly ILogger<LocationService> _logger;

    public LocationService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILocationRepository locationRepository,
        ILogger<LocationService> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _locationRepository = locationRepository;
        _logger = logger;
    }


    public async Task<List<string>> GetAllAsync()
    {
        List<Location> locations =
            await _locationRepository.GetAllAsync();

        if (!locations.Any())
        {
            throw new ResourceNotFoundException("No locations found.");
        }

        return locations
            .Select(l => l.LocationName)
            .ToList();
    }

    public async Task<List<LocationResponseDTO>> GetAllLocationsAsync()
    {
        List<Location> locations =
            await _locationRepository.GetAllAsync();

        return _mapper.Map<List<LocationResponseDTO>>(locations);
    }

    public async Task<LocationResponseDTO?> GetLocationByIdAsync(int locationId)
    {
        if (locationId <= 0)
        {
            throw new InvalidFieldException("Invalid location id.");
        }

        Location? location =
            await _locationRepository.GetLocationByIdAsync(locationId);

        if (location == null)
        {
            throw new ResourceNotFoundException("Location not found.");
        }

        return _mapper.Map<LocationResponseDTO>(location);
    }

    public async Task<LocationResponseDTO> GetLocationByDepartmentIdAsync(int departmentId)
    {
        if (departmentId <= 0)
        {
            throw new InvalidFieldException("Invalid department id.");
        }

        Location? location =
            await _locationRepository.GetLocationByDepartmentIdAsync(departmentId);

        if (location == null)
        {
            throw new ResourceNotFoundException("Location not found.");
        }

        return _mapper.Map<LocationResponseDTO>(location);
    }

    public async Task<int> GetLocationIdByNameAsync(string locationName)
    {
        if (string.IsNullOrWhiteSpace(locationName))
        {
            throw new InvalidFieldException("Invalid location name.");
        }

        int locationId =
            await _locationRepository.GetLocationIdByNameAsync(locationName);

        if (locationId == 0)
        {
            throw new ResourceNotFoundException("Location not found.");
        }

        return locationId;
    }

    public async Task<LocationResponseDTO> AddLocationAsync(LocationRequestDTO request)
    {
        int existing =
            await _locationRepository.GetLocationIdByNameAsync(request.LocationName);

        if (existing != 0)
        {
            throw new InvalidFieldException("Location already exists.");
        }

        Location location =
            _mapper.Map<Location>(request);

        Location added =
            await _locationRepository.AddAsync(location);

        return _mapper.Map<LocationResponseDTO>(added);
    }

    public async Task<LocationResponseDTO> UpdateLocationAsync(
        int locationId,
        LocationRequestDTO request)
    {
        Location? location =
            await _locationRepository.GetLocationByIdAsync(locationId);

        if (location == null)
        {
            throw new ResourceNotFoundException("Location not found.");
        }

        _mapper.Map(request, location);

        Location? updated =
            await _locationRepository.UpdateAsync(location);

        return _mapper.Map<LocationResponseDTO>(updated);
    }

    public async Task DeleteLocationAsync(int locationId)
    {
        bool deleted =
            await _locationRepository.DeleteAsync(locationId);

        if (!deleted)
        {
            throw new ResourceNotFoundException("Location not found.");
        }
    }
}
