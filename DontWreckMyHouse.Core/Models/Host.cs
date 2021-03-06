
namespace DontWreckMyHouse.Core.Models
{
    public class Host
    {
        public string ID { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public decimal StandardRate { get; set; }
        public decimal WeekendRate { get; set; }

        public Host() { }
        public Host(Host copy) 
        {
            ID = copy.ID;
            LastName = copy.LastName;
            Email = copy.Email;
            PhoneNumber = copy.PhoneNumber;
            Address = copy.Address;
            City = copy.City;
            State = copy.State;
            PostalCode = copy.PostalCode;
            StandardRate = copy.StandardRate;
            WeekendRate = copy.WeekendRate;
        }
        public Host(string id, string lastName, string email, string phoneNumber, string address, string city, string state, string postalCode, decimal standardRate, decimal weekendRate)
        {
            ID = id;
            LastName = lastName;
            Email = email;
            PhoneNumber = phoneNumber;
            Address = address;
            City = city;
            State = state;
            PostalCode = postalCode;
            StandardRate = standardRate;
            WeekendRate = weekendRate;
        }

        public override bool Equals(object? obj)
        {
            return obj is Host host &&
                   ID == host.ID &&
                   LastName == host.LastName &&
                   Email == host.Email &&
                   PhoneNumber == host.PhoneNumber &&
                   City == host.City &&
                   State == host.State &&
                   PostalCode == host.PostalCode &&
                   StandardRate == host.StandardRate &&
                   WeekendRate == host.WeekendRate;
        }
        public override int GetHashCode()
        {
            return HashCode.Combine(ID, LastName, Email, PhoneNumber, Address, City, State, PostalCode);
        }
    }
}
