using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RailwayTickets.Data;
using RailwayTickets.Models;

namespace RailwayTickets.Pages.Stations;

public class DeleteModel : PageModel
{
    private readonly RailwayContext _context;

    public DeleteModel(RailwayContext context)
    {
        _context = context;
    }

    [BindProperty]
    public Station? Station { get; set; }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        Station = await _context.Stations.AsNoTracking().FirstOrDefaultAsync(s => s.Id == id);
        if (Station == null)
        {
            return NotFound();
        }

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int id)
    {
        var station = await _context.Stations.FindAsync(id);
        if (station == null)
        {
            return NotFound();
        }

        _context.Stations.Remove(station);
        await _context.SaveChangesAsync();
        return RedirectToPage("Index");
    }
}

