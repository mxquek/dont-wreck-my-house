using DontWreckMyHouse.Core.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DontWreckMyHouse.DAL.Tests
{
    public class GuestRepositoryTest
    {
        const string DATA_DIRECTORY = "data";
        const string SEED_DIRECTORY = "seed";
        const string SEED_FILE = "testGuestSeed.csv";
        const string TEST_DIRECTORY = "test";
        const string TEST_FILE = "testGuests.csv";

        string Seed_Path = Path.Combine(DATA_DIRECTORY, SEED_DIRECTORY, SEED_FILE);
        string Test_Path = Path.Combine(DATA_DIRECTORY, TEST_DIRECTORY, TEST_FILE);

        GuestRepository guestRepository;

        [SetUp]
        public void Setup()
        {
            if (!Directory.Exists(TEST_DIRECTORY))
            {
                Directory.CreateDirectory(Path.Combine(DATA_DIRECTORY, TEST_DIRECTORY));
            }
            File.Copy(Seed_Path, Test_Path, true);

            //guestRepository = new GuestRepository(Test_Path, new GuestFormatter());
        }

        [Test]
        public void Test1()
        {

        }
    }
}
