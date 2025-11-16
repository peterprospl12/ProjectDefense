using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProjectDefense.Application.DTOs;
using ProjectDefense.Application.UseCases.Queries;

namespace ProjectDefense.Web.Pages.Lecturer.Rooms
{
    [Authorize(Roles = "Lecturer")]
    public class IndexModel(IMediator mediator) : PageModel
    {
        public IEnumerable<RoomDto> Rooms { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        public async Task OnGetAsync()
        {
            Rooms = await mediator.Send(new GetAllRoomsQuery());
        }
    }
}
