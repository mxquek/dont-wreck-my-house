using DontWreckMyHouse.Core.Interfaces;
using DontWreckMyHouse.Core.Models;

namespace DontWreckMyHouse.BLL
{
    public class HostService
    {
        public IHostRepository HostRepository;

        public HostService(IHostRepository repo)
        {
            HostRepository = repo;
        }
        
    }
}