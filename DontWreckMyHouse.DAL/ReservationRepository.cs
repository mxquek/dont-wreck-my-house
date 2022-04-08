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
        private string _Path;
        private const string HEADER = "id,start_date,end_date,guest_id,total";

        public ReservationRepository(string directory)
        {
            _Path = directory;
        }

        public Reservation Deserialize(string data)
        {
            Reservation result = new Reservation();
            string[] fields = data.Split(",");
            result.ID = int.Parse(fields[0]);
            result.StartDate = DateTime.Parse(fields[1]);
            result.EndDate = DateTime.Parse(fields[2]);
            result.GuestID = int.Parse(fields[3]);
            result.Total = decimal.Parse(fields[4]);

            return result;
        }

        public string Serialize(Reservation reservation)
        {
            return $"{reservation.ID},{reservation.StartDate:yyyy-MM-dd},{reservation.EndDate:yyyy-MM-dd},{reservation.GuestID},{reservation.Total}";
        }
        private string GetFilePath(string hostID)
        {
            return Path.Combine(_Path, $"{hostID}.csv");
        }

        public void WriteToFile(List<Reservation> reservations, string hostID)
        {
            try
            {
                using StreamWriter streamWriter = new StreamWriter(GetFilePath(hostID));
                streamWriter.WriteLine(HEADER);

                if(reservations == null)
                {
                    return;
                }
                foreach(Reservation reservation in reservations)
                {
                    streamWriter.WriteLine(Serialize(reservation));
                }
            }
            catch(IOException ex)
            {
                throw new Exception("Could not write reservations to file", ex);
            }
        }
    }
}
