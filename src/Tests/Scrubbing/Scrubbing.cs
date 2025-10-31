[TestFixture]
public class Scrubbing
{
    [Test]
    [Explicit]
    public void ScrubTesting()
    {
        var project = AttributeReader.GetProjectDirectory();
        var path = Path.Combine(project,"Scrubbing");
        foreach (var file in Directory.EnumerateFiles(path, "*.png"))
        {
            var fileName = Path.GetFileName(file);
            fileName = fileName.Split('#')[1].Replace(".verified","");
            var destFileName = Path.Combine(path, fileName);
            File.Move(file,destFileName);
        }
    }
}