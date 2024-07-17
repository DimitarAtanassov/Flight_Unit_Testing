using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Booking
    {
        public string Email { get; set; }
        public int NumberOfSeats { get; set; }

        public Booking() { }
        public Booking(string email, int numberOfseats)
        {
            this.Email = email;
            this.NumberOfSeats = numberOfseats;
        }
    }
}
