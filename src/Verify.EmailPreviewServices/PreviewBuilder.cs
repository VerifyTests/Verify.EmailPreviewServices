static class PreviewBuilder
{
    static readonly HttpClient httpClient;
    static readonly EmailPreviewServicesClient service;

    static PreviewBuilder()
    {
        var apiKey = VerifyEmailPreviewServices.ApiKey;
        httpClient = new();
        httpClient.DefaultRequestHeaders.Add("X-API-Key", apiKey);
        httpClient.DefaultRequestHeaders.Authorization = new("Bearer", apiKey);
        httpClient.DefaultRequestHeaders.Accept.Add(new("application/json"));
        service = new("https://app.emailpreviewservices.com/api", httpClient);
    }

    public static async Task<ConversionResult> Convert(EmailPreview instance)
    {
        ThrowIfDuplictes(instance.Devices);
        var targets = new List<Target>();
        var preview = await service.ExecutePreviewAsync(
            new()
            {
                Name = "temp",
                Body = instance.Html,
                Subject = "subject",
                Devices = instance.Devices.Select(Devices.KeyForDevice).ToList(),
            });

        var previews = await GetDevicePreviews(preview);

        foreach (var devicePreview in previews)
        {
            await using var httpStream = await httpClient.GetStreamAsync(devicePreview.Preview.Original);
            var device = Devices.DeviceForKey(devicePreview.DeviceKey);
            var memoryStream = await PreviewScrubber.Scrub(httpStream, device);
            targets.Add(new("png", memoryStream, device.ToString()));
        }

        await service.DeletePreviewAsync(preview.Id);
        return new(null, targets);
    }

    static void ThrowIfDuplictes(List<Device> devices)
    {
        if (devices.Count != devices.Distinct().Count())
        {
            throw new InvalidOperationException("Duplicate devices found.");
        }
    }

    static async Task<FileResponse> GetDevicePreviews(EmailPreviewData preview)
    {
        const int maxAttempts = 100;
        var delay = TimeSpan.FromSeconds(1);

        for (var i = 0; i < maxAttempts; i++)
        {
            var previews = await service.GetAllDevicesPreviewsAsync(preview.Id);

            if (previews.All(_ => _.Status == DevicePreviewDataStatus.SUCCESSFUL))
            {
                return previews;
            }

            await Task.Delay(delay);
        }

        throw new("Timed out");
    }
}