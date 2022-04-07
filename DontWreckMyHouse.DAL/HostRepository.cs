using DontWreckMyHouse.Core.Interfaces;
using DontWreckMyHouse.Core.Models;

namespace DontWreckMyHouse.DAL
{
    public class HostRepository : IHostRepository
    {
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