
using DontWreckMyHouse.BLL.Tests.TestDoubles;
using DontWreckMyHouse.Core.Models;
using DontWreckMyHouse.DAL.Tests;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace DontWreckMyHouse.BLL.Tests
{
    public class GuestServiceTest
    {
        GuestService guestService = new GuestService(new GuestRepositoryDouble());

        [Test]
        public void FindByID_ExistingGuestID_ReturnsGuest()
        {
            Guest expected = new Guest(GuestRepositoryTest.GUEST1);
            Result<Guest> actual = guestService.FindByID(GuestRepositoryTest.GUEST1.ID);

            Assert.AreEqual(expected, actual.Data);
            Assert.IsTrue(actual.Success);
        }
        [Test]
        public void FindByID_NonexistentGuestID_ReturnsNull()
        {
            Guest expected = null;
            Result<Guest> actual = guestService.FindByID(0);

            Assert.AreEqual(expected, actual.Data);
            Assert.IsFalse(actual.Success);
        }

        [Test]
        public void FindByEmail_ExistingGuestEmail_ReturnsGuest()
        {
            Guest expected = new Guest(GuestRepositoryTest.GUEST1);
            Result<Guest> actual = guestService.FindByEmail(GuestRepositoryTest.GUEST1.Email);

            Assert.AreEqual(expected, actual.Data);
            Assert.IsTrue(actual.Success);
        }
        [Test]
        public void FindByEmail_NonexistentGuestEmail_ReturnsNull()
        {
            Guest expected = null;
            Result<Guest> actual = guestService.FindByEmail("NonexistentGuestEmail@yahoo.com");

            Assert.AreEqual(expected, actual.Data);
            Assert.IsFalse(actual.Success);
        }

        [Test]
        public void FindByLastName_ExistingGuestLastName_ReturnsHost()
        {
            Guest expected = new Guest(GuestRepositoryTest.GUEST1);
            Result<List<Guest>> actual = new Result<List<Guest>>();
            actual = guestService.FindByLastName(GuestRepositoryTest.GUEST1.LastName);

            Assert.AreEqual(actual.Data.Count, 1);
            Assert.IsTrue(actual.Data.Any(guest => guest.Equals(expected)));
            Assert.IsTrue(actual.Success);
        }
        [Test]
        public void FindByLastName_NonexistentGuestLastName_ReturnsNoHosts()
        {
            Result<List<Guest>> actual = new Result<List<Guest>>();
            actual = guestService.FindByLastName("NonexistentGuestLastName");

            Assert.AreEqual(actual.Data.Count, 0);
            Assert.IsFalse(actual.Success);
        }
    }
}
