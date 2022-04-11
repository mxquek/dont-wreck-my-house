using DontWreckMyHouse.BLL.Tests.TestDoubles;
using DontWreckMyHouse.Core.Models;
using DontWreckMyHouse.DAL.Tests;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DontWreckMyHouse.BLL.Tests
{
    public class ReservationServiceTest
    {
        ReservationService reservationService;
        [SetUp]
        public void Setup()
        {
            reservationService = new ReservationService(new HostRepositoryDouble(), new GuestRepositoryDouble(), new ReservationRepositoryDouble());
        }

        [Test]
        public void GetReservationsByHostID_GivenExistingHostID_ReturnsListOfReservationsUnderHost()
        {
            Result<List<Reservation>> actual = new Result<List<Reservation>>();
            actual.Data = new List<Reservation>();
            reservationService.GetReservationsByHostID(actual, HostRepositoryTest.HOST1.ID);

            Assert.AreEqual(1, actual.Data.Count);
            Assert.IsTrue(actual.Success);
        }

        [Test]
        public void GetReservationsByHostID_GivenNonexistentHostID_ReturnsEmptyList()
        {
            Result<List<Reservation>> actual = new Result<List<Reservation>>();
            actual.Data = new List<Reservation>();
            reservationService.GetReservationsByHostID(actual, "NonexistentHostID");

            Assert.AreEqual(0, actual.Data.Count);
            Assert.IsFalse(actual.Success);
        }

        [Test]
        public void GetReservationsByHostID_GivenHostWithNoReservations_ReturnsEmptyList()
        {
            Result<List<Reservation>> actual = new Result<List<Reservation>>();
            actual.Data = new List<Reservation>();
            reservationService.GetReservationsByHostID(actual, HostRepositoryTest.HOST2.ID);

            Assert.AreEqual(0, actual.Data.Count);
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
        public void Add_GivenNonexistentGuestID_DoesNotAdd()
        {
            Host host = HostRepositoryTest.HOST1;

            Result<Reservation> actual = new Result<Reservation>();
            actual.Data = new Reservation(ReservationRepositoryTest.H1R1);
            actual.Data.GuestID = 1000;

            reservationService.Add(actual, host);

            Assert.IsFalse(actual.Success);
        }

        [Test]
        public void Add_GivenInvalidGuestID_DoesNotAdd()
        {
            Host host = HostRepositoryTest.HOST1;

            Result<Reservation> actual = new Result<Reservation>();
            actual.Data = new Reservation(ReservationRepositoryTest.H1R1);
            actual.Data.GuestID = -1000;

            reservationService.Add(actual, host);

            Assert.IsFalse(actual.Success);
        }

        [Test]
        public void Add_ValidReservation_AddsReservation()
        {
            Host host = HostRepositoryTest.HOST1;

            Result<Reservation> actual = new Result<Reservation>();
            actual.Data = new Reservation(ReservationRepositoryTest.H1R2);

            reservationService.Add(actual, host);

            Assert.IsTrue(actual.Success);
        }


        [Test]
        public void Remove_GivenExistingReservation_IsSuccessful()
        {
            Result<Reservation> actual = new Result<Reservation>();
            actual.Data = new Reservation(ReservationRepositoryTest.H1R1);

            reservationService.Remove(actual, HostRepositoryTest.HOST1);
            Assert.IsTrue(actual.Success);
        }
        [Test]
        public void Remove_GivenNonexistentReservation_DoesNotRemoveReservation()
        {
            Result<Reservation> actual = new Result<Reservation>();
            actual.Data = new Reservation();

            reservationService.Remove(actual, HostRepositoryTest.HOST1);
            Assert.IsFalse(actual.Success);

            Result<List<Reservation>> all = new Result<List<Reservation>>();
            all.Data = new List<Reservation>();
            reservationService.ReservationRepository.GetReservationsByHostID(HostRepositoryTest.HOST1.ID, all);

            Assert.IsFalse(all.Data.Any(r => r.Equals(actual.Data)));
            Assert.AreEqual(all.Data.Count, 1);
        }

        [Test]
        public void Edit_GivenExistingReservation_IsSuccessful()
        {
            Result<Reservation> actual = new Result<Reservation>();
            actual.Data = new Reservation(ReservationRepositoryTest.H1R1);
            actual.Data.StartDate = new DateTime(2022, 11, 12);
            actual.Data.EndDate = new DateTime(2022, 11, 14);
            actual.Data.Total = 125;

            reservationService.Edit(actual, HostRepositoryTest.HOST1);
            Assert.IsTrue(actual.Success);
        }
        [Test]
        public void Edit_GivenNonexistentReservation_DoesNotEditReservation()
        {
            Result<Reservation> actual = new Result<Reservation>();
            actual.Data = new Reservation();

            reservationService.Edit(actual, HostRepositoryTest.HOST1);
            Assert.IsFalse(actual.Success);

            Result<List<Reservation>> all = new Result<List<Reservation>>();
            all.Data = new List<Reservation>();
            reservationService.ReservationRepository.GetReservationsByHostID(HostRepositoryTest.HOST1.ID, all);

            Assert.IsFalse(all.Data.Any(r => r.Equals(actual.Data)));
            Assert.AreEqual(all.Data.Count, 1);
        }

        [Test]
        public void GetNextReservationID_GivenHostWithReservations_ReturnsNextID()
        {
            int expected = 2;
            int actual = reservationService.GetNextReservationID(HostRepositoryTest.HOST1.ID);

            Assert.AreEqual(expected, actual);
        }
        [Test]
        public void GetNextReservationID_GivenHostWithNoReservations_Returns1()
        {
            int expected = 1;
            int actual = reservationService.GetNextReservationID(HostRepositoryTest.HOST2.ID);

            Assert.AreEqual(expected, actual);
        }
        [Test]
        public void GetNextReservationID_GivenExistingReservationID_ReturnsExistingReservationID()
        {
            int expected = ReservationRepositoryTest.H1R1.ID;
            int actual = reservationService.GetNextReservationID(HostRepositoryTest.HOST1.ID, ReservationRepositoryTest.H1R1.ID);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void CalculateTotal_GivenInfo_ReturnsTotal()
        {
            decimal expected = 75;
            decimal actual = reservationService.CalculateTotal(HostRepositoryTest.HOST1, ReservationRepositoryTest.H1R1.StartDate, ReservationRepositoryTest.H1R1.EndDate);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ValidateReservation_NullHost_IsUnsuccessful()
        {
            Result<Reservation> result = new Result<Reservation>();
            result.Data = new Reservation(ReservationRepositoryTest.H1R1);

            Host host = null;

            reservationService.ValidateReservation(result, host);
            Assert.IsFalse(result.Success);
        }
        [Test]
        public void ValidateReservation_NonexistentGuestID_IsUnsuccessful()
        {
            Result<Reservation> result = new Result<Reservation>();
            result.Data = new Reservation(ReservationRepositoryTest.H1R1);
            result.Data.GuestID = 5;

            Host host = HostRepositoryTest.HOST1;

            reservationService.ValidateReservation(result, host);
            Assert.IsFalse(result.Success);
        }
        [Test]
        public void ValidateReservation_NonexistentHost_IsUnsuccessful()
        {
            Result<Reservation> result = new Result<Reservation>();
            result.Data = new Reservation(ReservationRepositoryTest.H1R1);

            Host host = new Host();

            reservationService.ValidateReservation(result, host);
            Assert.IsFalse(result.Success);
        }
        [Test]
        public void ValidateReservation_EndDateBeforeStartDate_IsUnsuccessful()
        {
            Result<Reservation> result = new Result<Reservation>();
            result.Data = new Reservation(ReservationRepositoryTest.H1R1);
            DateTime temp = new DateTime();
            temp = result.Data.EndDate;
            result.Data.EndDate = result.Data.StartDate;
            result.Data.StartDate = temp;

            Host host = new Host(HostRepositoryTest.HOST1);

            reservationService.ValidateReservation(result, host);
            Assert.IsFalse(result.Success);
        }
        [Test]
        public void ValidateReservation_DatesOverlapExistingReservation_IsUnsuccessful()
        {
            Result<Reservation> result = new Result<Reservation>();
            result.Data = new Reservation(ReservationRepositoryTest.H1R1);
            result.Data.ID = 2;


            Host host = new Host(HostRepositoryTest.HOST1);

            reservationService.ValidateReservation(result, host);
            Assert.IsFalse(result.Success);
        }
        [Test]
        public void ValidateReservation_ValidReservation_IsSuccessful()
        {
            Result<Reservation> result = new Result<Reservation>();
            result.Data = new Reservation(ReservationRepositoryTest.H1R1);


            Host host = HostRepositoryTest.HOST1;

            reservationService.ValidateReservation(result, host);
            Assert.IsTrue(result.Success);
        }
    }
    
}
