using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application
{
    public class BookDto
    {
        public Guid FlightId { get;  }
        public string PassengerEmail { get; }
        public int NumberOfSeats { get;  }
        public BookDto(Guid flightId, string passengerEmail, int numberOfSeats)
        {
            FlightId = flightId;
            PassengerEmail = passengerEmail;
            NumberOfSeats = numberOfSeats;
        }
    }
}
