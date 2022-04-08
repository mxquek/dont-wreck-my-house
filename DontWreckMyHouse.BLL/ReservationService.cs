using DontWreckMyHouse.Core.Interfaces;
using DontWreckMyHouse.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DontWreckMyHouse.BLL
{
    public class ReservationService
    {
        public IReservationRepository ReservationRepository;
        public ReservationService(IReservationRepository repo)
        {
            ReservationRepository = repo;
        }
        public Result<List<Reservation>> GetReservationsByHostID(string hostID)
        {
            Result<List<Reservation>> result = ReservationRepository.GetReservationsByHostID(hostID);
            if(result.Data.Count <= 0)
            {
                result.Success = false;
                result.Message = "No reservations found for the host.";
            }
            return result;
        }

    }
}
