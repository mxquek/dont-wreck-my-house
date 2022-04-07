using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DontWreckMyHouse.Core.Models
{
    public class Reservation
    {
        public int ID { get; set; }
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

        public Reservation() 
        {
        }
        public Reservation(int id, DateTime startDate, DateTime endDate, Guest guest, Host host)
        {
            ID = id;
            StartDate = startDate;
            EndDate = endDate;
            Guest = guest;
            Host = host;
        }

        public override bool Equals(object? obj)
        {
            return obj is Reservation reservation &&
                   ID == reservation.ID &&
                   StartDate == reservation.StartDate &&
                   EndDate == reservation.EndDate &&
                   Guest.Equals(reservation.Guest) &&
                   Host.Equals(reservation.Host) &&
                   Total == reservation.Total;
        }
        public override int GetHashCode()
        {
            return HashCode.Combine(ID, StartDate, EndDate, Guest.ID, Host.ID, Total);
        }
    }
}
