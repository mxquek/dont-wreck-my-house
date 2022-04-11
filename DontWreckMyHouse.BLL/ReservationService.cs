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
                return;
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
            if(reservation.Success == false) {return;}

            ReservationRepository.Add(reservation, host.ID);
            return;
        }
        public void Remove(Result<Reservation> reservation, Host host)
        {
            ValidateReservation(reservation, host);
            if (reservation.Success == false) { return; }

            Result<List<Reservation>> all = new Result<List<Reservation>>();
            all.Data = new List<Reservation>();

            ReservationRepository.GetReservationsByHostID(host.ID, all);
            if (all.Success == false)
            {
                reservation.Success = false;
                return;
            }

            if (all.Data.Where(r => r.Equals(reservation.Data)).ToList().Count == 0)
            {
                reservation.Success = false;
                reservation.Message = $"Reservation ID {reservation.Data.ID} was not found. Exiting...";
                return;
            }
            else { ReservationRepository.Remove(reservation, host.ID);}
            return;
        }
        public void Edit(Result<Reservation> updatedReservation, Host host)
        {
            ValidateReservation(updatedReservation, host);
            if (updatedReservation.Success == false) { return; }

            Result<List<Reservation>> all = new Result<List<Reservation>>();
            all.Data = new List<Reservation>();

            ReservationRepository.GetReservationsByHostID(host.ID, all);
            if (all.Success == false)
            {
                updatedReservation.Success = false;
                return;
            }
            
            int targetIndex = all.Data.IndexOf(all.Data.Where(r => r.ID == updatedReservation.Data.ID).FirstOrDefault());

            if (targetIndex == -1)
            {
                updatedReservation.Success = false;
                updatedReservation.Message = $"Reservation {updatedReservation.Data.ID} not found.";
                return;
            }
            else { ReservationRepository.Edit(updatedReservation, host.ID, targetIndex); }
            return;
        }

        //Supporting Methods
        public int GetNextReservationID(string hostID, int existingReservationID = 0)
        {
            Result<List<Reservation>> reservations = new Result<List<Reservation>>();
            reservations.Data = new List<Reservation>();
            ReservationRepository.GetReservationsByHostID(hostID, reservations);

            if (existingReservationID != 0) {return existingReservationID;}

            //If host has no reservations, return reservation ID of 1
            if (reservations.Data.Count == 0) {return 1;}

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
            if (result.Data.EndDate < result.Data.StartDate)
            {
                result.Success = false;
                result.Message = "End date must be after start date.";
                return;
            }
            result.Success = true;
        }

    }
}
