using DontWreckMyHouse.Core.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;

namespace DontWreckMyHouse.DAL.Tests
{
    public class ReservationRepositoryTest
    {
        static string CurrentDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;
        static string DATA_DIRECTORY = Path.Combine(CurrentDirectory, "data");
        const string SEED_DIRECTORY = "seed";
        static string SEED_DATA_DIRECTORY = "testReservationSeed";

        static string TEST_DIRECTORY = "test";
        static string TEST_DATA_DIRECTORY = "testReservations";

        string Seed_Path = Path.Combine(DATA_DIRECTORY, SEED_DIRECTORY, SEED_DATA_DIRECTORY);
        string Test_Path = Path.Combine(DATA_DIRECTORY, TEST_DIRECTORY, TEST_DATA_DIRECTORY);

        public static Host HOST1 = new Host("GUID-1111", "Doe", "JaneDoe@gmail.com", "(111) 111-1111", "1212 Everlane Rd", "Buffalo", "NY", "14201", 25, 50);
        public static Host HOST2 = new Host("GUID-2222", "Well", "ChristinaWell@yahoo.com", "(222) 222-2222", "4444 Oceanside Ave", "Plano", "TX", "75252", 10, 20);

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

        //Guest GUEST = new Guest(11, "Bob", "Jones", "BobJones@yahoo.com", "(111) 111-1111", "OH");

        [Test]
        public void Deserialize_StringReservation_ReturnsReservation()
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

        [Test]
        public void GetReservationsByHostID_GivenExistingHostID_GetsAllReservationsForHost()
        {

        }
    }
}
