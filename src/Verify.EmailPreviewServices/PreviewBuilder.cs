static class PreviewBuilder
{
    internal static readonly HttpClient HttpClient;
    internal static readonly EmailPreviewServicesClient Service;

    static PreviewBuilder()
    {
        var apiKey = VerifyEmailPreviewServices.ApiKey;
        HttpClient = new();
        HttpClient.DefaultRequestHeaders.Add("X-API-Key", apiKey);
        HttpClient.DefaultRequestHeaders.Authorization = new("Bearer", apiKey);
        HttpClient.DefaultRequestHeaders.Accept.Add(new("application/json"));
        Service = new("https://app.emailpreviewservices.com/api", HttpClient);
    }

    public static async Task<ConversionResult> Convert(EmailPreview instance)
    {
        ThrowIfDuplictes(instance.Devices);
        var targets = new List<Target>();
        var preview = await Service.ExecutePreviewAsync(
            new()
            {
                Name = "temp",
                Body = instance.Html,
                Subject = "subject",
                Devices = instance.Devices.Select(Devices.KeyForDevice).ToList(),
            });

        var previews = await GetDevicePreviews(preview);

        foreach (var devicePreview in previews.Previews)
        {
            await using var httpStream = await HttpClient.GetStreamAsync(devicePreview.Preview.Original);
            var device = Devices.DeviceForKey(devicePreview.DeviceKey);
            var memoryStream = await PreviewScrubber.Scrub(httpStream, device);
            targets.Add(new("jpg", memoryStream, device.ToString()));
        }

        await Service.DeletePreviewAsync(preview.Id);
        return new(null, targets);
    }

    static void ThrowIfDuplictes(ICollection<Device> devices)
    {
        if (devices.Count != devices.Distinct().Count())
        {
            throw new InvalidOperationException("Duplicate devices found.");
        }
    }

    static async Task<EmailPreviewData> GetDevicePreviews(EmailPreviewData preview)
    {
        const int maxAttempts = 100;
        var delay = TimeSpan.FromSeconds(1);

        for (var i = 0; i < maxAttempts; i++)
        {
            var previews = await Service.GetPreviewAsync(preview.Id);

            if (previews.Previews.All(_ => _.Status == DevicePreviewDataStatus.SUCCESSFUL))
            {
                return previews;
            }

            await Task.Delay(delay);
        }

        throw new("Timed out");
    }
}