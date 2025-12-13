using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RailwayTickets.Data;
using RailwayTickets.Models;

namespace RailwayTickets.Pages.Tickets;

public class DeleteModel : PageModel
{
    private readonly RailwayContext _context;

    public DeleteModel(RailwayContext context)
    {
        _context = context;
    }

    [BindProperty]
    public Ticket? Ticket { get; set; }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        Ticket = await _context.Tickets
            .Include(t => t.Train)
            .Include(t => t.FromStation)
            .Include(t => t.ToStation)
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.Id == id);

        if (Ticket == null)
        {
            return NotFound();
        }

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int id)
    {
        var ticket = await _context.Tickets.FindAsync(id);
        if (ticket == null)
        {
            return NotFound();
        }

        _context.Tickets.Remove(ticket);
        await _context.SaveChangesAsync();
        return RedirectToPage("Index");
    }
}

