using System.ComponentModel.DataAnnotations;
using IdGenerator;
using Models;
using Models.DTOs;
using Repositories;

namespace Services;

public class HostService : IHostService
{
    private IHostRepository _hostRepository;
    private GeneratorId _idGenerator = new GeneratorId();
    
    public HostService(IHostRepository hostRepository)
    {
        _hostRepository = hostRepository;
    }

    public List<HostListDTO> GetHostsList()
    {
        var res = new List<HostListDTO>();

        foreach (var hostDBModel in _hostRepository.GetHosts())
        {
            res.Add(new HostListDTO(hostDBModel));
        }

        return res;
    }

    public HostDetailsDTO GetHost(int id)
    {
        var hostDbModel = _hostRepository.GetHost(id);
        if (hostDbModel == null)
            return null;
        return new HostDetailsDTO(hostDbModel);
    }

    public HostDetailsDTO GetHost(string name)
    {
        var hostDbModel = _hostRepository.GetHost(name);
        if (hostDbModel == null)
            return null;
        return new HostDetailsDTO(hostDbModel);
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
        var newHost = new Host(_idGenerator.GenerateHostId(), host.FirstName, host.LastName, host.Type, host.Email, host.Phone, host.DateOfBirth);
        _hostRepository.AddHost(newHost);
    }

    public void UpdateHost(HostDetailsDTO hostDetailsDto)
    {
        var errors = hostDetailsDto.Validate();
        if (errors.Count > 0)
            throw new ValidationException(String.Join(Environment.NewLine, errors.Select(s => s.errorMessage)));
        var hostDbModel = new Host(hostDetailsDto.Id,  hostDetailsDto.FirstName, hostDetailsDto.LastName, hostDetailsDto.Type, hostDetailsDto.Email, hostDetailsDto.Phone, hostDetailsDto.DateOfBirth);
        _hostRepository.UpdateHost(hostDbModel);
    }
}