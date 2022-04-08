using DontWreckMyHouse.Core.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;

namespace DontWreckMyHouse.DAL.Tests
{
    public class HostRepositoryTest
    {
        static string CurrentDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;
        static string DATA_DIRECTORY = Path.Combine(CurrentDirectory, "data");
        const string SEED_DIRECTORY = "seed";
        const string SEED_FILE = "testHostSeed.csv";
        const string TEST_DIRECTORY = "test";
        const string TEST_FILE = "testHosts.csv";

        string Seed_Path = Path.Combine(DATA_DIRECTORY, SEED_DIRECTORY, SEED_FILE);
        static string Test_Path = Path.Combine(DATA_DIRECTORY, TEST_DIRECTORY, TEST_FILE);

        HostRepository hostRepository;

        [SetUp]
        public void Setup()
        {
            if (!Directory.Exists(TEST_DIRECTORY))
            {
                Directory.CreateDirectory(Path.Combine(DATA_DIRECTORY, TEST_DIRECTORY));
            }
            File.Copy(Seed_Path, Test_Path, true);

            hostRepository = new HostRepository(Test_Path);
        }

        [Test]
        public void Deserialize_StringHost_ReturnsHost()
        {
            Host expected = new Host("GUID-####", "Doe", "JaneDoe@gmail.com", "(123) 123-4567", "1212 Everlane Rd", "Buffalo", "NY", "14201", 25, 50);
            string stringHost = "GUID-####,Doe,JaneDoe@gmail.com,(123) 123-4567,1212 Everlane Rd,Buffalo,NY,14201,25,50";
            Host actual = hostRepository.Deserialize(stringHost);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void GetAll_ReturnsAllHosts()
        {
            List<Host> expected = new List<Host>();
            expected.Add(new Host("GUID-###1","Doe","JaneDoe@gmail.com","(123) 123-1234","1212 Everlane Rd","Buffalo","NY","14201",25,50));
            expected.Add(new Host("123-4ge23-sff43", "Well", "ChristinaWell@yahoo.com", "(222) 222-2222", "4444 Oceanside Ave", "Plano", "TX", "75252", 10, 20));
            Result <List<Host>> actual = hostRepository.GetAll();
            int max;

            if(expected.Count < actual.Data.Count)
            {
                max = actual.Data.Count;
            }
            else
            {
                max = expected.Count;
            }

            for(int i = 0; i < max; i++)
            {
                Assert.IsTrue(expected[i].Equals(actual.Data[i]));
            }
        }
    }
}