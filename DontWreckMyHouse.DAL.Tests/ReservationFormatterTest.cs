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
    public class ReservationFormatterTest
    {
        ReservationFormatter reservationFormatter = new ReservationFormatter();
        Guest GUEST = new Guest(11,"Bob","Jones","BobJones@yahoo.com","(111) 111-1111","OH");
        Host HOST = new Host("GUID-####", "Doe", "JaneDoe@gmail.com", "(123) 123-4567", "1212 Everlane Rd", "Buffalo", "NY", "14201", 25, 50);

        //[Test]
        //public void Deserialize_StringReservation_ReturnsReservation()
        //{
        //    Reservation expected = new Reservation();
        //    expected.ID = 1;
        //    expected.StartDate = DateTime.Now.AddDays(-1).Date;
        //    expected.EndDate = DateTime.Now.Date;
        //    expected.Guest = GUEST;
        //    expected.Host = HOST;

        //    string stringReservation = $"{expected.ID},{expected.StartDate:yyyy-MM-dd},{expected.EndDate:yyyy-MM-dd},{expected.Guest.ID},{expected.Total}";
        //    Reservation actual = reservationFormatter.Deserialize(stringReservation);

        //    Assert.AreEqual(expected, actual);
        //}

        //[Test]
        //public void Serialize_Reservation_ReturnsStringReservation()
        //{
        //    Reservation reservation = new Reservation();
        //    reservation.ID = 1;
        //    reservation.StartDate = DateTime.Now.AddDays(-1).Date;
        //    reservation.EndDate = DateTime.Now.Date;
        //    reservation.Guest = new Guest();
        //    reservation.Guest = GUEST;
        //    reservation.Host = HOST;


        //    string expected = $"{reservation.ID},{reservation.StartDate:yyyy-MM-dd},{reservation.EndDate:yyyy-MM-dd},{reservation.Guest.ID},{reservation.Total}";
        //    //string actual = reservationFormatter.Serialize(reservation);

        //    //Assert.AreEqual(expected, actual);
        //    Assert.IsTrue(true);
        //}
    }
}
