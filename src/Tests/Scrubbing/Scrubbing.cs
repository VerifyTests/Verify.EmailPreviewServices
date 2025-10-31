[TestFixture]
public class Scrubbing
{
    [Test]
    [Explicit]
    public async Task ScrubTesting()
    {
        var project = AttributeReader.GetProjectDirectory();
        var path = Path.Combine(project,"Scrubbing");
        foreach (var file in Directory.EnumerateFiles("*.png"))
        {
            var fileName = Path.GetFileName(file);
            fileName = fileName.Split('#')[1];
            File.Move(file,);
            var enumerateFiles =
                var preview = new EmailPreview
            {
                Html = html,
                Devices = [Device.Outlook2016, Device.iPhone12Pro]
            };
            await Verify(preview);
        }
    }
}