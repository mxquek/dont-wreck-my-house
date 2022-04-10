
namespace DontWreckMyHouse.Core.Models
{
    public class Guest
    {
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string State { get; set; }

        public Guest() { }
        public Guest(Guest existingGuest)
        {
            ID = existingGuest.ID;
            FirstName = existingGuest.FirstName;
            LastName = existingGuest.LastName;
            Email = existingGuest.Email;
            PhoneNumber = existingGuest.PhoneNumber;
            State = existingGuest.State;
        }
        public Guest (int id, string firstName, string lastName, string email, string phone, string state)
        {
            ID = id;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            PhoneNumber = phone;
            State = state;
        }

        public override bool Equals(object? obj)
        {
            return obj is Guest guest &&
                   ID == guest.ID &&
                   FirstName == guest.FirstName &&
                   LastName == guest.LastName &&
                   Email == guest.Email &&
                   PhoneNumber == guest.PhoneNumber &&
                   State == guest.State;
        }
        public override int GetHashCode()
        {
            return HashCode.Combine(ID, FirstName, LastName, Email, PhoneNumber, State);
        }
    }
}
