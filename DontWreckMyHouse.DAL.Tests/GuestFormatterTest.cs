using DontWreckMyHouse.Core.Models;
using DontWreckMyHouse.DAL.Formatters;
using NUnit.Framework;

namespace DontWreckMyHouse.DAL.Tests
{
    public class GuestFormatterTest
    {
        GuestFormatter guestFormatter = new GuestFormatter();

        [Test]
        public void Deserialize_StringGuest_ReturnsGuest()
        {
            Guest expected = new Guest(1, "John", "Smith", "JohnSmith@gmail.com", "(727) 123-4567", "TX");
            string stringGuest = "1,John,Smith,JohnSmith@gmail.com,(727) 123-4567,TX";
            Guest actual = guestFormatter.Deserialize(stringGuest);

            Assert.AreEqual(expected, actual);
        }
    }
}
