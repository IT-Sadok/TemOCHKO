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

    public List<HostListItemDTO> GetHostsList()
    {
        var res = new List<HostListItemDTO>();

        foreach (var hostDbModel in _hostRepository.GetHosts())
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

    public HostDetailsDTO GetHost(int id)
    {
        var hostDbModel = _hostRepository.GetHost(id);
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

    public HostDetailsDTO GetHost(string name)
    {
        var hostDbModel = _hostRepository.GetHost(name);
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

    public bool RemoveHost(int id)
    {
        return _hostRepository.RemoveHost(id);
    }

    public void AddHost(HostCreateDTO host)
    {
        var errors = host.Validate();
        if (errors.Count > 0)
            throw new ValidationException(String.Join(Environment.NewLine, errors.Select(s => s.errorMessage)));
        _hostRepository.AddHost(host);
    }

    public void UpdateHost(HostDetailsDTO hostDetailsDto)
    {
        var errors = hostDetailsDto.Validate();
        if (errors.Count > 0)
            throw new ValidationException(String.Join(Environment.NewLine, errors.Select(s => s.errorMessage)));
        var hostDbModel = new Models.Host 
        {
            HostId = hostDetailsDto.Id, FirstName = hostDetailsDto.FirstName, LastName = hostDetailsDto.LastName, Type = hostDetailsDto.Type, Email = hostDetailsDto.Email, Phone = hostDetailsDto.Phone, DateOfBirth = hostDetailsDto.DateOfBirth
        };
        _hostRepository.UpdateHost(hostDbModel);
    }

    public void SaveHosts()
    {
        _hostRepository.SaveHosts();
    }

    public int GetHostsCount()
    {
        return _hostRepository.GetHostsCount();
    }
}