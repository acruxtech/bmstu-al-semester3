using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RailwayTickets.Data;
using RailwayTickets.Models;

namespace RailwayTickets.Pages.Tickets;

public class EditModel : PageModel
{
    private readonly RailwayContext _context;

    public EditModel(RailwayContext context)
    {
        _context = context;
    }

    [BindProperty]
    public Ticket Ticket { get; set; } = new();

    public SelectList TrainOptions { get; private set; } = default!;
    public SelectList StationOptions { get; private set; } = default!;

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var ticket = await _context.Tickets.FindAsync(id);
        if (ticket == null)
        {
            return NotFound();
        }

        Ticket = ticket;
        await LoadSelectListsAsync();
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            await LoadSelectListsAsync();
            return Page();
        }

        _context.Attach(Ticket).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return RedirectToPage("Index");
    }

    private async Task LoadSelectListsAsync()
    {
        var trains = await _context.Trains.AsNoTracking().OrderBy(t => t.Number).ToListAsync();
        TrainOptions = new SelectList(trains, nameof(Train.Id), nameof(Train.Number));

        var stations = await _context.Stations.AsNoTracking()
            .OrderBy(s => s.City)
            .ThenBy(s => s.Name)
            .ToListAsync();
        StationOptions = new SelectList(stations, nameof(Station.Id), nameof(Station.Name));
    }

}

