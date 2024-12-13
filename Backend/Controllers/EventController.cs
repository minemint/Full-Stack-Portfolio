using DotnetStockAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace DotnetStockAPI.Controllers;

[Route("api/[controller]")]
public class EventController : ControllerBase
{


    private readonly ApplicationDbContext _context;

    public EventController(ApplicationDbContext context)
    {
        _context = context;
    }
    [HttpGet]
    public ActionResult<IEnumerable<Event>> GetEvents()
    { 
        try
        {
            var events = _context.events.ToList();
            return Ok(events);

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            throw;
        }
    }

}