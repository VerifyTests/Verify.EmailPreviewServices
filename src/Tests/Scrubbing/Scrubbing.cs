[TestFixture]
public class Scrubbing
{
    static readonly string path;

    static Scrubbing()
    {
        var project = ProjectFiles.ProjectDirectory;
        path = Path.Combine(project, "Scrubbing");
    }
    // [Test]
    // public void Foo([Values] Device device)
    // {
    //     var combine = Path.Combine(path, $"{device}.png");
    //     if (File.Exists(combine))
    //     {
    //         File.Move(combine,combine.Replace(".png",".jpg"));
    //     }
    // }

    [Test]
    public async Task ScrubTesting([Values] Device device)
    {
        var fileStream = File.OpenRead(Path.Combine(path, $"{device}.jpg"));
        var scrubbed = await Scrubber.Scrub(fileStream, device);
        await Verify(scrubbed, extension: "webp");
    }
}