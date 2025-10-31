static class Builder
{
    internal static readonly HttpClient HttpClient;
    internal static readonly EmailPreviewServicesClient Service;

    static Builder()
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

        await GetDevicePreviews(preview);

        using var zip = await Service.GetPreviewZipAsync(preview.Id);

        using var zipArchive = new ZipArchive(zip.Stream, ZipArchiveMode.Read);
        foreach (var entry in zipArchive.Entries)
        {
            var device = Devices.DeviceForKey(Path.GetFileNameWithoutExtension(entry.Name));
            var zipStream = await entry.OpenAsync();
            var memoryStream = await Scrubber.Scrub(zipStream, device);
            targets.Add(new("webp", memoryStream, device.ToString()));
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

    static async Task GetDevicePreviews(EmailPreviewData preview)
    {
        const int maxAttempts = 100;
        var delay = TimeSpan.FromSeconds(1);

        for (var i = 0; i < maxAttempts; i++)
        {
            var previews = await Service.GetPreviewAsync(preview.Id);

            if (previews.Previews.All(_ => _.Status == DevicePreviewDataStatus.SUCCESSFUL))
            {
                return;
            }

            await Task.Delay(delay);
        }

        throw new("Timed out");
    }
}