﻿using DontWreckMyHouse.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DontWreckMyHouse.Core.Interfaces
{
    public interface IReservationRepository
    {
        public Result<List<Reservation>> GetReservationsByHostID(string hostID);
        public Result<Reservation> Add(Reservation reservation, string hostID);
        public Result<Reservation> Remove(Reservation reservation, string hostID);
        public Result<Reservation> Edit(Reservation updatedReservation, string hostID);
    }
}
