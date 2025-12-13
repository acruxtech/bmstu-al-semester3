using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RailwayTickets.Data;
using RailwayTickets.Models;

namespace RailwayTickets.Pages.Trains;

public class IndexModel : PageModel
{
    private readonly RailwayContext _context;

    public IndexModel(RailwayContext context)
    {
        _context = context;
    }

    public IList<Train> Trains { get; private set; } = new List<Train>();

    public async Task OnGetAsync()
    {
        Trains = await _context.Trains.AsNoTracking()
            .OrderBy(t => t.Number)
            .ToListAsync();
    }
}

