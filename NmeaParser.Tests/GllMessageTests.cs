namespace NmeaParser.Tests;

[TestClass]
public class GllMessageTests
{
    [TestMethod]
    public void Constructor_ValidMessage_SetProperties()
    {
        string msg = "$LCGLL,4728.31,N,12254.25,W,091342,A,A*4C\r\n";

        GllMessage message = new GllMessage(msg);

        Assert.AreEqual(47, message.Latitude.Degrees);
        Assert.AreEqual(28.31m, message.Latitude.Minutes);
        Assert.IsTrue(message.Latitude.Direction);
        Assert.AreEqual(122, message.Longitude.Degrees);
        Assert.AreEqual(54.25m, message.Longitude.Minutes);
        Assert.IsTrue(message.Longitude.Direction);
        Assert.AreEqual(new TimeOnly(9, 13, 42), message.Time);
        Assert.IsTrue(message.Status);
        Assert.AreEqual(GllMessage.ModeIndicator.A, message.Mode);
    }

    [TestMethod]
    public void Constructor_TooManyArguments_Throws()
    {
        string invalidMsg = "$LCGLL,4728.31,N,12254.25,W,091342,A,A,A*4C\r\n";

        Assert.ThrowsExactly<ArgumentOutOfRangeException>(() => new GllMessage(invalidMsg));
    }

    [TestMethod]
    public void Constructor_TooFewArguments_Throws()
    {
        string invalidMsg = "$LCGLL,4728.31,N,12254.25,W,091342,A*4C\r\n";

        Assert.ThrowsExactly<ArgumentOutOfRangeException>(() => new GllMessage(invalidMsg));
    }

    [TestMethod]
    public void Constructor_InvalidLatitude_Throws()
    {
        string invalidMsg = "$LCGLL,1a23.45,N,12254.25,W,091342,A,A*4C\r\n";

        Assert.ThrowsExactly<ArgumentException>(() => new GllMessage(invalidMsg));
    }

    [TestMethod]
    public void Constructor_TooHighLatitudeDegrees_Throws()
    {
        string invalidMsg = "$LCGLL,9128.31,N,12254.25,W,091342,A,A*4C\r\n";

        Assert.ThrowsExactly<ArgumentOutOfRangeException>(() => new GllMessage(invalidMsg));
    }

    [TestMethod]
    public void Constructor_TooLowLatitudeDegrees_Throws()
    {
        string invalidMsg = "$LCGLL,-1128.31,N,12254.25,W,091342,A,A*4C\r\n";

        Assert.ThrowsExactly<ArgumentOutOfRangeException>(() => new GllMessage(invalidMsg));
    }

    [TestMethod]
    public void Constructor_TooHighLatitudeMinutes_Throws()
    {
        string invalidMsg = "$LCGLL,4760.31,N,12254.25,W,091342,A,A*4C\r\n";

        Assert.ThrowsExactly<ArgumentOutOfRangeException>(() => new GllMessage(invalidMsg));
    }

    [TestMethod]
    public void Constructor_InvalidLatitudeDirection_Throws()
    {
        string invalidMsg = "$LCGLL,4728.31,X,12254.25,W,091342,A,A*4C\r\n";

        Assert.ThrowsExactly<ArgumentException>(() => new GllMessage(invalidMsg));
    }

    [TestMethod]
    public void Constructor_InvalidLongitude_Throws()
    {
        string invalidMsg = "$LCGLL,4728.31,N,1a23.45,W,091342,A,A*4C\r\n";

        Assert.ThrowsExactly<ArgumentException>(() => new GllMessage(invalidMsg));
    }

    [TestMethod]
    public void Constructor_TooHighLongitudeDegrees_Throws()
    {
        string invalidMsg = "$LCGLL,4728.31,N,18254.25,W,091342,A,A*4C\r\n";

        Assert.ThrowsExactly<ArgumentOutOfRangeException>(() => new GllMessage(invalidMsg));
    }

    [TestMethod]
    public void Constructor_TooLowLongitudeDegrees_Throws()
    {
        string invalidMsg = "$LCGLL,4728.31,N,-12254.25,W,091342,A,A*4C\r\n";

        Assert.ThrowsExactly<ArgumentOutOfRangeException>(() => new GllMessage(invalidMsg));
    }

    [TestMethod]
    public void Constructor_TooHighLongitudeMinutes_Throws()
    {
        string invalidMsg = "$LCGLL,4728.31,N,12260.25,W,091342,A,A*4C\r\n";

        Assert.ThrowsExactly<ArgumentOutOfRangeException>(() => new GllMessage(invalidMsg));
    }

    [TestMethod]
    public void Constructor_InvalidLongitudeDirection_Throws()
    {
        string invalidMsg = "$LCGLL,4728.31,N,12254.25,X,091342,A,A*4C\r\n";

        Assert.ThrowsExactly<ArgumentException>(() => new GllMessage(invalidMsg));
    }

    [TestMethod]
    public void Constructor_InvalidTime_Throws()
    {
        string invalidMsg = "$LCGLL,4728.31,N,12254.25,W,1a1342,A,A*4C\r\n";

        Assert.ThrowsExactly<ArgumentException>(() => new GllMessage(invalidMsg));
    }

    [TestMethod]
    public void Constructor_TooHighHours_Throws()
    {
        string invalidMsg = "$LCGLL,4728.31,N,12254.25,W,241342,A,A*4C\r\n";

        Assert.ThrowsExactly<ArgumentOutOfRangeException>(() => new GllMessage(invalidMsg));
    }

    [TestMethod]
    public void Constructor_TooLowHours_Throws()
    {
        string invalidMsg = "$LCGLL,4728.31,N,12254.25,W,-101342,A,A*4C\r\n";

        Assert.ThrowsExactly<ArgumentOutOfRangeException>(() => new GllMessage(invalidMsg));
    }

    [TestMethod]
    public void Constructor_TooHighMinutes_Throws()
    {
        string invalidMsg = "$LCGLL,4728.31,N,12254.25,W,096042,A,A*4C\r\n";

        Assert.ThrowsExactly<ArgumentOutOfRangeException>(() => new GllMessage(invalidMsg));
    }

    [TestMethod]
    public void Constructor_TooHighSeconds_Throws()
    {
        string invalidMsg = "$LCGLL,4728.31,N,12254.25,W,091360,A,A*4C\r\n";

        Assert.ThrowsExactly<ArgumentOutOfRangeException>(() => new GllMessage(invalidMsg));
    }

    [TestMethod]
    public void Constructor_InvalidStatus_Throws()
    {
        string invalidMsg = "$LCGLL,4728.31,N,12254.25,W,091342,X,A*4C\r\n";

        Assert.ThrowsExactly<ArgumentException>(() => new GllMessage(invalidMsg));
    }

    [TestMethod]
    public void Constructor_InvalidMode_Throws()
    {
        string invalidMsg = "$LCGLL,4728.31,N,12254.25,W,091342,A,X*4C\r\n";

        Assert.ThrowsExactly<ArgumentException>(() => new GllMessage(invalidMsg));
    }

}