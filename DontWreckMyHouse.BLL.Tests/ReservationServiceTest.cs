using DontWreckMyHouse.BLL.Tests.TestDoubles;
using DontWreckMyHouse.Core.Models;
using DontWreckMyHouse.DAL.Tests;
using NUnit.Framework;
using System.Collections.Generic;

namespace DontWreckMyHouse.BLL.Tests
{
    public class ReservationServiceTest
    {
        ReservationService reservationService = new ReservationService(new HostRepositoryDouble(),new GuestRepositoryDouble(), new ReservationRepositoryDouble());



        [Test]
        public void GetReservationsByHostID_GivenExistingHostID_ReturnsListOfReservationsUnderHost()
        {
            Result<List<Reservation>> actual = new Result<List<Reservation>>();
            actual.Data = new List<Reservation>();
            reservationService.GetReservationsByHostID(actual, HostRepositoryTest.HOST1.ID);

            Assert.IsTrue(actual.Success);
        }

        [Test]
        public void GetReservationsByHostID_GivenNonexistentHostID_ReturnsEmptyList()
        {
            Result<List<Reservation>> actual = new Result<List<Reservation>>();
            actual.Data = new List<Reservation>();
            reservationService.GetReservationsByHostID(actual, "NonexistentHostID");

            Assert.IsFalse(actual.Success);
        }
    }

    
}
