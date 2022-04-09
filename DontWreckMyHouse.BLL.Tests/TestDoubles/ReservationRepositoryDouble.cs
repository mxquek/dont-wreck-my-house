using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DontWreckMyHouse.Core.Interfaces;
using DontWreckMyHouse.Core.Models;

namespace DontWreckMyHouse.BLL.Tests.TestDoubles
{
    public class ReservationRepositoryDouble : IReservationRepository
    {
        public Result<Reservation> Add(Reservation reservation, string hostID)
        {
            throw new NotImplementedException();
        }

        public void Edit(Result<Reservation> updatedReservation, string hostID)
        {
            throw new NotImplementedException();
        }

        public Result<List<Reservation>> GetReservationsByHostID(string hostID)
        {
            throw new NotImplementedException();
        }

        public Result<Reservation> Remove(Reservation reservation, string hostID)
        {
            throw new NotImplementedException();
        }
    }
}
