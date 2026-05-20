namespace NmeaParser.Tests;

[TestClass]
public class MwvMessageTests
{
    [TestMethod]
    public void Constructor_ValidMessage_SetProperties()
    {
        string msg = "$LIMWV,045.0,R,10.5,N,A*2B\r\n";

        MwvMessage message = new MwvMessage(msg);

        Assert.AreEqual(45.0m, message.WindAngle);
        Assert.IsTrue(message.Reference);
        Assert.AreEqual(10.5m, message.WindSpeed);
        Assert.AreEqual(MwvMessage.WindSpeedUnit.N, message.Unit);
        Assert.IsTrue(message.Status);
    }

    [TestMethod]
    public void Constructor_TooManyArguments_Throws()
    {
        string invalidMsg = "$LIMWV,045.0,R,10.5,N,A,A*2B\r\n";

        Assert.ThrowsExactly<ArgumentOutOfRangeException>(() => new MwvMessage(invalidMsg));
    }

    [TestMethod]
    public void Constructor_TooFewArguments_Throws()
    {
        string invalidMsg = "$LIMWV,045.0,R,10.5,N*2B\r\n";

        Assert.ThrowsExactly<ArgumentOutOfRangeException>(() => new MwvMessage(invalidMsg));
    }

    [TestMethod]
    public void Constructor_InvalidWindAngle_Throws()
    {
        string invalidMsg = "$LIMWV,abc,R,10.5,N,A*2B\r\n";

        Assert.ThrowsExactly<ArgumentException>(() => new MwvMessage(invalidMsg));
    }

    [TestMethod]
    public void Constructor_TooHighWindAngle_Throws()
    {
        string invalidMsg = "$LIMWV,360.0,R,10.5,N,A*2B\r\n";

        Assert.ThrowsExactly<ArgumentOutOfRangeException>(() => new MwvMessage(invalidMsg));
    }

    [TestMethod]
    public void Constructor_TooLowWindAngle_Throws()
    {
        string invalidMsg = "$LIMWV,-1.0,R,10.5,N,A*2B\r\n";

        Assert.ThrowsExactly<ArgumentOutOfRangeException>(() => new MwvMessage(invalidMsg));
    }

    [TestMethod]
    public void Constructor_InvalidReference_Throws()
    {
        string invalidMsg = "$LIMWV,045.0,X,10.5,N,A*2B\r\n";

        Assert.ThrowsExactly<ArgumentException>(() => new MwvMessage(invalidMsg));
    }

    [TestMethod]
    public void Constructor_InvalidWindSpeed_Throws()
    {
        string invalidMsg = "$LIMWV,045.0,R,abc,N,A*2B\r\n";

        Assert.ThrowsExactly<ArgumentException>(() => new MwvMessage(invalidMsg));
    }

    [TestMethod]
    public void Constructor_TooLowWindSpeed_Throws()
    {
        string invalidMsg = "$LIMWV,045.0,R,-1.0,N,A*2B\r\n";

        Assert.ThrowsExactly<ArgumentOutOfRangeException>(() => new MwvMessage(invalidMsg));
    }

    [TestMethod]
    public void Constructor_InvalidUnit_Throws()
    {
        string invalidMsg = "$LIMWV,045.0,R,10.5,X,A*2B\r\n";

        Assert.ThrowsExactly<ArgumentException>(() => new MwvMessage(invalidMsg));
    }

    [TestMethod]
    public void Constructor_InvalidStatus_Throws()
    {
        string invalidMsg = "$LIMWV,045.0,R,10.5,N,X*2B\r\n";

        Assert.ThrowsExactly<ArgumentException>(() => new MwvMessage(invalidMsg));
    }
}