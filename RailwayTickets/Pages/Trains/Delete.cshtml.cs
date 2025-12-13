using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RailwayTickets.Data;
using RailwayTickets.Models;

namespace RailwayTickets.Pages.Trains;

public class DeleteModel : PageModel
{
    private readonly RailwayContext _context;

    public DeleteModel(RailwayContext context)
    {
        _context = context;
    }

    [BindProperty]
    public Train? Train { get; set; }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        Train = await _context.Trains.AsNoTracking().FirstOrDefaultAsync(t => t.Id == id);
        if (Train == null)
        {
            return NotFound();
        }

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int id)
    {
        var train = await _context.Trains.FindAsync(id);
        if (train == null)
        {
            return NotFound();
        }

        _context.Trains.Remove(train);
        await _context.SaveChangesAsync();
        return RedirectToPage("Index");
    }
}

