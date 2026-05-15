using System.Globalization;
using Autocomp.Nmea.Common;

namespace NmeaParser;

public class GllMessage : BaseMessage
{
    public Coordinate Latitude { get; }
    public Coordinate Longitude { get; }
    public TimeOnly Time { get; }
    public bool Status { get; }
    public ModeIndicator Mode { get; }

    public GllMessage(string body) : base(body)
    {
        // Check number of arguments
        if (Fields.Length > 7)
            throw new ArgumentOutOfRangeException($"Too many arguments: {Fields.Length}");
        if (Fields.Length < 7)
            throw new ArgumentOutOfRangeException($"Too few arguments: {Fields.Length}");

        // Latitude
        if (!Decimal.TryParse(Fields[0], CultureInfo.InvariantCulture, out decimal rawLatitude))
            throw new ArgumentException($"Invalid latitude value: {Fields[0]}");
        // Degrees
        decimal rawLatitudeDegrees = Math.Floor(rawLatitude / 100);
        if (rawLatitudeDegrees > 90)
            throw new ArgumentOutOfRangeException($"Too high latitude degrees value: {rawLatitudeDegrees}");
        if (rawLatitudeDegrees < 0)
            throw new ArgumentOutOfRangeException($"Too low latitude degrees value: {rawLatitudeDegrees}");
        byte latitudeDegrees = (byte)rawLatitudeDegrees;
        // Minutes
        decimal latitudeMinutes = rawLatitude - latitudeDegrees * 100;
        if (latitudeMinutes >= 60)
            throw new ArgumentOutOfRangeException($"Too high latitude minutes value: {latitudeMinutes}");
        // Direction
        bool latitudeDirection = false;
        if (Fields[1].ToUpper() == "N")
            latitudeDirection = true;
        else if (Fields[1].ToUpper() == "S")
            latitudeDirection = false;
        else
            throw new ArgumentException($"Invalid latitude direction: {Fields[1]}");

        // Longitude
        if (!Decimal.TryParse(Fields[2], CultureInfo.InvariantCulture, out decimal rawLongitude))
            throw new ArgumentException($"Invalid latitude value: {Fields[2]}");
        // Degrees
        decimal rawLongitudeDegrees = Math.Floor(rawLongitude / 100);
        if (rawLongitudeDegrees > 180)
            throw new ArgumentOutOfRangeException($"Too high longitude degrees value: {rawLongitudeDegrees}");
        if (rawLongitudeDegrees < 0)
            throw new ArgumentOutOfRangeException($"Too low longitude degrees value: {rawLongitudeDegrees}");
        byte longitudeDegrees = (byte)rawLongitudeDegrees;
        // Minutes
        decimal longitudeMinutes = rawLongitude - longitudeDegrees * 100;
        if (longitudeMinutes >= 60)
            throw new ArgumentOutOfRangeException($"Too high longitude minutes value: {longitudeMinutes}");
        // Direction
        bool longitudeDirection = false;
        if (Fields[3].ToUpper() == "W")
            longitudeDirection = true;
        else if (Fields[3].ToUpper() == "E")
            longitudeDirection = false;
        else
            throw new ArgumentException($"Invalid longitude direction: {Fields[3]}");

        // Time
        if (!Decimal.TryParse(Fields[4], CultureInfo.InvariantCulture, out decimal rawTime))
            throw new ArgumentException($"Invalid time value: {Fields[4]}");
        // Hours
        decimal rawHours = Math.Floor(rawTime / 10000);
        if (rawHours > 23)
            throw new ArgumentOutOfRangeException($"Too high hours value: {rawHours}");
        if (rawHours < 0)
            throw new ArgumentOutOfRangeException($"Too low hours value: {rawHours}");
        int hours = (int)rawHours;
        // Minutes
        decimal rawMinutes = Math.Floor((rawTime - hours * 10000) / 100);
        if (rawMinutes > 59)
            throw new ArgumentOutOfRangeException($"Too high minutes value: {rawMinutes}");
        int minutes = (int)rawMinutes;
        // Seconds
        decimal rawSeconds = Math.Floor(rawTime - hours * 10000 - minutes * 100);
        if (rawSeconds > 59)
            throw new ArgumentOutOfRangeException($"Too high seconds value: {rawSeconds}");
        int seconds = (int)rawSeconds;
        // Milliseconds
        int milliseconds = (int)((rawTime - hours * 10000 - minutes * 100 - seconds) * 1000);

        // Status
        bool status;
        if (Fields[5].ToUpper() == "A")
            status = true;
        else if (Fields[5].ToUpper() == "V")
            status = false;
        else
            throw new ArgumentException($"Invalid status descriptor: {Fields[5]}");

        // Mode
        string rawMode = Fields[6].Substring(0, 1);
        if (!Enum.TryParse(rawMode, out ModeIndicator mode))
            throw new ArgumentException($"Invalid mode indicator: {rawMode}");

        // Assignement
        this.Latitude = new Coordinate(latitudeDegrees, latitudeMinutes, latitudeDirection);
        this.Longitude = new Coordinate(longitudeDegrees, longitudeMinutes, longitudeDirection);
        this.Time = new TimeOnly(hours, minutes, seconds, milliseconds);
        this.Status = status;
        this.Mode = mode;
    }

    public record Coordinate(byte Degrees, decimal Minutes, bool Direction);

    public enum ModeIndicator { A, D, E, M, S, N }
};
