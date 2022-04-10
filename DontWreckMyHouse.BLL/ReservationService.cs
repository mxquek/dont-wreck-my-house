using DontWreckMyHouse.Core.Interfaces;
using DontWreckMyHouse.Core.Models;

namespace DontWreckMyHouse.BLL
{
    public class ReservationService
    {
        public IReservationRepository ReservationRepository;
        
        public ReservationService(IReservationRepository repo)
        {
            ReservationRepository = repo;
        }

        //Main Functions
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
        public void Add(Result<Reservation> reservation, string hostID)
        {
            ReservationRepository.Add(reservation, hostID);
            return;
        }
        public void Remove(Result<Reservation> reservation, string hostID)
        {
            ReservationRepository.Remove(reservation, hostID);
            return;
        }
        public void Edit(Result<Reservation> updatedReservation, string hostID)
        {
            ReservationRepository.Edit(updatedReservation, hostID);

            return;
        }

        //Supporting Methods
        public void Make(Result<Reservation> result, Host host, Guest guest, DateTime startDate, DateTime endDate, int oldReservationID = 0)
        {
            Validate(result, host, guest, startDate, endDate, oldReservationID);
            if (result.Success == false)
            {
                return;
            }
            if (oldReservationID == 0)
            {
                result.Data.ID = GetNextReservationID(host.ID);
            }
            else
            {
                result.Data.ID = oldReservationID;
            }
            result.Data.StartDate = startDate;
            result.Data.EndDate = endDate;
            result.Data.GuestID = guest.ID;
            result.Data.Total = CalculateTotal(host, startDate, endDate);
            return;
        }
        private int GetNextReservationID(string hostID)
        {
            Result<List<Reservation>> reservations = new Result<List<Reservation>>();
            reservations.Data = new List<Reservation>();
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

        //Validation
        private void Validate(Result<Reservation> result, Host host, Guest guest, DateTime startDate, DateTime endDate, int reservationID = 0)
        {
            ValidateNulls(host, guest, startDate, endDate, result);
            ValidateReservationPeriod(host.ID,startDate,endDate,result,reservationID);

            return;
        }
        private void ValidateNulls(Host host, Guest guest, DateTime startDate, DateTime endDate, Result<Reservation> result)
        {
            if(host == null)
            {
                result.Message = "Must have a host";
                result.Success = false;
                return;
            }
            else if (guest == null)
            {
                result.Message = "Must have a guest";
                result.Success = false;
                return;
            }
            else if (startDate == null)
            {
                result.Message = "Must have a start date";
                result.Success = false;
                return;
            }
            else if (endDate == null)
            {
                result.Message = "Must have an end date";
                result.Success = false;
                return;
            }
            else
            {
                result.Success = true;
            }
        }
        private void ValidateReservationPeriod(string hostID, DateTime startDate, DateTime endDate, Result<Reservation> result, int reservationID = 0)
        {
            Result<List<Reservation>> reservations = new Result<List<Reservation>>();
            reservations.Data = new List<Reservation>();
            GetReservationsByHostID(hostID, reservations);
            if (reservations.Data.Any(r => ((startDate >= r.StartDate
                                        && startDate <= r.EndDate)
                                        || (endDate >= r.StartDate
                                        && endDate <= r.EndDate))
                                        && r.ID != reservationID))
            {
                result.Success = false;
                result.Message = "Reservation period overlaps with existing reservation. Dates must be during available dates.";
                return;
            }
            if(endDate < startDate)
            {
                result.Success = false;
                result.Message = "End date must be after start date.";
                return;
            }
            result.Success = true;
        }

        
    }
}
