using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DontWreckMyHouse.Core.Interfaces;
using DontWreckMyHouse.Core.Models;

namespace DontWreckMyHouse.DAL.Formatters
{
    public class GuestFormatter : IFormatter<Guest>
    {
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

        //Serialize should NOT be used
        public string Serialize(Guest data)
        {
            throw new NotImplementedException();
        }
    }
}
