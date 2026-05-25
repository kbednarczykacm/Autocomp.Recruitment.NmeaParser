using NmeaParser;

string? input = string.Empty;

while (input != "exit")
{
    Console.WriteLine("Enter a NMEA message (or 'exit' to quit):");
    input = Console.ReadLine();

    try
    {
        if (input == "exit")
        {
            continue;
        }
        if (input == null)
        {
            Console.WriteLine("Input cannot be null.");
        }
        else
        {
            BaseMessage message = BaseMessage.CreateMessage(input);
            PrintMessage(message);
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

void PrintMessage(BaseMessage msg)
{
    Console.WriteLine("------------------------------");
    Console.WriteLine($"Talker ID: {msg.TalkerId}");
    Console.WriteLine($"Message type: {msg.MessageType}");
    Console.WriteLine($"Checksum: {msg.Checksum}");
    Console.WriteLine($"Checksum OK: {msg.ChecksumOk}");

    switch (msg)
    {
        case GllMessage gll:
            PrintGllMessage(gll);
            break;
        case MwvMessage mwv:
            PrintMwvMessage(mwv);
            break;
        case null:
            throw new ArgumentNullException(nameof(msg));
        default:
            throw new NotSupportedException($"Invalid type: {msg.GetType()}");
    }
    Console.WriteLine("------------------------------");
}

void PrintGllMessage(GllMessage msg)
{
    Console.WriteLine($"Latitude: {msg.Latitude}");
    Console.WriteLine($"Longitude: {msg.Longitude}");
    Console.WriteLine($"Time: {msg.Time:T}");
    Console.WriteLine($"Status: {msg.Status}");
    Console.WriteLine($"Mode indicator: {msg.Mode}");
}

void PrintMwvMessage(MwvMessage msg)
{
    Console.WriteLine($"Wind angle: {msg.WindAngle}");
    Console.WriteLine($"Reference: {msg.Reference}");
    Console.WriteLine($"Wind speed: {msg.WindSpeed}");
    Console.WriteLine($"Unit: {msg.Unit}");
    Console.WriteLine($"Status: {msg.Status}");
}