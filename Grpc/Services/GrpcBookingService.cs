using Business.Interfaces;
using Grpc.Core;
using Protos;
using System.Diagnostics;

namespace Grpc.Services;

public class GrpcBookingService(IBookingService bookingService) : GrpcBooking.GrpcBookingBase
{
    private readonly IBookingService _bookingService = bookingService;


    // READ
    public override async Task<GetTicketsSoldAmountAllEventsResponse> GetTicketsSoldAmountAllEvents(GetTicketsSoldAmountAllEventsRequest request, ServerCallContext context)
    {
        Debug.WriteLine("Getting tickets sold for all events...");

        var result = await _bookingService.GetTicketsSoldAmountAllEvents();

        var response = new GetTicketsSoldAmountAllEventsResponse
        {
            Succeeded = result.Succeeded,
            StatusCode = result.StatusCode,
            ErrorMessage = result.ErrorMessage ?? string.Empty,
        };

        if (result.Data != null)
        {
            foreach (var eventTicketsSold in result.Data)
                response.EventTicketsSold.Add(new Protos.EventTicketsSold
                {
                    EventId = eventTicketsSold.EventId,
                    TicketsSold = eventTicketsSold.TicketsSold
                });
        }

        return response;
    }
}
