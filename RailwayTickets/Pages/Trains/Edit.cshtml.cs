using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RailwayTickets.Data;
using RailwayTickets.Models;

namespace RailwayTickets.Pages.Trains;

public class EditModel : PageModel
{
    private readonly RailwayContext _context;

    public EditModel(RailwayContext context)
    {
        _context = context;
    }

    [BindProperty]
    public Train Train { get; set; } = new();

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var train = await _context.Trains.FindAsync(id);
        if (train == null)
        {
            return NotFound();
        }

        Train = train;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        _context.Attach(Train).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await _context.Trains.AnyAsync(t => t.Id == Train.Id))
            {
                return NotFound();
            }
            throw;
        }

        return RedirectToPage("Index");
    }
}

