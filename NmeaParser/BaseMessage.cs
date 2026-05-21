using System.Reflection.Metadata.Ecma335;
using System.Runtime.Intrinsics.Arm;
using Autocomp.Nmea.Common;

namespace NmeaParser;

/// <summary>
/// Base NMEA message class. Contains common properties and methods for all types of messages.
/// </summary>
public abstract class BaseMessage : NmeaMessage
{
    /// <summary>
    /// Defines the talker ID (2 characters) of the NMEA message. 
    /// The talker ID identifies the source of the message and is extracted from the header of the NMEA message.
    /// </summary>
    public string TalkerId { get; }

    /// <summary>
    /// Defines the message type (3 characters) of the NMEA message. 
    /// The message type indicates the kind of data contained in the message and is extracted from the header of the NMEA message.
    /// </summary>
    public string MessageType { get; }

    /// <summary>
    /// Defines the checksum value of the NMEA message. 
    /// The checksum is used to verify the integrity of the message.
    /// The checksum is extracted from the NMEA message.
    /// </summary>
    public byte Checksum { get; }

    /// <summary>
    /// Defines the body of the NMEA message, which contains the actual data fields of the message.
    /// </summary>
    public string Message { get; }

    /// <summary>
    /// Indicates whether the checksum of the message is correct by comparing the extracted checksum with a calculated checksum based on the message content.
    /// </summary>
    public bool ChecksumOk => Checksum == CRC(Message) ? true : false;

    public BaseMessage(string body) : base(body)
    {
        if (Header.Length != 6)
            throw new ArgumentException("Invalid NMEA header.");

        TalkerId = Header.Substring(1, 2);
        MessageType = Header.Substring(3);

        if (Fields.Length < 1)
            throw new ArgumentException("Invalid NMEA message.");
        string rawFooter = Fields[Fields.Length - 1].Trim('\r', '\n');

        string[] footer = rawFooter.Split(Format.Suffix);
        if (footer.Length != 2)
            throw new ArgumentException($"Invalid NMEA message (no suffix \"{Format.Suffix} found\")");
        if (footer[1].Length < 2)
            throw new ArgumentException($"Invalid checksum");

        if (!Byte.TryParse(footer[1], System.Globalization.NumberStyles.HexNumber, null, out byte checksum))
            throw new ArgumentException($"Invalid checksum");

        Checksum = checksum;
        Message = body.Split(Format.Prefix)[1].Split(Format.Suffix)[0];
    }

    /// <summary>
    /// Factory method that creates an instance of a specific NMEA message type based on the raw message string and the default NMEA format.
    /// </summary>
    /// <inheritdoc cref="CreateMessage(string, NmeaFormat)" />
    public static BaseMessage CreateMessage(string rawMessage)
    {
        return CreateMessage(rawMessage, NmeaFormat.Default);
    }

    /// <summary>
    /// Factory method that creates an instance of a specific NMEA message type based on the raw message string and the provided NMEA format.
    /// </summary>
    /// <param name="rawMessage">The raw NMEA message string.</param>
    /// <param name="format">The NMEA format to use for parsing.</param>
    /// <returns>The created NMEA message instance inherited from BaseMessage.</returns>
    /// <exception cref="ArgumentException">Thrown when the raw message is invalid or does not match the expected format.</exception>
    /// <exception cref="NotSupportedException">Thrown when the message type is not supported.</exception>
    public static BaseMessage CreateMessage(string rawMessage, NmeaFormat format)
    {
        if (string.IsNullOrWhiteSpace(rawMessage) || !rawMessage.StartsWith(format.Prefix))
            throw new ArgumentException("Invalid NMEA message format");

        string[] fields = rawMessage.Split(format.Separator);
        string header = fields[0];

        if (header.Length != 6)
            throw new ArgumentException("Invalid NMEA header.");

        string messageType = header.Substring(header.Length - 3);

        BaseMessage message = messageType switch
        {
            "GLL" => new GllMessage(rawMessage),
            "MWV" => new MwvMessage(rawMessage),
            _ => throw new NotSupportedException($"Unsupported message type: {messageType}"),
        };

        return message;
    }

    /// <summary>
    /// Calculates the checksum of the NMEA message by performing a bitwise XOR operation on the message.
    /// </summary>
    /// <param name="msg">The NMEA message string.</param>
    /// <returns>The calculated checksum.</returns>
    byte CRC(string msg)
    {
        byte crc = 0;

        foreach (char character in msg)
        {
            crc ^= (byte)character;
        }

        return crc;
    }
};
