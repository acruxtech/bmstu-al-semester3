using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RailwayTickets.Data;
using RailwayTickets.Models;

namespace RailwayTickets.Pages.Tickets;

public class CreateModel : PageModel
{
    private readonly RailwayContext _context;

    public CreateModel(RailwayContext context)
    {
        _context = context;
    }

    [BindProperty]
    public Ticket Ticket { get; set; } = new();

    public SelectList TrainOptions { get; private set; } = default!;
    public SelectList StationOptions { get; private set; } = default!;

    public async Task OnGetAsync()
    {
        await LoadSelectListsAsync();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            await LoadSelectListsAsync();
            return Page();
        }

        _context.Tickets.Add(Ticket);
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

