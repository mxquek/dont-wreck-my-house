
namespace DontWreckMyHouse.Core.Models
{
    public class Reservation
    {
        public int ID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int GuestID { get; set; }
        public decimal Total { get; set; }

        public Reservation() 
        {
        }
        public Reservation(Reservation existingReservation)
        {
            ID = existingReservation.ID;
            StartDate = existingReservation.StartDate;
            EndDate = existingReservation.EndDate;
            GuestID = existingReservation.GuestID;
            Total = existingReservation.Total;
        }
        public Reservation(int id, DateTime startDate, DateTime endDate, int guestID, decimal total)
        {
            ID = id;
            StartDate = startDate;
            EndDate = endDate;
            GuestID = guestID;
            Total = total;
        }

        public override bool Equals(object? obj)
        {
            return obj is Reservation reservation &&
                   ID == reservation.ID &&
                   StartDate == reservation.StartDate &&
                   EndDate == reservation.EndDate &&
                   GuestID == reservation.GuestID &&
                   Total == reservation.Total;
        }
        public override int GetHashCode()
        {
            return HashCode.Combine(ID, StartDate, EndDate, GuestID, Total);
        }
    }
}
