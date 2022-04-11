using DontWreckMyHouse.Core.Models;

namespace DontWreckMyHouse.Core.Interfaces
{
    public interface IGuestRepository
    {
        public Guest FindByID(int guestID);
        Result<List<Guest>> GetAll();
    }
}
