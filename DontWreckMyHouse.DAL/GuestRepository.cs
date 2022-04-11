using DontWreckMyHouse.Core.Interfaces;
using DontWreckMyHouse.Core.Models;

namespace DontWreckMyHouse.DAL
{
    public class GuestRepository : IGuestRepository
    {
        private string _Path;

        public GuestRepository(string path)
        {
            _Path = path;
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
                        Guest record = Deserialize(currentLine.Trim());
                        result.Data.Add(record);
                        currentLine = sr.ReadLine();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Could not read guests",ex);
            }

            result.Success = true;
            return result;
        }

        public Guest Deserialize(string data)
        {
            Guest result = new Guest();

            string[] fields = data.Split(",");
            if (fields.Length != 6)
            {
                return null;
            }

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
