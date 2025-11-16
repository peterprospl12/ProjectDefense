using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProjectDefense.Application.DTOs;
using ProjectDefense.Application.UseCases.Commands;
using ProjectDefense.Application.UseCases.Queries;

namespace ProjectDefense.Web.Pages.Lecturer.Rooms
{
    [Authorize(Roles = "Lecturer")]
    public class DeleteModel(IMediator mediator) : PageModel
    {
        [BindProperty]
        public RoomDto Room { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Room = await mediator.Send(new GetRoomByIdQuery { Id = id.Value });

            if (Room == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {
                await mediator.Send(new DeleteRoomCommand(id.Value));
                TempData["StatusMessage"] = "Room has been deleted successfully.";
                return RedirectToPage("./Index");
            }
            catch (InvalidOperationException ex)
            {
                TempData["StatusMessage"] = $"Error: {ex.Message}";
                return RedirectToPage("./Index");
            }
        }
    }
}
