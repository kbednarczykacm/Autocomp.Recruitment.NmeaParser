using System.Reflection.Metadata.Ecma335;
using System.Runtime.Intrinsics.Arm;
using Autocomp.Nmea.Common;

namespace NmeaParser;

public abstract class BaseMessage : NmeaMessage
{
    public string TalkerId { get; }
    public string MessageType { get; }
    public byte Checksum { get; }
    public string Message { get; }
    public bool ChecksumOk => Checksum == CRC(Message) ? true : false;

    public BaseMessage(string body) : base(body)
    {
        if (Header.Length != 6)
            throw new ArgumentException("Invalid NMEA header.");

        TalkerId = Header.Substring(1, 2);
        MessageType = Header.Substring(3);
        Checksum = Convert.FromHexString(Fields[Fields.Length - 1].Split(Format.Suffix)[1].Substring(0, 2))[0];
        Message = body.Split(Format.Prefix)[1].Split(Format.Suffix)[0];
    }

    public static BaseMessage CreateMessage(string rawMessage)
    {
        return CreateMessage(rawMessage, NmeaFormat.Default);
    }

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
