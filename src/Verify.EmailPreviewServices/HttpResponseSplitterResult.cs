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
        var targets = new List<Target>();
        var preview = await service.ExecutePreviewAsync(
            new()
            {
                Name = "temp",
                Body = instance.Html,
                Subject = "subject",
                Devices = ["microsoft_outlook_2016"],
            });

        var previews = await GetDevicePreviews(preview);

        foreach (var devicePreview in previews)
        {
            await using var stream = await httpClient.GetStreamAsync(devicePreview.Preview.Original);
            var memoryStream = await PreviewScrubber.Outlook2016(stream);
            targets.Add(new("png", memoryStream, "Outlook2016"));
        }

        await service.DeletePreviewAsync(preview.Id);
        return new(instance, targets);
    }

    static async Task<ICollection<DevicePreviewData>> GetDevicePreviews(EmailPreviewData preview)
    {
        const int maxAttempts = 30;
        var delay = TimeSpan.FromSeconds(2);

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