using DontWreckMyHouse.Core.Models;

namespace DontWreckMyHouse.Core.Interfaces
{
    public interface IHostRepository
    {
        public Host FindByID(string hostID);
        public Result<List<Host>> GetAll();
    }
}
