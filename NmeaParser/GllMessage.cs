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
        // Latitude
        decimal rawLatitude = Convert.ToDecimal(Fields[0], CultureInfo.InvariantCulture);
        byte latitudeDegrees = (byte)Math.Floor(rawLatitude / 100);
        decimal latitudeMinutes = rawLatitude - latitudeDegrees * 100;
        bool latitudeDirection = false;

        if (Fields[1].ToUpper() == "N")
            latitudeDirection = true;
        else if (Fields[1].ToUpper() == "S")
            latitudeDirection = false;
        else
            throw new NotSupportedException($"Invalid latitude direction: {Fields[1]}");

        // Longitude
        decimal rawLongitude = Convert.ToDecimal(Fields[2], CultureInfo.InvariantCulture);
        byte longitudeDegrees = (byte)Math.Floor(rawLongitude / 100);
        decimal longitudeMinutes = rawLongitude - longitudeDegrees * 100;
        bool longitudeDirection = false;

        if (Fields[3].ToUpper() == "W")
            longitudeDirection = true;
        else if (Fields[3].ToUpper() == "E")
            longitudeDirection = false;
        else
            throw new NotSupportedException($"Invalid longitude direction: {Fields[3]}");

        // Time
        decimal rawTime = Convert.ToDecimal(Fields[4], CultureInfo.InvariantCulture);
        int hours = (int)Math.Floor(rawTime / 10000);
        int minutes = (int)Math.Floor((rawTime - hours * 10000) / 100);
        int seconds = (int)Math.Floor(rawTime - hours * 10000 - minutes * 100);
        int milliseconds = (int)(rawTime - hours * 10000 - minutes * 100 - seconds) * 1000;

        // Status
        bool status;
        if (Fields[5].ToUpper() == "A")
            status = true;
        else if (Fields[5].ToUpper() == "V")
            status = false;
        else
            throw new NotSupportedException($"Invalid status descriptor: {Fields[5]}");

        // Mode
        string rawMode = Fields[6].Substring(0, 1);
        if (!Enum.TryParse(rawMode, out ModeIndicator mode))
            throw new NotSupportedException($"Invalid mode indicator: {rawMode}");

        this.Latitude = new Coordinate(latitudeDegrees, latitudeMinutes, latitudeDirection);
        this.Longitude = new Coordinate(longitudeDegrees, longitudeMinutes, longitudeDirection);
        this.Time = new TimeOnly(hours, minutes, seconds, milliseconds);
        this.Status = status;
        this.Mode = mode;
    }

    public record Coordinate(byte Degrees, decimal Minutes, bool Direction);

    public enum ModeIndicator { A, D, E, M, S, N }
};
