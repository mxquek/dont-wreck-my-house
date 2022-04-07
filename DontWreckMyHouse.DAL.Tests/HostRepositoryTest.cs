using NUnit.Framework;
using System.IO;

namespace DontWreckMyHouse.DAL.Tests
{
    public class HostRepositoryTest
    {
        const string DATA_DIRECTORY = "data";
        const string SEED_DIRECTORY = "seed";
        const string SEED_FILE = "testHostSeed.csv";
        const string TEST_DIRECTORY = "test";
        const string TEST_FILE = "testHosts.csv";

        string Seed_Path = Path.Combine(DATA_DIRECTORY, SEED_DIRECTORY, SEED_FILE);
        string Test_Path = Path.Combine(DATA_DIRECTORY, TEST_DIRECTORY, TEST_FILE);

        HostRepository hostRepository;

        [SetUp]
        public void Setup()
        {
            if (!Directory.Exists(TEST_DIRECTORY))
            {
                Directory.CreateDirectory(Path.Combine(DATA_DIRECTORY, TEST_DIRECTORY));
            }
            File.Copy(Seed_Path, Test_Path, true);

            //hostRepository = new HostRepository(Test_Path, new HostFormatter());
        }

        [Test]
        public void Test1()
        {
            Assert.Pass();
        }
    }
}