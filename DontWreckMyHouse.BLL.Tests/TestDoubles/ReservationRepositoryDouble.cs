using System;
using System.Collections.Generic;
using System.IO;
using DontWreckMyHouse.Core.Interfaces;
using DontWreckMyHouse.Core.Models;

namespace DontWreckMyHouse.BLL.Tests.TestDoubles
{
    public class ReservationRepositoryDouble : IReservationRepository
    {
        static string ProjectDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.Parent.FullName;
        static string DAL_TEST_DIRECTORY = "DontWreckMyHouse.DAL.Tests";
        static string DATA_DIRECTORY = Path.Combine(ProjectDirectory, DAL_TEST_DIRECTORY, "data");

        static string SEED_DIRECTORY = "seed";
        static string SEED_DATA_DIRECTORY = "testReservationSeed";

        static string TEST_DIRECTORY = "test";
        static string TEST_DATA_DIRECTORY = "testReservations";

        string Seed_Path = Path.Combine(DATA_DIRECTORY, SEED_DIRECTORY, SEED_DATA_DIRECTORY);
        string Test_Path = Path.Combine(DATA_DIRECTORY, TEST_DIRECTORY, TEST_DATA_DIRECTORY);

        public ReservationRepositoryDouble()
        {
            if (!Directory.Exists(Test_Path))
            {
                Directory.CreateDirectory(Test_Path);
            }

            //Deletes all existing test files
            foreach (var testFile in Directory.GetFiles(Test_Path))
            {
                File.Delete(testFile);
            }

            //Clean copy of all seed files
            foreach (var srcPath in Directory.GetFiles(Seed_Path))
            {
                File.Copy(srcPath, srcPath.Replace(Seed_Path, Test_Path), true);
            }
        }

        public void Add(Result<Reservation> reservation, string hostID)
        {
            throw new NotImplementedException();
        }

        public void Edit(Result<Reservation> updatedReservation, string hostID)
        {
            throw new NotImplementedException();
        }

        public void GetReservationsByHostID(string HostID, Result<List<Reservation>> result)
        {
            throw new NotImplementedException();
        }

        public void Remove(Result<Reservation> reservation, string hostID)
        {
            throw new NotImplementedException();
        }
    }
}
