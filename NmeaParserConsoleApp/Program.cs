using NmeaParser;

string? input = string.Empty;

while (input != "exit")
{
    Console.WriteLine("Enter a NMEA message (or 'exit' to quit):");
    input = Console.ReadLine();

    try
    {
        if (input == null)
        {
            Console.WriteLine("Input cannot be null.");
        }
        else
        {
            BaseMessage message = BaseMessage.CreateMessage(input);
            Console.WriteLine($"Parsed message: {message}");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error parsing message: {ex.Message}");
    }

    Console.WriteLine("Press ENTER to continue...");
    Console.ReadLine();
    Console.Clear();
}