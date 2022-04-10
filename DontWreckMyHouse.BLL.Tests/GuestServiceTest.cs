
using DontWreckMyHouse.BLL.Tests.TestDoubles;
using DontWreckMyHouse.Core.Models;
using DontWreckMyHouse.DAL.Tests;
using NUnit.Framework;

namespace DontWreckMyHouse.BLL.Tests
{
    public class GuestServiceTest
    {
        GuestService guestService = new GuestService(new GuestRepositoryDouble());
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
            Result<Guest> actual = guestService.FindByEmail("NonexistentEmail@yahoo.com");

            Assert.AreEqual(expected, actual.Data);
            Assert.IsFalse(actual.Success);
        }
    }
}
