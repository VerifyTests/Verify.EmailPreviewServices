namespace VerifyTests;

public class EmailPreview
{
    public required string Html { get; init; }
    public required ICollection<Device> Devices { get; init; }
}