using DontWreckMyHouse.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DontWreckMyHouse.Core.Interfaces
{
    public interface IReservationRepository
    {
        public void GetReservationsByHostID(string HostID, Result<List<Reservation>> result);
        public Result<Reservation> Add(Reservation reservation, string hostID);
        public Result<Reservation> Remove(Reservation reservation, string hostID);
        public void Edit(Result<Reservation> updatedReservation, string hostID);
    }
}
