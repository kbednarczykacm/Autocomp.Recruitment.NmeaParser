using System.Globalization;
using Autocomp.Nmea.Common;

namespace NmeaParser;

/// <summary>
/// Represents an MWV NMEA message, which contains information about the wind angle, reference, wind speed, unit of wind speed, and status of the data.
/// </summary>
public class MwvMessage : BaseMessage
{
    /// <summary>
    /// Defines the wind angle, which is represented as a decimal value indicating the angle of the wind relative to the vessel.
    /// </summary>
    public decimal WindAngle { get; }

    /// <summary>
    /// Indicates the reference for the wind angle, where 'R' is Relative and 'T' is Theoretical. 
    /// This property is represented as a boolean value, where true indicates Relative reference and false indicates Theoretical reference.
    /// </summary>
    public bool Reference { get; }

    /// <summary>
    /// Defines the wind speed, which is represented as a decimal value indicating the speed of the wind. The unit of wind speed is determined by the Unit property.
    /// </summary>
    public decimal WindSpeed { get; }

    /// <summary>
    /// Defines the unit of wind speed, which is represented as an enumeration (K for kilometers per hour, M for miles per hour, N for knots, and S for meters per second).
    /// </summary>
    public WindSpeedUnit Unit { get; }

    /// <summary>
    /// Indicates the status of the data, where 'A' means Data Valid and 'V' means Data Invalid. 
    /// This property is represented as a boolean value, where true indicates valid data and false indicates invalid data.
    /// </summary>
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

    /// <summary>
    /// Defines the unit of wind speed, which is represented as an enumeration (K for kilometers per hour, M for miles per hour, N for knots, and S for meters per second).
    /// </summary>
    public enum WindSpeedUnit { K, M, N, S }
};
