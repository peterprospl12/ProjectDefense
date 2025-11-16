using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProjectDefense.Application.UseCases.Commands;
using System.ComponentModel.DataAnnotations;

namespace ProjectDefense.Web.Pages.Lecturer.Rooms
{
    [Authorize(Roles = "Lecturer")]
    public class CreateModel(IMediator mediator) : PageModel
    {
        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [StringLength(100)]
            public string Name { get; init; }

            [Required]
            [StringLength(20)]
            public string Number { get; init; }
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await mediator.Send(new CreateRoomCommand(Input.Name, Input.Number));

            TempData["StatusMessage"] = "Room has been created successfully.";
            return RedirectToPage("./Index");
        }
    }
}
