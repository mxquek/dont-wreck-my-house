using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DontWreckMyHouse.Core.Interfaces;
using DontWreckMyHouse.Core.Models;

namespace DontWreckMyHouse.BLL.Tests.TestDoubles
{
    public class HostRepositoryDouble : IHostRepository
    {
        public HostRepositoryDouble() { }

        public Host FindByID(string hostID)
        {
            Result<List<Host>> all = GetAll();
            if (all.Success == false)
            {
                return null;
            }

            return all.Data.FirstOrDefault(host => host.ID == hostID);
        }
        public Result<List<Host>> GetAll()
        {
            Result<List<Host>> result = new Result<List<Host>>();
            result.Data = new List<Host>();

            result.Data.Add(new Host("GUID-1111", "Doe", "JaneDoe@gmail.com", "(111) 111-1111", "1212 Everlane Rd", "Buffalo", "NY", "14201", 25, 50));
            result.Data.Add(new Host("GUID-2222", "Well", "ChristinaWell@yahoo.com", "(222) 222-2222", "4444 Oceanside Ave", "Plano", "TX", "75252", 10, 20));

            result.Success = true;
            return result;
        }

        public Host Deserialize(string data)
        {
            Host result = new Host();
            string[] fields = data.Split(",");
            result.ID = fields[0];
            result.LastName = fields[1];
            result.Email = fields[2];
            result.PhoneNumber = fields[3];
            result.Address = fields[4];
            result.City = fields[5];
            result.State = fields[6];
            result.PostalCode = fields[7];
            result.StandardRate = decimal.Parse(fields[8]);
            result.WeekendRate = decimal.Parse(fields[9]);

            return result;
        }

    }
}
