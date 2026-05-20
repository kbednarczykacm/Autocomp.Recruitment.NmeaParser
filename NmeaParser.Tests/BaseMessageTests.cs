namespace NmeaParser.Tests;

[TestClass]
public class BaseMessageTests
{
    private class FakeMessage : BaseMessage
    {
        public FakeMessage(string body) : base(body) { }
    }

    [TestMethod]
    public void Constructor_ValidMessage_SetProperties()
    {
        string msg = "$LCGLL,4728.31,N,12254.25,W,091342,A,A*4C\r\n";

        FakeMessage message = new FakeMessage(msg);

        Assert.AreEqual("LC", message.TalkerId);
        Assert.AreEqual("GLL", message.MessageType);
        Assert.AreEqual(76, message.Checksum);
        Assert.AreEqual("LCGLL,4728.31,N,12254.25,W,091342,A,A", message.Message);
        Assert.IsTrue(message.ChecksumOk);
    }

    [TestMethod]
    public void Constructor_InvalidHeader_Throws()
    {
        string invalidMsg = "$LGL,4728.31,N,12254.25,W,091342,A,A*4C\r\n";

        Assert.ThrowsExactly<ArgumentException>(() => new FakeMessage(invalidMsg));
    }

    [TestMethod]
    public void Constructor_InvalidMessage_Throws()
    {
        string invalidMsg = "$LCGLL";

        Assert.ThrowsExactly<ArgumentException>(() => new FakeMessage(invalidMsg));
    }

    [TestMethod]
    public void Constructor_InvalidFooter_Throws()
    {
        string invalidMsg = "$LCGLL,4728.31,N,12254.25,W,091342,A,A\r\n";

        Assert.ThrowsExactly<ArgumentException>(() => new FakeMessage(invalidMsg));
    }

    [TestMethod]
    public void Constructor_ChecksumTooShort_Throws()
    {
        string invalidMsg = "$LCGLL,4728.31,N,12254.25,W,091342,A,A*1\r\n";

        Assert.ThrowsExactly<ArgumentException>(() => new FakeMessage(invalidMsg));
    }

    [TestMethod]
    public void Constructor_ChecksumNotHex_Throws()
    {
        string invalidMsg = "$LCGLL,4728.31,N,12254.25,W,091342,A,A*ZZ\r\n";

        Assert.ThrowsExactly<ArgumentException>(() => new FakeMessage(invalidMsg));
    }

    [TestMethod]
    public void CreateMessage_ValidGLL_ReturnsGllMessage()
    {
        string msg = "$LCGLL,4728.31,N,12254.25,W,091342,A,A*4C\r\n";

        BaseMessage message = BaseMessage.CreateMessage(msg);

        Assert.IsInstanceOfType(message, typeof(GllMessage));
    }

    [TestMethod]
    public void CreateMessage_ValidMWV_ReturnsMwvMessage()
    {
        string msg = "$LCMWV,123,R,10.5,N,A*4C\r\n";

        BaseMessage message = BaseMessage.CreateMessage(msg);

        Assert.IsInstanceOfType(message, typeof(MwvMessage));
    }

    [TestMethod]
    public void CreateMessage_UnsupportedMessageType_ThrowsNotSupportedException()
    {
        string msg = "$LCXYZ,4728.31,N,12254.25,W,091342,A,A*4C\r\n";

        Assert.ThrowsExactly<NotSupportedException>(() => BaseMessage.CreateMessage(msg));
    }

    [TestMethod]
    public void CreateMessage_EmptyMessage_ThrowsArgumentException()
    {
        string msg = "";

        Assert.ThrowsExactly<ArgumentException>(() => BaseMessage.CreateMessage(msg));
    }

    [TestMethod]
    public void CreateMessage_NullMessage_ThrowsArgumentException()
    {
        string msg = null;

        Assert.ThrowsExactly<ArgumentException>(() => BaseMessage.CreateMessage(msg));
    }

    [TestMethod]
    public void CreateMessage_InvalidFormat_ThrowsArgumentException()
    {
        string msg = "LCGLL,4728.31,N,12254.25,W,091342,A,A*4C\r\n";

        Assert.ThrowsExactly<ArgumentException>(() => BaseMessage.CreateMessage(msg));
    }

    [TestMethod]
    public void CreateMessage_InvalidHeader_ThrowsArgumentException()
    {
        string msg = "$LGL,4728.31,N,12254.25,W,091342,A,A*4C\r\n";

        Assert.ThrowsExactly<ArgumentException>(() => BaseMessage.CreateMessage(msg));
    }
}