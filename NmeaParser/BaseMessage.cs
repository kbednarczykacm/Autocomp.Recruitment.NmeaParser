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
        TalkerId = Header.Substring(1, 2);
        MessageType = Header.Substring(3);
        Checksum = Convert.FromHexString(Fields[Fields.Length - 1].Split("*")[1].Substring(0, 2))[0];
        Message = body.Split("$")[1].Split("*")[0];
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
