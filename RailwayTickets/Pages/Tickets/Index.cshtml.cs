using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RailwayTickets.Data;
using RailwayTickets.Models;

namespace RailwayTickets.Pages.Tickets;

public class IndexModel : PageModel
{
    private readonly RailwayContext _context;

    public IndexModel(RailwayContext context)
    {
        _context = context;
    }

    public IList<Ticket> Tickets { get; private set; } = new List<Ticket>();

    public async Task OnGetAsync()
    {
        Tickets = await _context.Tickets
            .Include(t => t.Train)
            .Include(t => t.FromStation)
            .Include(t => t.ToStation)
            .AsNoTracking()
            .OrderByDescending(t => t.Id)
            .ToListAsync();
    }
}

