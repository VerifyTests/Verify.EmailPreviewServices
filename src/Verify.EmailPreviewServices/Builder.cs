static class Builder
{
    internal static readonly HttpClient HttpClient;
    internal static readonly EmailPreviewServicesClient Service;

    static Builder()
    {
        var apiKey = VerifyEmailPreviewServices.ApiKey;
        HttpClient = new()
        {
            Timeout = TimeSpan.FromMinutes(10)
        };
        HttpClient.DefaultRequestHeaders.Add("X-API-Key", apiKey);
        HttpClient.DefaultRequestHeaders.Authorization = new("Bearer", apiKey);
        HttpClient.DefaultRequestHeaders.Accept.Add(new("application/json"));
        Service = new("https://app.emailpreviewservices.com/api", HttpClient);
    }

    public static async Task<ConversionResult> Convert(EmailPreview instance)
    {
        ThrowIfDuplictes(instance.Devices);
        var targets = new List<Target>();
        var preview = await CreatePreview(instance);
        try
        {
            await GetDevicePreviews(preview);

            await AddTargets(preview, targets);
        }
        finally
        {
            await Service.DeletePreviewAsync(preview.Id);
        }

        return new(null, targets);
    }

    static Task<EmailPreviewData> CreatePreview(EmailPreview instance) =>
        Service.ExecutePreviewAsync(
            new()
            {
                Name = "temp",
                Body = instance.Html,
                Subject = "subject",
                Devices = instance.Devices.Select(Devices.KeyForDevice).ToList(),
            });

    static async Task AddTargets(EmailPreviewData preview, List<Target> targets)
    {
        using var fileResponse = await Service.GetPreviewZipAsync(preview.Id);
        using var zipArchive = new ZipArchive(fileResponse.Stream, ZipArchiveMode.Read);
        foreach (var entry in zipArchive.Entries)
        {
            var device = Devices.DeviceForKey(Path.GetFileNameWithoutExtension(entry.Name));
            var zipStream = await entry.OpenAsync();
            var memoryStream = await Scrubber.Scrub(zipStream, device);
            targets.Add(new("webp", memoryStream, device.ToString()));
        }
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
        const int maxAttempts = 50;

        await Task.Delay(1000);
        for (var i = 0; i < maxAttempts; i++)
        {
            var previews = await Service.GetPreviewAsync(preview.Id);

            var failed = previews.Previews.SingleOrDefault(_ => _.Status == DevicePreviewDataStatus.FAILED);
            if (failed != null)
            {
                throw new($"Preview failed to generate. DeviceKey: {failed.DeviceKey}");
            }

            if (previews.Previews.All(_ => _.Status == DevicePreviewDataStatus.SUCCESSFUL))
            {
                return;
            }

            await BackoffDelay(i);
        }

        throw new("Timed out");
    }

    static Task BackoffDelay(int attempt)
    {
        const double multiplier = 1.5;
        const int maxDelaySeconds = 8;

        var exponentialDelay = Math.Pow(multiplier, attempt);
        var cappedDelay = Math.Min(exponentialDelay, maxDelaySeconds);

        return Task.Delay(TimeSpan.FromSeconds(cappedDelay));
    }
}