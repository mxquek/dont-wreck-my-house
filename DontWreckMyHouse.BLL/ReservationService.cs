using DontWreckMyHouse.Core.Interfaces;
using DontWreckMyHouse.Core.Models;
using System.Linq;

namespace DontWreckMyHouse.BLL
{
    public class ReservationService
    {
        public IHostRepository HostRepository;
        public IGuestRepository GuestRepository;
        public IReservationRepository ReservationRepository;
        
        public ReservationService(IHostRepository hostRepository, IGuestRepository guestRepository, IReservationRepository reservationRepo)
        {
            HostRepository = hostRepository;
            GuestRepository = guestRepository;
            ReservationRepository = reservationRepo;
        }

        //Main Functions
        public void GetReservationsByHostID(Result<List<Reservation>> result, string hostID)
        {
            if (HostRepository.FindByID(hostID) == null)
            {
                result.Success = false;
                result.Message = "Requested host does not exist";
            }

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
        public void Add(Result<Reservation> reservation, Host host)
        {
            ValidateReservation(reservation, host);
            if(reservation.Success == false)
            {
                return;
            }

            ReservationRepository.Add(reservation, host.ID);
            return;
        }
        public void Remove(Result<Reservation> reservation, Host host)
        {
            ValidateReservation(reservation, host);
            if (reservation.Success == false)
            {
                return;
            }

            ReservationRepository.Remove(reservation, host.ID);
            return;
        }
        public void Edit(Result<Reservation> updatedReservation, Host host)
        {
            ValidateReservation(updatedReservation, host);
            if (updatedReservation.Success == false)
            {
                return;
            }

            ReservationRepository.Edit(updatedReservation, host.ID);
            return;
        }

        //Supporting Methods
        public int GetNextReservationID(string hostID, int existingReservationID = 0)
        {
            Result<List<Reservation>> reservations = new Result<List<Reservation>>();
            reservations.Data = new List<Reservation>();
            ReservationRepository.GetReservationsByHostID(hostID, reservations);

            if (existingReservationID != 0)
            {
                return existingReservationID;
            }

            //If host has no reservations, return reservation ID of 1
            if (reservations.Data.Count == 0)
            {
                return 1;
            }
            return reservations.Data.OrderBy(r => r.ID).Last().ID + 1;
        }
        public decimal CalculateTotal(Host host, DateTime startDate, DateTime endDate)
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
        public void ValidateReservation (Result<Reservation> result, Host host)
        {
            result.Success = true;
            ValidateNulls(result, host);
            if(!result.Success) {return;}
            ValidateHost(result, host);
            if (!result.Success) {return;}
            ValidateGuestID(result);
            if (!result.Success) {return;}
            ValidateReservationDates(result,host.ID);
        }

        private void ValidateNulls(Result<Reservation> result, Host host)
        {
            if(host == null)
            {
                result.Success = false;
                result.Message = "Host cannot be empty";
            }
            if(result.Data == null)
            {
                result.Success = false;
                result.Message = "Reservation cannot be empty";
            }
        }
        private void ValidateGuestID(Result<Reservation> result)
        {
            if(result.Data.GuestID <= 0)
            {
                result.Success = false;
                result.Message = "Invalid GuestID";
                return;
            }
            if (GuestRepository.FindByID(result.Data.GuestID) == null)
            {
                result.Success = false;
                result.Message = "Guest does not exist";
            }
            else
            {
                return;
            }
        }
        private void ValidateHost(Result<Reservation> result, Host host)
        {
            if (HostRepository.FindByID(host.ID) == null)
            {
                result.Success = false;
                result.Message = "Host does not exist";
            }
            else
            {
                return;
            }
        }
        private void ValidateReservationDates(Result<Reservation> result, string hostID)
        {
            Result<List<Reservation>> reservations = new Result<List<Reservation>>();
            reservations.Data = new List<Reservation>();
            GetReservationsByHostID(reservations, hostID);

            if (reservations.Data.Any(r => ((result.Data.StartDate >= r.StartDate
                                        && result.Data.StartDate <= r.EndDate)
                                        || (result.Data.EndDate >= r.StartDate
                                        && result.Data.EndDate <= r.EndDate))
                                        && r.ID != result.Data.ID))
            {
                result.Success = false;
                result.Message = "Reservation period overlaps with existing reservation. Dates must be during available dates.";
                return;
            }
            if (result.Data.EndDate < result.Data.EndDate)
            {
                result.Success = false;
                result.Message = "End date must be after start date.";
                return;
            }
            result.Success = true;
        }

    }
}
