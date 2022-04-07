using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DontWreckMyHouse.Core.Interfaces;
using DontWreckMyHouse.Core.Models;

namespace DontWreckMyHouse.DAL
{
    public class ReservationRepository : IReservationRepository
    {
        public Reservation Deserialize(string data)
        {
            Reservation result = new Reservation();
            string[] fields = data.Split(",");
            result.ID = int.Parse(fields[0]);
            result.StartDate = DateTime.Parse(fields[1]);
            result.EndDate = DateTime.Parse(fields[2]);
            result.Guest.ID = int.Parse(fields[3]);
            //result.Total = decimal.Parse(fields[4]);

            return result;
        }

        public string Serialize(Reservation reservation)
        {
            return $"{reservation.ID},{reservation.StartDate:yyyy-MM-dd},{reservation.EndDate:yyyy-MM-dd},{reservation.Guest.ID},{reservation.Total}";
        }
    }
}
