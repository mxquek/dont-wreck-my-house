using DontWreckMyHouse.Core.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DontWreckMyHouse.DAL.Tests
{
    public class ReservationRepositoryTest
    {
        static string CURRENT_DIRECTORY = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;
        static string DATA_DIRECTORY = Path.Combine(CURRENT_DIRECTORY, "data");

        static string SEED_DIRECTORY = "seed";
        static string SEED_DATA_DIRECTORY = "testReservationSeed";

        static string TEST_DIRECTORY = "test";
        static string TEST_DATA_DIRECTORY = "testReservations";

        string Seed_Path = Path.Combine(DATA_DIRECTORY, SEED_DIRECTORY, SEED_DATA_DIRECTORY);
        string Test_Path = Path.Combine(DATA_DIRECTORY, TEST_DIRECTORY, TEST_DATA_DIRECTORY);

        //H1R1 exists in Seed
        static DateTime H1R1StartDate = new DateTime(2022, 11, 11);
        static DateTime H1R1EndDate = new DateTime(2022, 11, 12);
        public static Reservation H1R1 = new Reservation(1, H1R1StartDate, H1R1EndDate, 1, 75);

        //H1R2 does not exist in Seed
        static DateTime H1R2StartDate = new DateTime(2022, 12, 9);
        static DateTime H1R2EndDate = new DateTime(2022, 12, 10);
        public static Reservation H1R2 = new Reservation(2, H1R2StartDate, H1R2EndDate, 1, 75);

        ReservationRepository reservationRepository;

        [SetUp]
        public void Setup()
        {
            if (!Directory.Exists(Test_Path))
            {
                Directory.CreateDirectory(Test_Path);
            }

            //Deletes all existing test files
            foreach(var testFile in Directory.GetFiles(Test_Path))
            {
                File.Delete(testFile);
            }

            //Clean copy of all seed files
            foreach(var srcPath in Directory.GetFiles(Seed_Path))
            {
                File.Copy(srcPath, srcPath.Replace(Seed_Path, Test_Path), true);
            }
            
            reservationRepository = new ReservationRepository(Test_Path);
        }

        [Test]
        public void GetReservationsByHostID_GivenExistingHostID_GetsAllReservationsForHost()
        {
            List<Reservation> expected = new List<Reservation>();
            expected.Add(new Reservation(H1R1));

            Result<List<Reservation>> actual = new Result<List<Reservation>>();
            actual.Data = new List<Reservation>();

            reservationRepository.GetReservationsByHostID(HostRepositoryTest.HOST1.ID, actual);

            Assert.AreEqual(expected.Count, actual.Data.Count);
            Assert.AreEqual(expected, actual.Data);
        }
        [Test]
        public void GetGetReservationsByHostID_GivenNonexistentHostID_GetsNoReservations()
        {
            List<Reservation> expected = new List<Reservation>();

            Result<List<Reservation>> actual = new Result<List<Reservation>>();
            actual.Data = new List<Reservation>();

            reservationRepository.GetReservationsByHostID("NonexistentHostID", actual);

            Assert.AreEqual(expected, actual.Data);
        }
        [Test]
        public void Add_GivenValidReservation_AddReservation()
        {
            Result<Reservation> actual = new Result<Reservation>();
            actual.Data = new Reservation(H1R2);

            reservationRepository.Add(actual, HostRepositoryTest.HOST1.ID);
            Assert.IsTrue(actual.Success);

            //Assuming GetReservationByHostID is functional
            Result<List<Reservation>> all = new Result<List<Reservation>>();
            all.Data = new List<Reservation>();
            reservationRepository.GetReservationsByHostID(HostRepositoryTest.HOST1.ID, all);

            Assert.IsTrue(all.Data.Any(r => r.Equals(actual.Data)));
        }

        [Test]
        public void Remove_GivenExistingReservation_RemoveReservation()
        {
            Result<Reservation> actual = new Result<Reservation>();
            actual.Data = new Reservation(H1R1);

            reservationRepository.Remove(actual, HostRepositoryTest.HOST1.ID);
            Assert.IsTrue(actual.Success);

            //Assuming GetReservationByHostID is functional
            Result<List<Reservation>> all = new Result<List<Reservation>>();
            all.Data = new List<Reservation>();
            reservationRepository.GetReservationsByHostID(HostRepositoryTest.HOST1.ID, all);

            Assert.IsFalse(all.Data.Any(r => r.Equals(actual.Data)));
            Assert.AreEqual(all.Data.Count, 0);
        }

        [Test]
        public void Edit_GivenExistingReservation_EditsReservation()
        {
            Result<Reservation> actual = new Result<Reservation>();
            actual.Data = new Reservation(H1R1);
            actual.Data.StartDate = new DateTime(2022, 11, 12);
            actual.Data.EndDate = new DateTime(2022, 11, 14);
            actual.Data.Total = 125;

            reservationRepository.Edit(actual, HostRepositoryTest.HOST1.ID, 0);
            Assert.IsTrue(actual.Success);

            //Assuming GetReservationByHostID is functional
            Result<List<Reservation>> all = new Result<List<Reservation>>();
            all.Data = new List<Reservation>();
            reservationRepository.GetReservationsByHostID(HostRepositoryTest.HOST1.ID, all);

            Assert.IsTrue(all.Data.Any(r => r.Equals(actual.Data)));
            Assert.IsFalse(all.Data.Any(r => r.Equals(ReservationRepositoryTest.H1R1)));
            Assert.AreEqual(all.Data.Count, 1);
        }

        [Test]
        public void Deserialize_ValidStringReservation_ReturnsReservation()
        {
            Reservation expected = new Reservation();
            expected.ID = 1;
            expected.StartDate = DateTime.Now.AddDays(-1).Date;
            expected.EndDate = DateTime.Now.Date;
            expected.GuestID = 1;
            expected.Total = 100;

            string stringReservation = $"{expected.ID},{expected.StartDate:yyyy-MM-dd},{expected.EndDate:yyyy-MM-dd},{expected.GuestID},{expected.Total}";
            Reservation actual = reservationRepository.Deserialize(stringReservation);

            Assert.AreEqual(expected, actual);
        }
        [Test]
        public void Deserialize_InvalidStringReservation_ReturnsReservation()
        {
            Reservation expected = new Reservation();
            expected.ID = 1;
            expected.StartDate = DateTime.Now.AddDays(-1).Date;
            expected.EndDate = DateTime.Now.Date;
            expected.GuestID = 1;
            expected.Total = 100;

            string stringReservation = $"{expected.ID},{expected.StartDate:yyyy-MM-dd},{expected.EndDate:yyyy-MM-dd},{expected.GuestID},{expected.Total},Extra";

            expected = null;
            Reservation actual = reservationRepository.Deserialize(stringReservation);

            Assert.AreEqual(expected, actual);
        }
        [Test]
        public void Serialize_Reservation_ReturnsStringReservation()
        {
            Reservation reservation = new Reservation();
            reservation.ID = 1;
            reservation.StartDate = DateTime.Now.AddDays(-1).Date;
            reservation.EndDate = DateTime.Now.Date;
            reservation.GuestID = 1;
            reservation.Total = 100;

            string expected = $"{reservation.ID},{reservation.StartDate:yyyy-MM-dd},{reservation.EndDate:yyyy-MM-dd},{reservation.GuestID},{reservation.Total}";
            string actual = reservationRepository.Serialize(reservation);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void WriteToFile_GivenNonexistentFile_CreatesNewFile()
        {
            string expectedPath = Path.Combine(Test_Path, "NonExistentFile.csv");

            reservationRepository.WriteToFile(new List<Reservation>(), "NonExistentFile");
            Assert.IsTrue(File.Exists(expectedPath));
        }
    }

}
