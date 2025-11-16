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
    public class EditModel : PageModel
    {
        private readonly IMediator _mediator;

        public EditModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            public int Id { get; set; }

            [Required]
            [StringLength(100)]
            public string Name { get; set; }

            [Required]
            [StringLength(20)]
            public string Number { get; set; }
        }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var room = await _mediator.Send(new GetRoomByIdQuery { Id = id.Value });
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

            await _mediator.Send(new UpdateRoomCommand(Input.Id, Input.Name, Input.Number));

            TempData["StatusMessage"] = "Room has been updated successfully.";
            return RedirectToPage("./Index");
        }
    }
}