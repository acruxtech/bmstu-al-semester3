using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RailwayTickets.Data;
using RailwayTickets.Models;

namespace RailwayTickets.Pages.Stations;

public class EditModel : PageModel
{
    private readonly RailwayContext _context;

    public EditModel(RailwayContext context)
    {
        _context = context;
    }

    [BindProperty]
    public Station Station { get; set; } = new();

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var station = await _context.Stations.FindAsync(id);
        if (station == null)
        {
            return NotFound();
        }

        Station = station;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        _context.Attach(Station).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await _context.Stations.AnyAsync(s => s.Id == Station.Id))
            {
                return NotFound();
            }
            throw;
        }

        return RedirectToPage("Index");
    }
}

