using DontWreckMyHouse.Core.Interfaces;
using DontWreckMyHouse.Core.Models;

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
                result.Message = $"Guest ID {guestID}: {result.Data.FirstName} {result.Data.LastName} found.";
            }
            return result;
        }
        public Result<Guest> FindByEmail(string email)
        {
            Result<List<Guest>> guests = GuestRepository.GetAll();
            Result<Guest> result = new Result<Guest>();

            result.Data = guests.Data.Where(guest => guest.Email == email).FirstOrDefault();
            if(result == null)
            {
                result.Success = false;
                result.Message = $"No guest found with email: {email}.";
            }
            else
            {
                result.Success = true;
            }

            return result;
        }
        public Result<List<Guest>> FindByLastName(string prefix)
        {
            Result<List<Guest>> result = new Result<List<Guest>>();

            result.Data = GuestRepository.GetAll().Data
                          .Where(guest => guest.LastName.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                          .ToList();

            return result;
        }
    }
}
