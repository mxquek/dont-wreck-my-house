using DontWreckMyHouse.Core.Interfaces;
using DontWreckMyHouse.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DontWreckMyHouse.BLL
{
    public class GuestService
    {
        public IGuestRepository GuestRepository;
        public GuestService(IGuestRepository repo)
        {
            GuestRepository = repo;
        }
        public Result<Guest> FindByID(int guestID)
        {
            Result<List<Guest>> guests = GuestRepository.GetAll();
            Result<Guest> result = new Result<Guest>();
            result.Data = guests.Data.Where(guest => guest.ID == guestID).FirstOrDefault();
            if(result.Data == null)
            {
                result.Success = false;
                result.Message = $"No guest found with ID: {guestID}.";
            }
            else
            {
                result.Success = true;
                //result.Message = $"Guest ID: {guestID} found.";
            }
            return result;
        }
    }
}
