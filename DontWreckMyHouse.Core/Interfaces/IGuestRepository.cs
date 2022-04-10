using DontWreckMyHouse.Core.Models;

namespace DontWreckMyHouse.Core.Interfaces
{
    public interface IGuestRepository
    {
        Result<List<Guest>> GetAll();
    }
}
