using FluentAssertions;
using Data;
using Domain;
using Microsoft.EntityFrameworkCore;
using Application.Test;

namespace Application.Test
{
    public class FlightApplicationSpecifications
    {
        // entities are acting like our DB here
        readonly Entities entities = new Entities(new DbContextOptionsBuilder<Entities>()
                .UseInMemoryDatabase("Flights").Options);

        readonly BookingService bookingService;
        public FlightApplicationSpecifications()
        {
            bookingService = new BookingService(entities: entities);
        }

        [Theory]
        [InlineData("m@m.com", 2)]
        [InlineData("a@c.com", 2)]
        
        public void Remembers_bookings(string passengerEmail, int numberOfSeats)
        {

            var flight = new Flight(3);

            entities.Flights.Add(flight);

            

            bookingService.Book(new BookDto(
                flightId: flight.Id, passengerEmail: passengerEmail, numberOfSeats: numberOfSeats
                ));

            bookingService.FindBookings(flight.Id).Should().ContainEquivalentOf(
                new BookingRm(passengerEmail: passengerEmail, numberOfSeats: numberOfSeats)
                );
        }

        [Theory]
        [InlineData(3)]
        [InlineData(10)]
        public void Frees_up_seats_after_booking(int initialCapacity)
        {
            var flight = new Flight(initialCapacity);
            entities.Flights.Add(flight);
            entities.SaveChanges();

            bookingService.Book(new BookDto(
                flightId: flight.Id, passengerEmail: "m@m.com", numberOfSeats: 2
            ));

            // When
            bookingService.CancelBooking(new CancelBookingDto(
                flightId: flight.Id,
                passengerEmail: "m@m.com",
                numberOfSeats: 2
            ));

            // Then
            bookingService.GetRemainingNumberOfSeatsFor(flight.Id).Should().Be(initialCapacity);
        }
    }



}
