using System.Net.Http.Headers;
using EmailPreviewServices;

[TestFixture]
public class Samples
{
    static string? apiKey = Environment.GetEnvironmentVariable("EmailPreviewServicesApiKey");
    [Test]
    public async Task DeviceList()
    {
        var httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Add("X-API-Key", apiKey);
        httpClient.DefaultRequestHeaders.Authorization = new("Bearer", apiKey);
        httpClient.DefaultRequestHeaders.Accept.Add(new("application/json"));
        var service = new EmailPreviewServicesClient("https://app.emailpreviewservices.com/api", httpClient);
        var deviceListAsync = await service.GetDeviceListAsync();
        await Verify(deviceListAsync);
    }
    [Test]
    public async Task Simple()
    {
        // var html =
        //     """
        //     <!DOCTYPE html>
        //     <html>
        //     <head>
        //        <meta charset="UTF-8">
        //        <meta name="viewport" content="width=device-width, initial-scale=1.0">
        //        <title>Test Email</title>
        //     </head>
        //     <body style="font-family: Arial, sans-serif; background-color: #f4f4f4; margin: 0; padding: 20px;">
        //        <div style="max-width: 600px; margin: 0 auto; background-color: white; padding: 20px; border-radius: 10px;">
        //            <h1 style="color: #333;">Welcome to Our Service!</h1>
        //            <p style="color: #666; line-height: 1.6;">
        //                This is a test email to demonstrate the email preview functionality.
        //            </p>
        //            <a href="https://example.com"
        //               style="display: inline-block; padding: 10px 20px; background-color: #007bff;
        //                      color: white; text-decoration: none; border-radius: 5px; margin-top: 10px;">
        //                Get Started
        //            </a>
        //            <p style="color: #999; font-size: 12px; margin-top: 30px;">
        //                © 2025 Your Company. All rights reserved.
        //            </p>
        //        </div>
        //     </body>
        //     </html>
        //     """;

        var httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Add("X-API-Key", apiKey);
        var service = new EmailPreviewServicesClient("app.emailpreviewservices.com", httpClient);
        var deviceListAsync = await service.GetDeviceListAsync();
        await Verify(deviceListAsync);
    }
}