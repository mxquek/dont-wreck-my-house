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

        //Main Functions
        public void GetReservationsByHostID(string HostID, Result<List<Reservation>> result)
        {
            string filePath = GetFilePath(HostID);

            if (!File.Exists(filePath))
            {
                result.Success = false;
                result.Message = "Host file does not exist.";
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

            return;
        }
        public void Add(Result<Reservation> reservation, string hostID)
        {
            Result<List<Reservation>> all = new Result<List<Reservation>>();
            all.Data = new List<Reservation>();

            GetReservationsByHostID(hostID, all);
            //Don't check for success, as an empty list is acceptable

            all.Data.Add(reservation.Data);
            WriteToFile(all.Data, hostID);

            reservation.Success = true;
            reservation.Message = $"Reservation {reservation.Data.ID} added.";
            return;
        }
        public void Remove(Result<Reservation> reservation, string hostID)
        {
            Result<List<Reservation>> all = new Result<List<Reservation>>();
            all.Data = new List<Reservation>();
            GetReservationsByHostID(hostID, all);

            all.Data.Remove(reservation.Data);
            WriteToFile(all.Data, hostID);

            reservation.Success = true;
            reservation.Message = $"Reservation {reservation.Data.ID} successfully deleted.";
            
            return;
        }
        public void Edit(Result<Reservation> updatedReservation, string hostID, int targetIndex)
        {
            Result<List<Reservation>> all = new Result<List<Reservation>>();
            all.Data = new List<Reservation>();

            GetReservationsByHostID(hostID, all);

            all.Data[targetIndex].StartDate = updatedReservation.Data.StartDate;
            all.Data[targetIndex].EndDate = updatedReservation.Data.EndDate;
            all.Data[targetIndex].Total = updatedReservation.Data.Total;

            updatedReservation.Success = true;
            updatedReservation.Message = $"Reservation {updatedReservation.Data.ID} updated.";

            WriteToFile(all.Data, hostID);
            return;
        }

        //File IO Functions
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
        
        public string GetFilePath(string hostID)
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
