using System.ComponentModel.DataAnnotations;
using Models.DTOs;
using Repositories.Host;

namespace Services.Host;

public class HostService : IHostService
{
    private IHostRepository _hostRepository;
    
    public HostService(IHostRepository hostRepository)
    {
        _hostRepository = hostRepository;
    }

    public async Task<List<HostListItemDTO>> GetHostsListAsync()
    {
        var res = new List<HostListItemDTO>();
        var hostDbModels = await _hostRepository.GetHostsAsync();
        
        foreach (var hostDbModel in hostDbModels)
        {
            res.Add(new HostListItemDTO
            {
                Id = hostDbModel.HostId,
                FirstName = hostDbModel.FirstName,
                LastName = hostDbModel.LastName,
                Type = hostDbModel.Type,
                Phone =  hostDbModel.Phone,
            });
        }

        return res;
    }

    public async Task<HostDetailsDTO> GetHostAsync(int id)
    {
        var hostDbModel = await _hostRepository.GetHostAsync(id);
        if (hostDbModel == null) return null;
        return new HostDetailsDTO
        {
            Id = hostDbModel.HostId,
            FirstName = hostDbModel.FirstName,
            LastName = hostDbModel.LastName,
            Type = hostDbModel.Type,
            Email = hostDbModel.Email,
            Phone = hostDbModel.Phone,
            DateOfBirth = hostDbModel.DateOfBirth
        };
    }

    public async Task<HostDetailsDTO> GetHostAsync(string name)
    {
        var hostDbModel = await _hostRepository.GetHostAsync(name);
        if (hostDbModel == null) return null;
        return new HostDetailsDTO
        {
            Id = hostDbModel.HostId,
            FirstName = hostDbModel.FirstName,
            LastName = hostDbModel.LastName,
            Type = hostDbModel.Type,
            Email = hostDbModel.Email,
            Phone = hostDbModel.Phone,
            DateOfBirth = hostDbModel.DateOfBirth
        };
    }

    public Task<bool> RemoveHostAsync(int id)
    {
        return _hostRepository.RemoveHostAsync(id);
    }

    public async Task AddHostAsync(HostCreateDTO host)
    {
        var errors = host.Validate();
        if (errors.Count > 0)
            throw new ValidationException(String.Join(Environment.NewLine, errors.Select(s => s.errorMessage)));
        await _hostRepository.AddHostAsync(host);
    }

    public async Task UpdateHostAsync(HostDetailsDTO hostDetailsDto)
    {
        var errors = hostDetailsDto.Validate();
        if (errors.Count > 0)
            throw new ValidationException(String.Join(Environment.NewLine, errors.Select(s => s.errorMessage)));
        var hostDbModel = new Models.Host 
        {
            HostId = hostDetailsDto.Id, FirstName = hostDetailsDto.FirstName, LastName = hostDetailsDto.LastName, Type = hostDetailsDto.Type, Email = hostDetailsDto.Email, Phone = hostDetailsDto.Phone, DateOfBirth = hostDetailsDto.DateOfBirth
        };
        await _hostRepository.UpdateHostAsync(hostDbModel);
    }

    public Task SaveHostsAsync()
    {
        return _hostRepository.SaveHostsAsync();
    }

    public Task<int> GetHostsCountAsync()
    {
        return _hostRepository.GetHostsCountAsync();
    }
}