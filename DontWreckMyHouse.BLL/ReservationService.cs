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
            reservation.ID = GetNextReservationID(host.ID);
            reservation.StartDate = startDate;
            reservation.EndDate = endDate;
            reservation.GuestID = guest.ID;
            reservation.Total = CalculateTotal(host, startDate, endDate);
            return reservation;
        }

        private int GetNextReservationID(string hostID)
        {
            Result<List<Reservation>> reservations = ReservationRepository.GetReservationsByHostID(hostID);
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

        private Result<Reservation> Validate(Host host, Guest guest, DateTime startDate, DateTime endDate)
        {
            Result<Reservation> result = new Result<Reservation>();
            ValidateNulls(host, guest, startDate, endDate, result);
            ValidateReservationPeriod(host.ID,startDate,endDate,result);

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

        private void ValidateReservationPeriod(string hostID, DateTime startDate, DateTime endDate, Result<Reservation> result)
        {
            Result < List < Reservation >> reservations = GetReservationsByHostID(hostID);
            if (reservations.Data.Any(r => (startDate >= r.StartDate
                                        && startDate <= r.EndDate)
                                        || (endDate >= r.StartDate
                                        && endDate <= r.EndDate)))
            {
                result.Success = false;
                result.Message = "Reservation period overlaps with existing reservation. Dates must be during available dates.";
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

        public Result<Reservation> Edit(Reservation oldReservation, string hostID, DateTime? newStartDate, DateTime? newEndDate)
        {
            Result<Reservation> result = new Result<Reservation>();
            result.Success = true;
            result.Message = "Default";

            Reservation updatedReservation = new Reservation(oldReservation);
            
            if(newStartDate != null)
            {
                updatedReservation.StartDate = (DateTime)newStartDate;
            }
            if(newEndDate != null)
            {
                updatedReservation.EndDate = (DateTime)newEndDate;
            }

            ValidateReservationPeriod(hostID, updatedReservation.StartDate, updatedReservation.EndDate, result);
            if(result.Success == false)
            {
                return result;
            }
            else
            {
                ReservationRepository.Edit(updatedReservation, hostID);
            }
            throw new NotImplementedException();
        }
    }
}
