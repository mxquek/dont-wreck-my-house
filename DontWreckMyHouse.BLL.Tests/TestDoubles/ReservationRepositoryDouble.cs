using System;
using System.Collections.Generic;
using DontWreckMyHouse.Core.Interfaces;
using DontWreckMyHouse.Core.Models;

namespace DontWreckMyHouse.BLL.Tests.TestDoubles
{
    public class ReservationRepositoryDouble : IReservationRepository
    {
        public void Add(Result<Reservation> reservation, string hostID)
        {
            throw new NotImplementedException();
        }

        public void Edit(Result<Reservation> updatedReservation, string hostID)
        {
            throw new NotImplementedException();
        }

        public void GetReservationsByHostID(string HostID, Result<List<Reservation>> result)
        {
            throw new NotImplementedException();
        }

        public void Remove(Result<Reservation> reservation, string hostID)
        {
            throw new NotImplementedException();
        }
    }
}
