public static class ModuleInitializer
{
    #region Initialize

    [ModuleInitializer]
    public static void Init() =>
        VerifyEmailPreviewServices.Initialize();

    #endregion

/**
    #region InitializeWithKey

    [ModuleInitializer]
    public static void Init() =>
        VerifyEmailPreviewServices.Initialize("ApiKey");

    #endregion
**/
    [ModuleInitializer]
    public static void InitOther()
    {
        VerifyDiffPlex.Initialize(OutputType.Compact);
        VerifierSettings.InitializePlugins();
        VerifyImageMagick.RegisterComparers(.01);
    }
}