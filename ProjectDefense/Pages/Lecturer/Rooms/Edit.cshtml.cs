using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProjectDefense.Application.UseCases.Commands;
using ProjectDefense.Application.UseCases.Queries;
using System.ComponentModel.DataAnnotations;

namespace ProjectDefense.Web.Pages.Lecturer.Rooms
{
    [Authorize(Roles = "Lecturer")]
    public class EditModel(IMediator mediator) : PageModel
    {
        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            public int Id { get; init; }

            [Required]
            [StringLength(100)]
            public string Name { get; init; }

            [Required]
            [StringLength(20)]
            public string Number { get; init; }
        }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var room = await mediator.Send(new GetRoomByIdQuery { Id = id.Value });
            if (room == null)
            {
                return NotFound();
            }

            Input = new InputModel
            {
                Id = room.Id,
                Name = room.Name,
                Number = room.Number
            };

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await mediator.Send(new UpdateRoomCommand(Input.Id, Input.Name, Input.Number));

            TempData["StatusMessage"] = "Room has been updated successfully.";
            return RedirectToPage("./Index");
        }
    }
}