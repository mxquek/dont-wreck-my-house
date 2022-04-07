using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DontWreckMyHouse.Core.Interfaces;
using DontWreckMyHouse.Core.Models;

namespace DontWreckMyHouse.BLL.Tests.TestDoubles
{
    public class HostRepositoryDouble : IHostRepository
    {
        public Result<List<Host>> GetAll()
        {
            throw new NotImplementedException();
        }
    }
}
