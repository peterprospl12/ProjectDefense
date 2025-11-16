using System.Text;
using ClosedXML.Excel;
using ProjectDefense.Application.DTOs;
using ProjectDefense.Application.Interfaces;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace ProjectDefense.Infrastructure.Services;

public class ExportService : IExportService
{
    public Task<byte[]> ExportToTxtAsync(IEnumerable<ReservationDto> reservations)
    {
        var builder = new StringBuilder();
        builder.AppendLine("Reservation Report");
        builder.AppendLine("---------------------------------");
        foreach (var reservation in reservations)
        {
            var studentInfo = string.IsNullOrWhiteSpace(reservation.StudentName) ? "FREE" : reservation.StudentName;
            var roomName = string.IsNullOrWhiteSpace(reservation.RoomName) ? "-" : reservation.RoomName;
            var roomNumber = string.IsNullOrWhiteSpace(reservation.RoomNumber) ? "-" : reservation.RoomNumber;
            
            builder.AppendLine($"Slot: {reservation.StartTime:g} - {reservation.EndTime:t} | Room: {roomName} ({roomNumber}) | Student: {studentInfo}" );
        }
        builder.AppendLine("---------------------------------");

        return Task.FromResult(Encoding.UTF8.GetBytes(builder.ToString()));
    }

    public Task<byte[]> ExportToXlsxAsync(IEnumerable<ReservationDto> reservations)
    {
        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("Reservations");
        worksheet.Cell("A1").Value = "Start Time";
        worksheet.Cell("B1").Value = "End Time";
        worksheet.Cell("C1").Value = "Room";
        worksheet.Cell("D1").Value = "Room number";
        worksheet.Cell("E1").Value = "Student Name";

        var row = 2;
        foreach (var reservation in reservations)
        {
            worksheet.Cell(row, 1).Value = reservation.StartTime;
            worksheet.Cell(row, 2).Value = reservation.EndTime;
            worksheet.Cell(row, 3).Value = reservation.RoomName;
            worksheet.Cell(row, 4).Value = reservation.RoomNumber;
            worksheet.Cell(row, 5).Value = string.IsNullOrWhiteSpace(reservation.StudentName) ? "FREE" : reservation.StudentName;
            row++;
        }

        worksheet.Columns().AdjustToContents();

        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        return Task.FromResult(stream.ToArray());
    }

    public Task<byte[]> ExportToPdfAsync(IEnumerable<ReservationDto> reservations)
    {
        QuestPDF.Settings.License = LicenseType.Community;

        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(2, Unit.Centimetre);
                page.DefaultTextStyle(x => x.FontSize(12));

                page.Header()
                    .Text("Reservation Report")
                    .SemiBold().FontSize(20).FontColor(Colors.Blue.Medium);

                page.Content()
                    .Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.RelativeColumn(3);
                            columns.RelativeColumn(3);
                            columns.RelativeColumn(2);
                            columns.RelativeColumn(2);
                            columns.RelativeColumn(4);
                        });

                        table.Header(header =>
                        {
                            header.Cell().Text("Start Time");
                            header.Cell().Text("End Time");
                            header.Cell().Text("Room");
                            header.Cell().Text("Room number");
                            header.Cell().Text("Student");
                        });

                        foreach (var reservation in reservations)
                        {
                            table.Cell().Text(reservation.StartTime.ToString("g"));
                            table.Cell().Text(reservation.EndTime.ToString("t"));
                            table.Cell().Text(reservation.RoomName);
                            table.Cell().Text(reservation.RoomNumber);
                            table.Cell().Text(string.IsNullOrWhiteSpace(reservation.StudentName) ? "FREE" : reservation.StudentName);
                        }
                    });

                page.Footer()
                    .AlignCenter()
                    .Text(x =>
                    {
                        x.Span("Page ");
                        x.CurrentPageNumber();
                    });
            });
        });

        return Task.FromResult(document.GeneratePdf());
    }
}
