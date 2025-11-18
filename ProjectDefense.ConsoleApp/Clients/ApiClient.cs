using System.Net.Http.Json;
using ProjectDefense.Application.DTOs;

namespace ProjectDefense.ConsoleApp.Clients;

public class ApiClient(HttpClient httpClient)
{
    public async Task<IEnumerable<AvailableReservationDto>?> GetAvailableSlotsAsync()
    {
        try
        {
            return await httpClient.GetFromJsonAsync<IEnumerable<AvailableReservationDto>>("api/slots/available");
        }
        catch (HttpRequestException e)
        {
            System.Console.WriteLine($"HTTP Request error: {e.Message}");
            return null;
        }
    }

    public async Task BookSlotAsync(int slotId, string studentId)
    {
        var request = new BookReservationDto() { StudentIndex = studentId };
        try
        {
            var response = await httpClient.PostAsJsonAsync($"api/slots/{slotId}/book", request);
            
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                var errorMessage = errorContent.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries).FirstOrDefault();
                throw new InvalidOperationException(errorMessage ?? "Unexpected error occured.");
            }
        }
        catch (HttpRequestException e)
        {
            throw new InvalidOperationException($"HTTP Request error: {e.Message}", e);
        }
    }
}