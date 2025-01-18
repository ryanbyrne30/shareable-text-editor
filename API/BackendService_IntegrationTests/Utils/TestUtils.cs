namespace BackendService_IntegrationTests.Utils;

public static class TestUtils
{
    public static void AssertDatesAreEqual(DateTime? expected, DateTime? actual)
    {
        if (expected == null && actual == null)
        {
            Assert.Pass();
            return;
        }
        if (expected == null || actual == null)
        {
            Assert.Fail("One date is null, the other is not.");
            return;
        }
        Assert.Multiple(() =>
        {
            Assert.That(actual.Value.Year, Is.EqualTo(expected.Value.Year));
            Assert.That(actual.Value.Month, Is.EqualTo(expected.Value.Month));
            Assert.That(actual.Value.Day, Is.EqualTo(expected.Value.Day));
            Assert.That(actual.Value.Hour, Is.EqualTo(expected.Value.Hour));
            Assert.That(actual.Value.Minute, Is.EqualTo(expected.Value.Minute));
            Assert.That(actual.Value.Second, Is.EqualTo(expected.Value.Second));
            Assert.That(actual.Value.Millisecond, Is.EqualTo(expected.Value.Millisecond));
        });
    }
}