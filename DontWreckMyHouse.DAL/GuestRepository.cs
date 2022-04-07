using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DontWreckMyHouse.Core.Interfaces;
using DontWreckMyHouse.Core.Models;
using DontWreckMyHouse.DAL.Formatters;

namespace DontWreckMyHouse.DAL
{
    public class GuestRepository : IGuestRepository
    {
        private string _Path;
        private GuestFormatter _GuestFormatter;

        public GuestRepository(string path, GuestFormatter guestFormatter)
        {
            _Path = path;
            _GuestFormatter = guestFormatter;
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
                        Guest record = _GuestFormatter.Deserialize(currentLine.Trim());
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
    }
}
