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
            if(result.Data == null)
            {
                return result;
            }
            if(result.Data.Count <= 0)
            {
                result.Success = false;
                result.Message = "No reservations found for the host.";
            }
            return result;
        }

        public Reservation Make(Host host, Guest guest, DateTime startDate, DateTime endDate)
        {
            Reservation reservation = new Reservation();
            Validate(host, guest, startDate, endDate);
            reservation.Total = CalculateTotal(host, startDate, endDate);
            return reservation;
        }

        private decimal CalculateTotal(Host host, DateTime startDate, DateTime endDate)
        {
            decimal total = 0;

            for (DateTime d = startDate; d <= endDate; d.AddDays(1))
            {
                if (d.DayOfWeek == DayOfWeek.Saturday || d.DayOfWeek == DayOfWeek.Sunday)
                {
                    total += host.WeekendRate;
                }
                else
                {
                    total += host.StandardRate;
                }
            }
            return total;
        }

        private Result<string> Validate(Host host, Guest guest, DateTime startDate, DateTime endDate)
        {
            Result<string> result = new Result<string>();
            ValidateNulls(host, guest, startDate, endDate, result);
            ValidateReservationPeriod(host,startDate,endDate,result);

            return result;
        }

        private void ValidateNulls(Host host, Guest guest, DateTime startDate, DateTime endDate, Result<string> result)
        {
            if(host == null)
            {
                result.Message = "Must have a host";
                result.Success = false;
            }
            if (guest == null)
            {
                result.Message = "Must have a guest";
                result.Success = false;
            }
            if (startDate == null)
            {
                result.Message = "Must have a start date";
                result.Success = false;
            }
            if (endDate == null)
            {
                result.Message = "Must have an end date";
                result.Success = false;
            }
        }

        private void ValidateReservationPeriod(Host host, DateTime startDate, DateTime endDate, Result<string> result)
        {
            Result < List < Reservation >> reservations = GetReservationsByHostID(host.ID);
            if (reservations.Data.Any(r => (startDate >= r.StartDate
                                        && startDate <= r.EndDate)
                                        || (endDate >= r.StartDate
                                        && endDate <= r.EndDate)))
            {
                result.Success = false;
                result.Message = "Reservation period overlaps with existing reservation. Dates must be during available dates.";
            }
        }
    }
}
