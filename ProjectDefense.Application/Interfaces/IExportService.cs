using ProjectDefense.Application.DTOs;

namespace ProjectDefense.Application.Interfaces;

public interface IExportService
{
    Task<byte[]> ExportToTxtAsync(IEnumerable<ReservationDto> reservations);
    Task<byte[]> ExportToXlsxAsync(IEnumerable<ReservationDto> reservations);
    Task<byte[]> ExportToPdfAsync(IEnumerable<ReservationDto> reservations);
}
