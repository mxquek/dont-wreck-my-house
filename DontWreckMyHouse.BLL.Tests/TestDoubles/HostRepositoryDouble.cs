using System;
using System.Collections.Generic;
using System.IO;
using DontWreckMyHouse.Core.Interfaces;
using DontWreckMyHouse.Core.Models;

namespace DontWreckMyHouse.BLL.Tests.TestDoubles
{
    public class HostRepositoryDouble : IHostRepository
    {
        static string ProjectDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.Parent.FullName;
        static string DAL_TEST_DIRECTORY = "DontWreckMyHouse.DAL.Tests";
        static string DATA_DIRECTORY = Path.Combine(ProjectDirectory, DAL_TEST_DIRECTORY, "data");
        const string TEST_DIRECTORY = "test";
        const string TEST_FILE = "testHosts.csv";
        private string _Path = Path.Combine(DATA_DIRECTORY, TEST_DIRECTORY, TEST_FILE);

        const string SEED_DIRECTORY = "seed";
        const string SEED_FILE = "testHostSeed.csv";

        string Seed_Path = Path.Combine(DATA_DIRECTORY, SEED_DIRECTORY, SEED_FILE);

        public HostRepositoryDouble()
        {
            if (!Directory.Exists(TEST_DIRECTORY))
            {
                Directory.CreateDirectory(Path.Combine(DATA_DIRECTORY, TEST_DIRECTORY));
            }
            File.Copy(Seed_Path, _Path, true);
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
