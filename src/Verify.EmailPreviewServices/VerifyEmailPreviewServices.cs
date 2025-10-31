namespace VerifyTests;

public static class VerifyEmailPreviewServices
{
    public static bool Initialized { get; private set; }

    public static void Initialize(string? apiKey = null)
    {
        if (Initialized)
        {
            throw new("Already Initialized");
        }

        Initialized = true;
        VerifyEmailPreviewServices.apiKey = apiKey;
        InnerVerifier.ThrowIfVerifyHasBeenRun();
        VerifierSettings.RegisterFileConverter<EmailPreview>(
            (instance, _) => PreviewBuilder.Convert(instance));
    }

    static string? apiKey;

    internal static string ApiKey
    {
        get
        {
            if (apiKey == null)
            {
                var envKey = Environment.GetEnvironmentVariable("EmailPreviewServicesApiKey");
                apiKey = envKey ?? throw new("Provide an apiKey via VerifyEmailPreviewServices.Initialize(apiKey) or an EmailPreviewServicesApiKey environment variable.");
            }

            return apiKey;
        }
    }
}