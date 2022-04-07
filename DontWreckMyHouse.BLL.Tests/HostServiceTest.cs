using DontWreckMyHouse.BLL.Tests.TestDoubles;
using DontWreckMyHouse.Core.Models;
using DontWreckMyHouse.DAL;
using NUnit.Framework;

namespace DontWreckMyHouse.BLL.Tests
{
    public class HostServiceTest
    {
        HostService hostService = new HostService(new HostRepositoryDouble());

        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public void FindByEmail_ExistingEmail_ReturnsHost()
        {
            Host expected = new Host("GUID-###1", "Doe", "JaneDoe@gmail.com", "(123) 123-1234", "1212 Everlane Rd", "Buffalo", "NY", "14201", 25, 50);
            Result<Host> actual = hostService.FindByEmail("JaneDoe@gmail.com");

            Assert.IsTrue(expected.Equals(actual.Data));
            Assert.IsTrue(actual.Success);
        }

        [Test]
        public void FindByEmail_NonexistentEmail_ReturnsNull()
        {
            Host expected = null;
            Result<Host> actual = hostService.FindByEmail("lemonade@yahoo.com");

            Assert.IsTrue(expected == actual.Data);
            Assert.IsFalse(actual.Success);
        }
    }
}