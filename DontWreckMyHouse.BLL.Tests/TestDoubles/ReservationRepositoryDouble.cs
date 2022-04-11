using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DontWreckMyHouse.Core.Interfaces;
using DontWreckMyHouse.Core.Models;

namespace DontWreckMyHouse.BLL.Tests.TestDoubles
{
    public class ReservationRepositoryDouble : IReservationRepository
    {

        public ReservationRepositoryDouble() 
        
        {
        
        }

        public void GetReservationsByHostID(string HostID, Result<List<Reservation>> result)
        {
            result.Data = new List<Reservation>();

            //H1R1 exists in Seed
            DateTime H1R1StartDate = new DateTime(2022, 11, 11);
            DateTime H1R1EndDate = new DateTime(2022, 11, 12);

            if (HostID == "GUID-1111")
            {
                result.Data.Add(new Reservation(1, H1R1StartDate, H1R1EndDate, 1, 75));
                result.Success = true;
            }
            else { return; }

            result.Data = result.Data.OrderBy(reservation => reservation.StartDate).ToList();
            return;
        }

        public void Add(Result<Reservation> reservation, string hostID)
        {
            Result<List<Reservation>> all = new Result<List<Reservation>>();
            all.Data = new List<Reservation>();

            GetReservationsByHostID(hostID, all);

            all.Data.Add(reservation.Data);

            reservation.Success = true;
            reservation.Message = $"Reservation {reservation.Data.ID} added.";
            return;
        }
        public void Remove(Result<Reservation> reservation, string hostID)
        {
            Result<List<Reservation>> all = new Result<List<Reservation>>();
            all.Data = new List<Reservation>();
            GetReservationsByHostID(hostID, all);

            all.Data.Remove(reservation.Data);

            reservation.Success = true;
            reservation.Message = $"Reservation {reservation.Data.ID} successfully deleted.";

            return;
        }
        public void Edit(Result<Reservation> updatedReservation, string hostID, int targetIndex)
        {
            Result<List<Reservation>> all = new Result<List<Reservation>>();
            all.Data = new List<Reservation>();

            GetReservationsByHostID(hostID, all);

            all.Data[targetIndex].StartDate = updatedReservation.Data.StartDate;
            all.Data[targetIndex].EndDate = updatedReservation.Data.EndDate;
            all.Data[targetIndex].Total = updatedReservation.Data.Total;

            updatedReservation.Success = true;
            updatedReservation.Message = $"Reservation {updatedReservation.Data.ID} updated.";

            return;
        }

    }
}
