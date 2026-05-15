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
        // Wind Angle
        decimal windAngle = Convert.ToDecimal(Fields[0], CultureInfo.InvariantCulture);

        // Reference
        bool reference = false;

        if (Fields[1].ToUpper() == "R")
            reference = true;
        else if (Fields[1].ToUpper() == "T")
            reference = false;
        else
            throw new NotSupportedException($"Invalid reference: {Fields[1]}");

        // Wind Speed
        decimal windSpeed = Convert.ToDecimal(Fields[2], CultureInfo.InvariantCulture);

        // Unit
        string rawUnit = Fields[3].Substring(0, 1);
        if (!Enum.TryParse(rawUnit, out WindSpeedUnit unit))
            throw new NotSupportedException($"Invalid unit: {rawUnit}");

        // Status
        bool status;
        if (Fields[4].ToUpper() == "A")
            status = true;
        else if (Fields[4].ToUpper() == "V")
            status = false;
        else
            throw new ArgumentException($"Invalid status descriptor: {Fields[4]}");

        // Assignement
        this.WindAngle = windAngle;
        this.Reference = reference;
        this.WindSpeed = windSpeed;
        this.Unit = unit;
        this.Status = status;
    }

    public enum WindSpeedUnit { K, M, N, S }
};
