using System;
using System.Collections.Generic;
using DontWreckMyHouse.Core.Interfaces;
using DontWreckMyHouse.Core.Models;

namespace DontWreckMyHouse.BLL.Tests.TestDoubles
{
    public class GuestRepositoryDouble : IGuestRepository
    {
        public Result<List<Guest>> GetAll()
        {
            throw new NotImplementedException();
        }
    }
}
