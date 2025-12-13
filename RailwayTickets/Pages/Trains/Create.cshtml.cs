using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RailwayTickets.Data;
using RailwayTickets.Models;

namespace RailwayTickets.Pages.Trains;

public class CreateModel : PageModel
{
    private readonly RailwayContext _context;

    public CreateModel(RailwayContext context)
    {
        _context = context;
    }

    [BindProperty]
    public Train Train { get; set; } = new();

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        _context.Trains.Add(Train);
        await _context.SaveChangesAsync();
        return RedirectToPage("Index");
    }
}

