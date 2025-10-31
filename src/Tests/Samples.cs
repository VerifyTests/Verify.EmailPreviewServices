using EmailPreviewServices;

[TestFixture]
public class Samples
{
    static readonly HttpClient httpClient;
    static readonly EmailPreviewServicesClient service;

    static Samples()
    {
        var apiKey = Environment.GetEnvironmentVariable("EmailPreviewServicesApiKey")!;
        httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Add("X-API-Key", apiKey);
        httpClient.DefaultRequestHeaders.Authorization = new("Bearer", apiKey);
        httpClient.DefaultRequestHeaders.Accept.Add(new("application/json"));
        service = new("https://app.emailpreviewservices.com/api", httpClient);
    }

    [Test]
    public async Task DeviceList()
    {
        var devices = await service.GetDeviceListAsync();
        await Verify(devices);
    }

    [Test]
    [Explicit]
    public async Task Simple()
    {
        var html =
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
                   <a href="https://example.com"
                      style="display: inline-block; padding: 10px 20px; background-color: #007bff;
                             color: white; text-decoration: none; border-radius: 5px; margin-top: 10px;">
                       Get Started
                   </a>
                   <p style="color: #999; font-size: 12px; margin-top: 30px;">
                       © 2025 Your Company. All rights reserved.
                   </p>
               </div>
            </body>
            </html>
            """;

        var preview = await service.ExecutePreviewAsync(
            new()
            {
                Name = "the name",
                Body = html,
                Subject = "subhect",
                Devices = ["microsoft_outlook_2016"],
            });
        const int maxAttempts = 30;
        var delay = TimeSpan.FromSeconds(2);

        Stream streamAsync = null!;
        for (var i = 0; i < maxAttempts; i++)
        {
            var devicesPreview = await service.GetDevicePreviewAsync(preview.Id, "microsoft_outlook_2016");

            if (devicesPreview.Status == DevicePreviewDataStatus.SUCCESSFUL)
            {
                streamAsync = await httpClient.GetStreamAsync(devicesPreview.Preview.Original);
                break;
            }

            await Task.Delay(delay);
        }

        await service.DeletePreviewAsync(preview.Id);
        await Verify(streamAsync, extension: "png");
        // await using var streamAsync = await httpClient.GetStreamAsync(preview.Body);
        // await using var fileStream = File.Create("temp.html");
        // await streamAsync.CopyToAsync(fileStream);
    }
}