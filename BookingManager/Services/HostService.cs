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
}