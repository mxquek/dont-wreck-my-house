using DontWreckMyHouse.BLL.Tests.TestDoubles;
using DontWreckMyHouse.Core.Models;
using DontWreckMyHouse.DAL.Tests;
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
            Host expected = new Host(HostRepositoryTest.HOST1);
            Result<Host> actual = hostService.FindByEmail(HostRepositoryTest.HOST1.Email);

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