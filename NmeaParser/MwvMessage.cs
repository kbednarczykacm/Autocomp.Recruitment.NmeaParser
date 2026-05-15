using System.Globalization;
using Autocomp.Nmea.Common;

namespace NmeaParser;

public class MwvMessage : BaseMessage
{
    public decimal WindAngle { get; }
    public bool Reference { get; }
    public decimal WindSpeed { get; }
    public WindSpeedUnit Unit { get; }
    public bool Status { get; }

    public MwvMessage(string body) : base(body)
    {
        // Check number of arguments
        if (Fields.Length > 5)
            throw new ArgumentOutOfRangeException($"Too many arguments: {Fields.Length}");
        if (Fields.Length < 5)
            throw new ArgumentOutOfRangeException($"Too few arguments: {Fields.Length}");

        // Wind Angle
        if (!Decimal.TryParse(Fields[0], CultureInfo.InvariantCulture, out decimal windAngle))
            throw new ArgumentException($"Invalid wind angle value: {Fields[0]}");
        if (windAngle >= 360)
            throw new ArgumentOutOfRangeException($"Too high wind angle value: {windAngle}");
        if (windAngle < 0)
            throw new ArgumentOutOfRangeException($"Too low wind angle value: {windAngle}");

        // Reference
        bool reference = false;
        if (Fields[1].ToUpper() == "R")
            reference = true;
        else if (Fields[1].ToUpper() == "T")
            reference = false;
        else
            throw new ArgumentException($"Invalid reference: {Fields[1]}");

        // Wind Speed
        if (!Decimal.TryParse(Fields[2], CultureInfo.InvariantCulture, out decimal windSpeed))
            throw new ArgumentException($"Invalid wind speed value: {Fields[2]}");
        if (windSpeed < 0)
            throw new ArgumentOutOfRangeException($"Too low wind speed value: {windSpeed}");

        // Unit
        string rawUnit = Fields[3].Substring(0, 1);
        if (!Enum.TryParse(rawUnit, out WindSpeedUnit unit))
            throw new ArgumentException($"Invalid unit: {rawUnit}");

        // Status
        string rawStatus = Fields[4].Substring(0, 1);
        bool status;
        if (rawStatus.ToUpper() == "A")
            status = true;
        else if (rawStatus.ToUpper() == "V")
            status = false;
        else
            throw new ArgumentException($"Invalid status descriptor: {rawStatus}");

        // Assignement
        this.WindAngle = windAngle;
        this.Reference = reference;
        this.WindSpeed = windSpeed;
        this.Unit = unit;
        this.Status = status;
    }

    public enum WindSpeedUnit { K, M, N, S }
};
