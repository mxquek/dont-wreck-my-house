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
            Guest guest1 = new Guest(1, "Sullivan", "Lomas", "slomas0@mediafire.com", "(702) 7768761", "NV");
            Guest guest2 = new Guest(2, "Olympie", "Gecks", "ogecks1@dagondesign.com", "(202) 2528316", "DC");
            expected.Data.Add(guest1);
            expected.Data.Add(guest2);

            Result<List<Guest>> actual = guestRepository.GetAll();

            Assert.AreEqual(expected.Data, actual.Data);
        }

        [Test]
        public void Deserialize_StringGuest_ReturnsGuest()
        {
            Guest expected = new Guest(1, "John", "Smith", "JohnSmith@gmail.com", "(727) 123-4567", "TX");
            string stringGuest = "1,John,Smith,JohnSmith@gmail.com,(727) 123-4567,TX";
            Guest actual = guestRepository.Deserialize(stringGuest);

            Assert.AreEqual(expected, actual);
        }
    }
}
