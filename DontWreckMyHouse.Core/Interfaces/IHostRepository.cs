using DontWreckMyHouse.Core.Models;

namespace DontWreckMyHouse.Core.Interfaces
{
    public interface IHostRepository
    {
        public Result<List<Host>> GetAll();
    }
}
