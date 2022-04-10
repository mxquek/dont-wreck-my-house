using System;
using System.Collections.Generic;
using System.IO;
using DontWreckMyHouse.Core.Interfaces;
using DontWreckMyHouse.Core.Models;

namespace DontWreckMyHouse.BLL.Tests.TestDoubles
{
    public class GuestRepositoryDouble : IGuestRepository
    {
        static string ProjectDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.Parent.FullName;
        static string DAL_TEST_DIRECTORY = "DontWreckMyHouse.DAL.Tests";
        static string DATA_DIRECTORY = Path.Combine(ProjectDirectory, DAL_TEST_DIRECTORY, "data");

        const string SEED_DIRECTORY = "seed";
        const string SEED_FILE = "testGuestSeed.csv";

        const string TEST_DIRECTORY = "test";
        const string TEST_FILE = "testGuests.csv";

        string Seed_Path = Path.Combine(DATA_DIRECTORY, SEED_DIRECTORY, SEED_FILE);
        private string Test_Path = Path.Combine(DATA_DIRECTORY, TEST_DIRECTORY, TEST_FILE);
        public GuestRepositoryDouble()
        {
            if (!Directory.Exists(TEST_DIRECTORY))
            {
                Directory.CreateDirectory(Path.Combine(DATA_DIRECTORY, TEST_DIRECTORY));
            }
            File.Copy(Seed_Path, Test_Path, true);
        }

        public Result<List<Guest>> GetAll()
        {
            Result<List<Guest>> result = new Result<List<Guest>>();
            result.Data = new List<Guest>();

            if (!File.Exists(Test_Path))
            {
                result.Success = false;
                return result;
            }

            try
            {
                using (StreamReader sr = new StreamReader(Test_Path))
                {
                    string currentLine = sr.ReadLine();
                    if (currentLine != null)
                    {
                        currentLine = sr.ReadLine();
                    }
                    while (currentLine != null)
                    {
                        Guest record = Deserialize(currentLine.Trim());
                        result.Data.Add(record);
                        currentLine = sr.ReadLine();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Could not read guests", ex);
            }

            result.Success = true;
            return result;
        }

        public Guest Deserialize(string data)
        {
            Guest result = new Guest();

            string[] fields = data.Split(",");
            result.ID = int.Parse(fields[0]);
            result.FirstName = fields[1];
            result.LastName = fields[2];
            result.Email = fields[3];
            result.PhoneNumber = fields[4];
            result.State = fields[5];

            return result;
        }
    }
}
