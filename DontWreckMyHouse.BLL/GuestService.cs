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
        public IGuestRepository guestRepository;
        public GuestService(IGuestRepository repo)
        {
            guestRepository = repo;
        }
        public Guest GetGuestByID(int guestID)
        {
            throw new NotImplementedException();
        }
    }
}
