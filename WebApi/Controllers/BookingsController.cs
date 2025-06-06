using Business.Dtos;
using Business.Interfaces;
using Business.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BookingsController(IBookingService bookingService) : ControllerBase
{
    private readonly IBookingService _bookingService = bookingService;

    // READ
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _bookingService.GetAllBookingsAsync();
        var bookings = result.Data;

        return result.Succeeded ? Ok(bookings) : StatusCode(result.StatusCode, result.ErrorMessage);
    }


    // CREATE
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Booking))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Create(CreateBookingRequestDto request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _bookingService.CreateBookingAsync(request);
        return result.Succeeded ? Created((string?)null, result.Data) : StatusCode(result.StatusCode, result.ErrorMessage);
    }


    // DELETE
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
            return BadRequest("Parameter cannot be null.");

        var result = await _bookingService.DeleteEventAsync(id);
        return result.Succeeded ? Ok($"Event with id {id} successfully deleted.") : StatusCode(result.StatusCode, result.ErrorMessage);
    }
}
