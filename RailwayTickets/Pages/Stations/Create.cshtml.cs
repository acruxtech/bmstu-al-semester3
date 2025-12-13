using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RailwayTickets.Data;
using RailwayTickets.Models;

namespace RailwayTickets.Pages.Stations;

public class CreateModel : PageModel
{
    private readonly RailwayContext _context;

    public CreateModel(RailwayContext context)
    {
        _context = context;
    }

    [BindProperty]
    public Station Station { get; set; } = new();

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        _context.Stations.Add(Station);
        await _context.SaveChangesAsync();
        return RedirectToPage("Index");
    }
}

