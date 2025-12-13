using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RailwayTickets.Data;
using RailwayTickets.Models;

namespace RailwayTickets.Pages.Stations;

public class IndexModel : PageModel
{
    private readonly RailwayContext _context;

    public IndexModel(RailwayContext context)
    {
        _context = context;
    }

    public IList<Station> Stations { get; private set; } = new List<Station>();

    public async Task OnGetAsync()
    {
        Stations = await _context.Stations
            .AsNoTracking()
            .OrderBy(s => s.City)
            .ThenBy(s => s.Name)
            .ToListAsync();
    }
}

