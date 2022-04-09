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

        public Result<List<Reservation>> GetReservationsByHostID(string HostID)
        {
            Result<List<Reservation>> result = new Result<List<Reservation>>();
            result.Data = new List<Reservation>();

            string filePath = GetFilePath(HostID);

            if (!File.Exists(filePath))
            {
                result.Success = false;
                result.Message = "Host does not have any reservations.";
            }

            else
            {
                try
                {
                    using(StreamReader sr = new StreamReader(filePath))
                    {
                        string currentLine=sr.ReadLine();
                        if (currentLine != null)
                        {
                            currentLine = sr.ReadLine();
                        }
                        while(currentLine != null)
                        {
                            Reservation reservation = Deserialize(currentLine.Trim());
                            result.Data.Add(reservation);
                            currentLine = sr.ReadLine();
                        }
                        result.Success = true;
                        result.Message = "Reservations were found";
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Could not read reservations.", ex);
                }
            }
            result.Data = result.Data.OrderBy(reservation => reservation.StartDate).ToList();

            return result;
        }

        public Result<Reservation> Add(Reservation reservation, string hostID)
        {
            Result<Reservation> result = new Result<Reservation>();
            Result<List<Reservation>> all = GetReservationsByHostID(hostID);
            all.Data.Add(reservation);
            WriteToFile(all.Data, hostID);
            return result;
        }

        public Result<Reservation> Remove(Reservation reservation, string hostID)
        {
            Result<Reservation> result = new Result<Reservation>();
            Result<List<Reservation>> all = GetReservationsByHostID(hostID);
            all.Data.Remove(reservation);
            WriteToFile(all.Data, hostID);
            result.Success = true;
            result.Message = $"Reservation {reservation.ID} successfully deleted";
            return result;
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
            reservations = reservations.OrderBy(reservation => reservation.ID).ToList();
            string filePath = GetFilePath(hostID);
            try
            {
                if (!File.Exists(filePath))
                {
                    //referenced stack overflow
                    using (FileStream fs = File.Create(filePath)) { }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Could not create new file", ex);
            }
            try
            {
                using StreamWriter streamWriter = new StreamWriter(filePath);
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
