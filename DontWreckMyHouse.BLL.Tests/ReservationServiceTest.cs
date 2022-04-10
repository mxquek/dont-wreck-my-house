using DontWreckMyHouse.BLL.Tests.TestDoubles;
using DontWreckMyHouse.Core.Models;
using DontWreckMyHouse.DAL.Tests;
using NUnit.Framework;
using System.Collections.Generic;

namespace DontWreckMyHouse.BLL.Tests
{
    public class ReservationServiceTest
    {
        ReservationService reservationService = new ReservationService(new ReservationRepositoryDouble());
    }
}
