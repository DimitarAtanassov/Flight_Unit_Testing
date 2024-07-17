
namespace Domain
{
    public class Flight
    {
        private List<Booking> bookingList = new();
        public IEnumerable<Booking> BookingList => bookingList; // BookingList => : Expression-bodied member syntax, which provides a concise way to implement a read-only property. The BookingList property returns the 'bookingList' field
        public int RemainingNumberOfSeats { get; set; }
        public Guid Id { get; }

        [Obsolete("Needed by EF")]
        Flight() { }
        public Flight(int seatCapacity)
        {
            RemainingNumberOfSeats = seatCapacity;
        }

        public object? Book(string passengerEmail, int numberOfSeats)
        {
            if (this.RemainingNumberOfSeats < numberOfSeats) return new OverbookingError();

            RemainingNumberOfSeats -= numberOfSeats;

            bookingList.Add(new Booking(passengerEmail, numberOfSeats));

            return null;
        }

        public object? CancelBooking(string passengerEmail, int numberOfSeats)
        {
            var booking = bookingList.FirstOrDefault(b => b.Email == passengerEmail && b.NumberOfSeats == numberOfSeats);
            // LINQ
            if (!bookingList.Any(booking => booking.Email == passengerEmail)) return new BookingNotFoundError();
                
            RemainingNumberOfSeats += numberOfSeats;
            bookingList.Remove(booking);
            return null;
        }
    }
}
