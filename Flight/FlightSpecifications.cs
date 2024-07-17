using Domain;
using FluentAssertions;
namespace FlightTest
{
    public class FlightSpecifications
    {
        [Theory]    // Theory tells us some test data is coming from outside of the method
        [InlineData (3,1,2)]    // Provides a data source for our theory attribute above, we pass the values to our parameters (seatCapacity will be 3, numberOfSeats will be 1, remainingNumberOfSeats will be 2)
        [InlineData(6, 3, 3)]
        [InlineData(10, 6, 4)]  // At this point we are testing for 3 differnt scenarios
        public void Booking_reduces_the_number_of_seats(int seatCapacity, int numberOfSeat, int remainingNumberOfSeats)
        {
            var flight = new Flight(seatCapacity: seatCapacity);

            flight.Book("test@gmail.com", numberOfSeat);

            flight.RemainingNumberOfSeats.Should().Be(remainingNumberOfSeats);
        }

        [Fact]
        public void Avoids_overbooking()
        {
            // Given 
            var flight = new Flight(seatCapacity: 3);

            // When
            var error = flight.Book("test@gmail.com", 4);

            // Then
            error.Should().BeOfType<OverbookingError>();
        }

        [Fact]
        public void Books_flights_successfully()
        {
            var flight = new Flight(seatCapacity: 3);
            var error = flight.Book("test@gmail.com", 1);
            error.Should().BeNull();
        }

        [Fact]
        public void Remebers_bookings()
        {
            var flight = new Flight(seatCapacity: 150);
            
            flight.Book(passengerEmail: "a@b.com", numberOfSeats: 4);

            flight.BookingList.Should().ContainEquivalentOf(new Booking("a@b.com", 4));
        }

        [Theory]
        [InlineData(3,1,1,3)]
        [InlineData(4, 2, 2, 4)]
        [InlineData(7, 5, 4, 6)]
        public void Canceling_bookings_frees_up_the_seats(int initalCapacity, int numberOfSeatsToBook, int numberOfSeatsToCancel, int remaingNumberOfSeats)
        {
            // Given
            var flight = new Flight(initalCapacity);
            flight.Book(passengerEmail: "a@b.com", numberOfSeats: numberOfSeatsToBook);

            // When
            flight.CancelBooking(passengerEmail: "a@b.com", numberOfSeats: numberOfSeatsToCancel);


            // Then
            flight.RemainingNumberOfSeats.Should().Be(remaingNumberOfSeats);
        }

        [Fact]
        public void Doesnt_cancel_bookings_for_passengers_who_have_not_booked()
        {
            
            var flight = new Flight(3);
            
            var error = flight.CancelBooking(passengerEmail: "a@b.com", numberOfSeats: 2);
            
            error.Should().BeOfType<BookingNotFoundError>();
        }

        [Fact]
        public void Returns_null_when_successfully_cancels_a_booking()
        {
            var flight = new Flight(3);

            flight.Book(passengerEmail: "a@b.com", numberOfSeats: 1);
            var error = flight.CancelBooking(passengerEmail: "a@b.com", numberOfSeats: 1);
            
            error.Should().BeNull();
        }
    }
}