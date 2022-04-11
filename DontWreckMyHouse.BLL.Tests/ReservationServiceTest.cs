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

        [Test]
        public void Add_GivenNullHost_DoesNotAdd()
        {
            Host nullHost = null;

            Result<Reservation> actual = new Result<Reservation>();
            actual.Data = ReservationRepositoryTest.H1R2;

            reservationService.Add(actual, nullHost);

            Assert.IsFalse(actual.Success);
        }

        [Test]
        public void Add_GivenNullReservation_DoesNotAdd()
        {
            Host host = HostRepositoryTest.HOST1;

            Result<Reservation> actual = new Result<Reservation>();
            actual.Data = null;

            reservationService.Add(actual, host);

            Assert.IsFalse(actual.Success);
        }

        [Test]
        public void Add_GivenNonexistentGuestIDReservation_DoesNotAdd()
        {
            Host host = HostRepositoryTest.HOST1;

            Result<Reservation> actual = new Result<Reservation>();
            actual.Data = ReservationRepositoryTest.H1R1;
            actual.Data.GuestID = 1000;

            reservationService.Add(actual, host);

            Assert.IsFalse(actual.Success);
        }

        [Test]
        public void Add_GivenInvalidGuestIDReservation_DoesNotAdd()
        {
            Host host = HostRepositoryTest.HOST1;

            Result<Reservation> actual = new Result<Reservation>();
            actual.Data = ReservationRepositoryTest.H1R1;
            actual.Data.GuestID = -1000;

            reservationService.Add(actual, host);

            Assert.IsFalse(actual.Success);
        }

        [Test]
        public void Add_ValidReservation_AddsReservation()
        {
            Host host = HostRepositoryTest.HOST1;

            Result<Reservation> actual = new Result<Reservation>();
            actual.Data = ReservationRepositoryTest.H1R2;

            reservationService.Add(actual, host);

            Assert.IsTrue(actual.Success);
        }
    }

    
}
