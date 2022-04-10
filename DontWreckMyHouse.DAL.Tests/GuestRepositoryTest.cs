using DontWreckMyHouse.Core.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;

namespace DontWreckMyHouse.DAL.Tests
{
    public class GuestRepositoryTest
    {
        static string CurrentDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;
        static string DATA_DIRECTORY = Path.Combine(CurrentDirectory,"data");
        const string SEED_DIRECTORY = "seed";
        const string SEED_FILE = "testGuestSeed.csv";
        const string TEST_DIRECTORY = "test";
        const string TEST_FILE = "testGuests.csv";

        string Seed_Path = Path.Combine(DATA_DIRECTORY, SEED_DIRECTORY, SEED_FILE);
        static string Test_Path = Path.Combine(DATA_DIRECTORY, TEST_DIRECTORY, TEST_FILE);

        public static Guest GUEST1 = new Guest(1, "John", "Smith", "JohnSmith@gmail.com", "(333) 333-3333", "TX");
        public static Guest GUEST2 = new Guest(2, "Terry", "Bob", "TBob@yahoo.com", "(444) 444-4444", "NV");

        GuestRepository guestRepository = new GuestRepository(Test_Path);

        [SetUp]
        public void Setup()
        {
            if (!Directory.Exists(TEST_DIRECTORY))
            {
                Directory.CreateDirectory(Path.Combine(DATA_DIRECTORY, TEST_DIRECTORY));
            }
            File.Copy(Seed_Path, Test_Path, true);
        }

        [Test]
        public void GetAll_Guests_ReturnsAllGuests()
        {
            Result<List<Guest>> expected = new Result<List<Guest>>();
            expected.Data = new List<Guest>();
            expected.Data.Add(GUEST1);
            expected.Data.Add(GUEST2);

            Result<List<Guest>> actual = guestRepository.GetAll();

            Assert.AreEqual(expected.Data, actual.Data);
        }

        [Test]
        public void Deserialize_StringGuest_ReturnsGuest()
        {
            Guest expected = new Guest(GUEST1);
            string stringGuest = "1,John,Smith,JohnSmith@gmail.com,(333) 333-3333,TX";
            Guest actual = guestRepository.Deserialize(stringGuest);

            Assert.AreEqual(expected, actual);
        }
    }
}
