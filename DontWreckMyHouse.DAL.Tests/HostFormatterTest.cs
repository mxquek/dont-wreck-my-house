using DontWreckMyHouse.Core.Models;
using DontWreckMyHouse.DAL.Formatters;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DontWreckMyHouse.DAL.Tests
{
    public class HostFormatterTest
    {
        HostFormatter hostFormatter = new HostFormatter();

        [Test]
        public void Deserialize_StringHost_ReturnsHost()
        {
            Host expected = new Host("GUID-####", "Doe", "JaneDoe@gmail.com", "(123) 123-4567", "1212 Everlane Rd", "Buffalo", "NY", "14201", 25, 50);
            string stringHost = "GUID-####,Doe,JaneDoe@gmail.com,(123) 123-4567,1212 Everlane Rd,Buffalo,NY,14201,25,50";
            Host actual = hostFormatter.Deserialize(stringHost);

            Assert.AreEqual(expected, actual);
        }
    }
}
