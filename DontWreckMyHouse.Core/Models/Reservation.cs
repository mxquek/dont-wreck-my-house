﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DontWreckMyHouse.Core.Models
{
    public class Reservation
    {
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Guest Guest { get; set; }
        public Host Host { get; set; }
        public decimal Total
        { 
            get
            {
                if (Host == null)
                {
                    return 0;
                }
                decimal total = 0;

                for(DateTime d = StartDate; d <= EndDate; d.AddDays(1))
                {
                    if (d.DayOfWeek == DayOfWeek.Saturday|| d.DayOfWeek == DayOfWeek.Sunday)
                    {
                        total += Host.WeekendRate;
                    }
                    else
                    {
                        total += Host.StandardRate;
                    }
                }
                return total;
            }
        }

        public Reservation(int id, DateTime startDate, DateTime endDate, Guest guest, Host host)
        {
            Id = id;
            StartDate = startDate;
            EndDate = endDate;
            Guest = guest;
            Host = host;
        }
    }
}
