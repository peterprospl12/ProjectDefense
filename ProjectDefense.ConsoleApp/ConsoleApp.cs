using ProjectDefense.ConsoleApp.Clients;

namespace ProjectDefense.ConsoleApp;

public class ConsoleApp(ApiClient apiClient)
{
    public async Task RunAsync()
    {
        Console.WriteLine("Welcome to the reservation system console client!");
        Console.Write("Please enter your student ID (e.g., a GUID): ");
        var studentId = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(studentId))
        {
            Console.WriteLine("Student ID cannot be empty.");
            return;
        }

        Console.WriteLine($"Logged in as student with ID: {studentId}\n");

        while (true)
        {
            Console.WriteLine("Select an option:");
            Console.WriteLine("1. Display available slots");
            Console.WriteLine("2. Book a slot");
            Console.WriteLine("3. Exit");
            Console.Write("> ");

            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    await ShowAvailableSlots();
                    break;
                case "2":
                    await BookSlot(studentId);
                    break;
                case "3":
                    return;
                default:
                    Console.WriteLine("Invalid option, please try again.");
                    break;
            }
            Console.WriteLine();
        }
    }

    private async Task ShowAvailableSlots()
    {
        Console.WriteLine("\nFetching available slots...");
        var slots = await apiClient.GetAvailableSlotsAsync();

        if (slots == null || !slots.Any())
        {
            Console.WriteLine("No available slots or an error occurred.");
            return;
        }

        Console.WriteLine("--- Available Slots ---");
        foreach (var slot in slots)
        {
            Console.WriteLine($"ID: {slot.Id} | Room: {slot.RoomName} ({slot.RoomNumber}) | Time: {slot.StartTime:g} - {slot.EndTime:t}");
        }
        Console.WriteLine("-----------------------");
    }

    private async Task BookSlot(string studentId)
    {
        Console.Write("Enter the ID of the slot you want to book: ");
        if (!int.TryParse(Console.ReadLine(), out var slotId))
        {
            Console.WriteLine("Invalid slot ID.");
            return;
        }

        Console.WriteLine($"Attempting to book slot ID: {slotId} for student {studentId}...");
        var success = await apiClient.BookSlotAsync(slotId, studentId);

        if (success)
        {
            Console.WriteLine("Booking was successful!");
        }
        else
        {
            Console.WriteLine("Failed to book the slot.");
        }
    }
}