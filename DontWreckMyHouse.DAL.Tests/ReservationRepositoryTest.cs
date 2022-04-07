using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DontWreckMyHouse.DAL.Tests
{
    public class ReservationRepositoryTest
    {
        const string DATA_DIRECTORY = "data";
        const string SEED_DIRECTORY = "seed";
        const string SEED_FILE = "testReservationSeed.csv";
        const string TEST_DIRECTORY = "test";
        const string TEST_FILE = "testReservations.csv";

        string Seed_Path = Path.Combine(DATA_DIRECTORY, SEED_DIRECTORY, SEED_FILE);
        string Test_Path = Path.Combine(DATA_DIRECTORY, TEST_DIRECTORY, TEST_FILE);

        ReservationRepository reservationRepository;

        [SetUp]
        public void Setup()
        {
            if (!Directory.Exists(TEST_DIRECTORY))
            {
                Directory.CreateDirectory(Path.Combine(DATA_DIRECTORY, TEST_DIRECTORY));
            }
            File.Copy(Seed_Path, Test_Path, true);

            //reservationRepository = new ReservationRepository(Test_Path, new ReservationFormatter());
        }

        [Test]
        public void Test1()
        {
            Assert.Pass();
        }
    }
}
