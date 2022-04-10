using DontWreckMyHouse.BLL.Tests.TestDoubles;
using DontWreckMyHouse.Core.Models;
using DontWreckMyHouse.DAL.Tests;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

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

        [Test]
        public void FindByLastName_ExistingLastName_ReturnsHost()
        {
            Host expected = new Host(HostRepositoryTest.HOST1);
            Result<List<Host>> actual = new Result<List<Host>>();
            actual = hostService.FindByLastName(HostRepositoryTest.HOST1.LastName);

            Assert.AreEqual(actual.Data.Count, 1);
            Assert.IsTrue(actual.Data.Any(host => host.Equals(expected)));
            Assert.IsTrue(actual.Success);
        }

        [Test]
        public void FindByLastName_GivenNonexistentLastName_ReturnsNoHosts()
        {
            Result<List<Host>> actual = new Result<List<Host>>();
            actual = hostService.FindByLastName("NonexistentLastName");

            Assert.AreEqual(actual.Data.Count, 0);
            Assert.IsFalse(actual.Success);
        }
    }
}