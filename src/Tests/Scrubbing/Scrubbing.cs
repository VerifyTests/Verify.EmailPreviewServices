[TestFixture]
public class Scrubbing
{
    static readonly string path;

    static Scrubbing()
    {
        var project = AttributeReader.GetProjectDirectory();
        path = Path.Combine(project, "Scrubbing");
    }

    [Test]
    public async Task ScrubTesting([Values] Device device)
    {
        var fileStream = File.OpenRead(Path.Combine(path, $"{device}.png"));
        await Verify(fileStream, extension: "png");
    }
}