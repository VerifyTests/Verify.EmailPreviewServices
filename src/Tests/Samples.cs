[TestFixture]
public class Samples
{
    static string html =
        """
        <!DOCTYPE html>
        <html>
        <head>
           <meta charset="UTF-8">
           <meta name="viewport" content="width=device-width, initial-scale=1.0">
           <title>Test Email</title>
        </head>
        <body style="font-family: Arial, sans-serif; background-color: #f4f4f4; margin: 0; padding: 20px;">
           <div style="max-width: 600px; margin: 0 auto; background-color: white; padding: 20px; border-radius: 10px;">
               <h1 style="color: #333;">Welcome to Our Service!</h1>
               <p style="color: #666; line-height: 1.6;">
                   This is a test email to demonstrate the email preview functionality.
               </p>
               <a href="https://"
                  style="display: inline-block; padding: 10px 20px; background-color: #007bff;
                         color: white; text-decoration: none; border-radius: 5px; margin-top: 10px;">
                   Get Started
               </a>
           </div>
        </body>
        </html>
        """;

    [Test]
    [Explicit]
    public async Task BuildDeviceEnum()
    {
        var devices = await PreviewBuilder.Service.GetDeviceListAsync();
        var stringBuilder = new StringBuilder();
        foreach (var device in devices)
        {
            var name = device.Name
                .Replace("Seznam.cz", "Seznam")
                .Replace("Freenet.de", "Freenet")
                .Replace("Microsoft Outlook", "Outlook")
                .Replace(")", "")
                .Replace(".", "")
                .Replace("\"", "")
                .Replace(" ", "")
                .Replace("(", "")
                .Replace("Outlook.com", "OutlookWeb");
            stringBuilder.AppendLine(
                $"""

                 [Description("{device.DeviceKey}")]
                 {name},
                 """);
        }

        await Verify(stringBuilder);
    }

    [Test]
    [Explicit]
    public async Task EmailList()
    {
        var devices = await PreviewBuilder.Service.GetEmailsAsync();
        await Verify(devices);
    }

    // [Test]
    // [Explicit]
    // public async Task GenerateAll([Values] Device device)
    // {
    //     var preview = new EmailPreview
    //     {
    //         Html = html,
    //         Devices = [device]
    //     };
    //     await Verify(preview);
    // }

    [Test]
    [Explicit]
    public async Task GeneratePreview()
    {
        var preview = new EmailPreview
        {
            Html = html,
            Devices = [Device.Outlook2016, Device.iPhone12Pro]
        };
        await Verify(preview);
    }
}