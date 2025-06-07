using Business.Dtos;
using Business.Dtos.API;
using Business.Interfaces;
using Business.Models;
using Data.Entities;
using Data.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Reflection;

namespace Business.Services;

public class BookingService(IBookingRepository bookingRepository) : IBookingService
{
    private readonly IBookingRepository _bookingRepository = bookingRepository;


    // CREATE
    public async Task<BookingResult<Booking?>> CreateBookingAsync(CreateBookingRequestDto request)
    {
        if (request == null)
            return BookingResult<Booking?>.BadRequest("Request cannot be null.");

        try
        {
            var bookingEntity = new BookingEntity
            {
                EventId = request.EventId,
                CustomerId = request.CustomerId,
                TicketQuantity = request.TicketQuantity,
                PackageId = request.PackageId,
            };

            await _bookingRepository.AddAsync(bookingEntity);
            var result = await _bookingRepository.SaveAsync();

            if (!result.Succeeded)
                return BookingResult<Booking?>.InternalServerError($"Failed creating booking. {result.ErrorMessage}");

            var createdBookingEntity = (await _bookingRepository.GetOneAsync(x => x.Id == bookingEntity.Id)).Data;
            if (createdBookingEntity == null)
                return BookingResult<Booking?>.InternalServerError($"Failed retrieving booking entity after creation.");

            var bookingModel = new Booking
            {
                Id = createdBookingEntity.Id,
                EventId = createdBookingEntity.EventId,
                CustomerId = createdBookingEntity.CustomerId,
                TicketQuantity = createdBookingEntity.TicketQuantity,
                PackageId = createdBookingEntity.PackageId,
                BookingDate = createdBookingEntity.BookingDate,
            };

            return BookingResult<Booking?>.Created(bookingModel);
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return BookingResult<Booking?>.InternalServerError($"Exception occured in {MethodBase.GetCurrentMethod()!.Name}");
        }
    }


    // READ
    public async Task<BookingResult<IEnumerable<Booking>>> GetAllBookingsAsync()
    {
        var result = await _bookingRepository.GetAllAsync();
        if (!result.Succeeded)
            return BookingResult<IEnumerable<Booking>>.InternalServerError($"Failed retrieving booking entities. {result.ErrorMessage}");

        var entities = result.Data;
        var bookings = entities?.Select(x => new Booking
        {
            Id = x.Id,
            EventId = x.EventId,
            CustomerId = x.CustomerId,
            TicketQuantity = x.TicketQuantity,
            PackageId = x.PackageId,
            BookingDate = x.BookingDate,
        });

        return BookingResult<IEnumerable<Booking>>.Ok(bookings!);
    }

    public async Task<BookingResult<int>> GetTicketsSoldAmountByEventId(string eventId)
    {
        if (string.IsNullOrWhiteSpace(eventId))
            return BookingResult<int>.BadRequest("EventId cannot be null or empty.");

        var result = await _bookingRepository.GetAllAsync(where: x => x.EventId == eventId);
        if (!result.Succeeded)
            return BookingResult<int>.InternalServerError($"Failed retrieving booking entities. {result.ErrorMessage}");

        var entities = result.Data;
        int totalTickets = entities?.Sum(x => x.TicketQuantity) ?? 0;

        return BookingResult<int>.Ok(totalTickets);
    }

    public async Task<BookingResult<IEnumerable<EventTicketsSold>>> GetTicketsSoldAmountAllEvents()
    {
        var result = await _bookingRepository.GetAllAsync();
        if (!result.Succeeded)
            return BookingResult<IEnumerable<EventTicketsSold>>.InternalServerError($"Failed retrieving booking entities. {result.ErrorMessage}");

        var entities = result.Data;
        var eventTicketsSold = entities
            ?.GroupBy(b => b.EventId)
            .Select(g => new EventTicketsSold
            {
                EventId = g.Key,
                TicketsSold = g.Sum(b => b.TicketQuantity)
            });

        return BookingResult<IEnumerable<EventTicketsSold>>.Ok(eventTicketsSold!);
    }


    // DELETE
    public async Task<BookingResult<bool>> DeleteEventAsync(string id)
    {
        try
        {
            var getResult = await _bookingRepository.GetOneAsync(x => x.Id == id);
            if (!getResult.Succeeded || getResult.Data == null)
                return BookingResult<bool>.NotFound($"Booking with id {id} not found.");

            var entity = getResult.Data;
            _bookingRepository.Delete(entity);
            var result = await _bookingRepository.SaveAsync();
            if (!result.Succeeded)
                return BookingResult<bool>.InternalServerError($"Failed deleting event with id {id}.");

            return BookingResult<bool>.Ok(true);
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return BookingResult<bool>.InternalServerError($"Exception occured in {MethodBase.GetCurrentMethod()!.Name}");
        }
    }
}
