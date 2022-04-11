using DontWreckMyHouse.Core.Interfaces;
using DontWreckMyHouse.Core.Models;
using System.Linq;

namespace DontWreckMyHouse.DAL
{
    public class HostRepository : IHostRepository
    {
        private string _Path;

        public HostRepository(string path)
        {
            _Path = path;
        }

        public Host FindByID(string hostID)
        {
            Result<List<Host>> all = GetAll();
            if(all.Success == false)
            {
                return null;
            }

            return all.Data.FirstOrDefault(host => host.ID == hostID);
        }

        public Result<List<Host>> GetAll()
        {
            Result<List<Host>> result = new Result<List<Host>>();
            result.Data = new List<Host>();

            if (!File.Exists(_Path))
            {
                result.Success = false;
                return result;
            }

            try
            {
                using (StreamReader sr = new StreamReader(_Path))
                {
                    string currentLine = sr.ReadLine();
                    if (currentLine != null)
                    {
                        currentLine = sr.ReadLine();
                    }
                    while (currentLine != null)
                    {
                        Host record = Deserialize(currentLine.Trim());
                        result.Data.Add(record);
                        currentLine = sr.ReadLine();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Could not read Hosts", ex);
            }

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