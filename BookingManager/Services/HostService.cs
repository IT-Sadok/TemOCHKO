using Models.DTOs;
using Repositories;

namespace Services;

public class HostService : IHostService
{
    private IHostRepository _hostRepository;
    
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
}