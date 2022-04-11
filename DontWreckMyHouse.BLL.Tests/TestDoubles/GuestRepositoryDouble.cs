using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DontWreckMyHouse.Core.Interfaces;
using DontWreckMyHouse.Core.Models;
using DontWreckMyHouse.DAL.Tests;

namespace DontWreckMyHouse.BLL.Tests.TestDoubles
{
    public class GuestRepositoryDouble : IGuestRepository
    {

        public GuestRepositoryDouble()
        {
        }

        public Guest FindByID(int guestID)
        {
            Result<List<Guest>> all = GetAll();
            if (all.Success == false)
            {
                return null;
            }

            return all.Data.FirstOrDefault(guest => guest.ID == guestID);
        }
        public Result<List<Guest>> GetAll()
        {
            Result<List<Guest>> result = new Result<List<Guest>>();
            result.Data = new List<Guest>();

            result.Data.Add(new Guest(1, "John", "Smith", "JohnSmith@gmail.com", "(333) 333-3333", "TX"));
            result.Data.Add(new Guest(2, "Terry", "Bob", "TBob@yahoo.com", "(444) 444-4444", "NV"));

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
