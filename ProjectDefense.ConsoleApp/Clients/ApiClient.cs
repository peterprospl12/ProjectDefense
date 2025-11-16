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

    public async Task<bool> BookSlotAsync(int slotId, string studentId)
    {
        var request = new BookReservationDto() { StudentIndex = studentId };
        try
        {
            var response = await httpClient.PostAsJsonAsync($"api/slots/{slotId}/book", request);
            
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Reservation error: {response.StatusCode}. Response: {errorContent}");
                return false;
            }
            
            return true;
        }
        catch (HttpRequestException e)
        {
            Console.WriteLine($"HTTP Request error: {e.Message}");
            return false;
        }
    }
}
