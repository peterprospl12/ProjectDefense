using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProjectDefense.Application.UseCases.Queries;
using ProjectDefense.Application.UseCases.Commands;
using ProjectDefense.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace ProjectDefense.Web.Pages.Lecturer.Blocks
{
    [Authorize(Roles = "Lecturer")]
    public class CreateModel : PageModel
    {
        private readonly IMediator _mediator;
        private readonly UserManager<User> _userManager;

        public CreateModel(IMediator mediator, UserManager<User> userManager)
        {
            _mediator = mediator;
            _userManager = userManager;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public SelectList Rooms { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        public class InputModel
        {
            [Required]
            [Display(Name = "Room")]
            public int RoomId { get; set; }

            [Required]
            [Display(Name = "Start (local)")]
            [DataType(DataType.DateTime)]
            public DateTime Start { get; set; } = DateTime.Now;

            [Required]
            [Display(Name = "End (local)")]
            [DataType(DataType.DateTime)]
            public DateTime End { get; set; } = DateTime.Now.AddHours(1);
        }

        public async Task OnGetAsync()
        {
            var rooms = await _mediator.Send(new GetAllRoomsQuery());
            Rooms = new SelectList(rooms, "Id", "Name");
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await OnGetAsync();
                return Page();
            }

            if (Input.End <= Input.Start)
            {
                ModelState.AddModelError(string.Empty, "End must be after Start.");
                await OnGetAsync();
                return Page();
            }

            var lecturer = await _userManager.GetUserAsync(User);
            if (lecturer == null)
            {
                return Forbid();
            }

            try
            {
                var command = new BlockPeriodCommand
                (
                    lecturer.Id,
                    Input.Start,
                    Input.End
                );

                await _mediator.Send(command);

                StatusMessage = "Selected period has been blocked. Existing reservations in this period were cancelled.";
                return RedirectToPage("/Lecturer/Index");
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                await OnGetAsync();
                return Page();
            }
        }
    }
}
