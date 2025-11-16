using System.ComponentModel.DataAnnotations;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProjectDefense.Application.DTOs;
using ProjectDefense.Application.Interfaces;
using ProjectDefense.Application.UseCases.Queries;
using ProjectDefense.Domain.Entities;

namespace ProjectDefense.Web.Pages.Lecturer;

[Authorize(Roles = "Lecturer")]
public class ExportModel(
    IMediator mediator,
    IExportService exportService,
    UserManager<User> userManager) : PageModel
{
    [BindProperty]
    public InputModel Input { get; set; } = new();
    public SelectList RoomOptions { get; set; }

    public enum ExportFormat { Txt, Xlsx, Pdf }

    public class InputModel
    {
        [Required]
        [Display(Name = "Room")]
        public int? RoomId { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; } = DateTime.Today;

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "End Date")]
        public DateTime EndDate { get; set; } = DateTime.Today.AddDays(7);

        [Required]
        public ExportFormat Format { get; set; }
    }

    public async Task OnGetAsync()
    {
        await LoadRoomOptions();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            await LoadRoomOptions();
            return Page();
        }

        var lecturerId = userManager.GetUserId(User);
        if (lecturerId == null)
        {
            return Forbid();
        }

        var query = new GetAllReservationsByRangeQuery(
            lecturerId,
            Input.RoomId!.Value,
            Input.StartDate,
            Input.EndDate.AddDays(1).AddTicks(-1));

        var reservations = await mediator.Send(query);
        var reservationDtos = reservations.ToList();

        byte[] fileContents;
        string contentType;
        string fileName = $"Export_{Input.StartDate:yyyy-MM-dd}_{Input.EndDate:yyyy-MM-dd}";

        switch (Input.Format)
        {
            case ExportFormat.Txt:
                fileContents = await exportService.ExportToTxtAsync(reservationDtos);
                contentType = "text/plain";
                fileName += ".txt";
                break;
            case ExportFormat.Xlsx:
                fileContents = await exportService.ExportToXlsxAsync(reservationDtos);
                contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                fileName += ".xlsx";
                break;
            case ExportFormat.Pdf:
                fileContents = await exportService.ExportToPdfAsync(reservationDtos);
                contentType = "application/pdf";
                fileName += ".pdf";
                break;
            default:
                return BadRequest("Invalid format");
        }

        return File(fileContents, contentType, fileName);
    }

    private async Task LoadRoomOptions()
    {
        var rooms = await mediator.Send(new GetAllRoomsQuery());
        RoomOptions = new SelectList(rooms, "Id", "Name");
    }
}