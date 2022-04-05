using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DontWreckMyHouse.Core.Models
{
    public class Host
    {
        public string ID { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public decimal StandardRate { get; set; }
        public decimal WeekendRate { get; set; }

        public Host(string id, string lastName, string email, string phoneNumber, string city, string state, string postalCode, decimal standardRate, decimal weekendRate)
        {
            ID = id;
            LastName = lastName;
            Email = email;
            PhoneNumber = phoneNumber;
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
            return HashCode.Combine(ID, LastName, Email, PhoneNumber, City, State, PostalCode);
        }
    }
}
