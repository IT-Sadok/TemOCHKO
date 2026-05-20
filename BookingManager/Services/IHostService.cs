using Models.DTOs;

namespace Services;

public interface IHostService
{
    List<HostListDTO> GetHostsList();
}