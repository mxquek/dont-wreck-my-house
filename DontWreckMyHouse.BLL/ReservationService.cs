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
        public void GetReservationsByHostID(string hostID, Result<List<Reservation>> result)
        {
            ReservationRepository.GetReservationsByHostID(hostID, result);
            if(result.Data == null)
            {
                result.Success = false;
                result.Message = "List was not created.";
                return;
            }
            if(result.Data.Count <= 0)
            {
                result.Success = false;
                result.Message = "No reservations found for the host.";
            }
            return;
        }

        public Reservation Make(Host host, Guest guest, DateTime startDate, DateTime endDate, int oldReservationID = 0)
        {
            Reservation reservation = new Reservation();
            Validate(host, guest, startDate, endDate, oldReservationID);
            if (oldReservationID == 0)
            {
                reservation.ID = GetNextReservationID(host.ID);
            }
            else
            {
                reservation.ID = oldReservationID;
            }
            reservation.StartDate = startDate;
            reservation.EndDate = endDate;
            reservation.GuestID = guest.ID;
            reservation.Total = CalculateTotal(host, startDate, endDate);
            return reservation;
        }

        private int GetNextReservationID(string hostID)
        {
            Result<List<Reservation>> reservations = new Result<List<Reservation>>();
            ReservationRepository.GetReservationsByHostID(hostID, reservations);
            return reservations.Data.OrderBy(r => r.ID).Last().ID + 1;
        }

        private decimal CalculateTotal(Host host, DateTime startDate, DateTime endDate)
        {
            decimal total = 0;
            DateTime d = startDate;
            for (; d <= endDate; d = d.AddDays(1))
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

        private Result<Reservation> Validate(Host host, Guest guest, DateTime startDate, DateTime endDate, int reservationID = 0)
        {
            Result<Reservation> result = new Result<Reservation>();
            ValidateNulls(host, guest, startDate, endDate, result);
            ValidateReservationPeriod(host.ID,startDate,endDate,result,reservationID);

            return result;
        }

        private void ValidateNulls(Host host, Guest guest, DateTime startDate, DateTime endDate, Result<Reservation> result)
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

        private void ValidateReservationPeriod(string hostID, DateTime startDate, DateTime endDate, Result<Reservation> result, int reservationID = 0)
        {
            Result<List<Reservation>> reservations = new Result<List<Reservation>>();
            GetReservationsByHostID(hostID, reservations);
            if (reservations.Data.Any(r => ((startDate >= r.StartDate
                                        && startDate <= r.EndDate)
                                        || (endDate >= r.StartDate
                                        && endDate <= r.EndDate))
                                        && r.ID != reservationID))
            {
                result.Success = false;
                result.Message = "Reservation period overlaps with existing reservation. Dates must be during available dates.";
            }
            if(endDate < startDate)
            {
                result.Success = false;
                result.Message = "End date must be after start date.";
            }
        }

        public Result<Reservation> Add(Reservation reservation, string hostID)
        {
            Result<Reservation> result = ReservationRepository.Add(reservation, hostID);
            return result;
        }

        public Result<Reservation> Remove(Reservation reservation, string hostID)
        {
            Result<Reservation> result = ReservationRepository.Remove(reservation, hostID);
            return result;
        }

        public void Edit(Result<Reservation> updatedReservation,string hostID)
        {
            ReservationRepository.Edit(updatedReservation, hostID);

            return;
        }
    }
}
