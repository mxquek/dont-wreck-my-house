using DontWreckMyHouse.Core.Models;

namespace DontWreckMyHouse.Core.Interfaces
{
    public interface IReservationRepository
    {
        public void GetReservationsByHostID(string HostID, Result<List<Reservation>> result);
        public void Add(Result<Reservation> reservation, string hostID);
        public void Remove(Result<Reservation> reservation, string hostID);
        public void Edit(Result<Reservation> updatedReservation, string hostID);
    }
}
