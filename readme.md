# <img src="/src/icon.png" height="30px"> Verify.EmailPreviewServices

[![Discussions](https://img.shields.io/badge/Verify-Discussions-yellow?svg=true&label=)](https://github.com/orgs/VerifyTests/discussions)
[![Build status](https://ci.appveyor.com/api/projects/status/nwvywmfs2xb4tpsd?svg=true)](https://ci.appveyor.com/project/SimonCropp/Verify-Ulid)
[![NuGet Status](https://img.shields.io/nuget/v/Verify.EmailPreviewServices.svg)](https://www.nuget.org/packages/Verify.EmailPreviewServices/)

Extends [Verify](https://github.com/VerifyTests/Verify) to enable snapshotting of emails via [EmailPreviewServices](https://emailpreviewservices.com).<!-- singleLineInclude: intro. path: /docs/intro.include.md -->

The purpose of this project is provide faster feedback when using code to generate html emails.

[EmailPreviewServices is a paid service](https://emailpreviewservices.com/en/pricing), and a account is required to get an API key.

**See [Milestones](../../milestones?state=closed) for release notes.**


## Sponsors


### Entity Framework Extensions<!-- include: zzz. path: /docs/zzz.include.md -->

[Entity Framework Extensions](https://entityframework-extensions.net/?utm_source=simoncropp&utm_medium=Verify.EmailPreviewServices) is a major sponsor and is proud to contribute to the development this project.

[![Entity Framework Extensions](https://raw.githubusercontent.com/VerifyTests/Verify.EmailPreviewServices/refs/heads/main/docs/zzz.png)](https://entityframework-extensions.net/?utm_source=simoncropp&utm_medium=Verify.EmailPreviewServices)<!-- endInclude -->


## NuGet

 * https://nuget.org/packages/Verify.EmailPreviewServices


## Setup

Call `VerifyEmailPreviewServices.Initialize` at test startup.

<!-- snippet: Initialize -->
<a id='snippet-Initialize'></a>
```cs
[ModuleInitializer]
public static void Init() =>
    VerifyEmailPreviewServices.Initialize();
```
<sup><a href='/src/Tests/ModuleInitializer.cs#L3-L9' title='Snippet source file'>snippet source</a> | <a href='#snippet-Initialize' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->

The default behavior is to use an environment variable named `EmailPreviewServicesApiKey` as the API key.

An explicit API key can be used:

<!-- snippet: InitializeWithKey -->
<a id='snippet-InitializeWithKey'></a>
```cs
[ModuleInitializer]
public static void Init() =>
    VerifyEmailPreviewServices.Initialize("ApiKey");
```
<sup><a href='/src/Tests/ModuleInitializer.cs#L12-L18' title='Snippet source file'>snippet source</a> | <a href='#snippet-InitializeWithKey' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->


## Sample html

Assume the code under test produces the following html email.

snippet: html


## Generating previews

<!-- snippet: sample -->
<a id='snippet-sample'></a>
```cs
[Test]
[Explicit]
public async Task GeneratePreview()
{
    var preview = new EmailPreview
    {
        Html = html,
        Devices =
            [
                Device.Outlook2019,
            ]
    };
    await Verify(preview);
}
```
<sup><a href='/src/Tests/Samples.cs#L78-L95' title='Snippet source file'>snippet source</a> | <a href='#snippet-sample' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->

Result:

<img src="/src/Tests/Samples.GeneratePreview%23iPhone13.verified.webp" height="100px">

<img src="/src/Tests/Samples.GeneratePreview%23Outlook2019.verified.webp" height="100px">


## Performance

Generating previews takes in the range of tens of seconds. The time take can vary based on the number and type of email devices selected. For example:

 * Generating a a single device (GmailFirefox) takes ~20sec.
 * Generating previews for 5 devices (OutlookWebChrome, Outlook2019, Outlook2016, iPhone13, and GmailFirefox) takes ~25sec.

Execution will timeout after 6min and throw an exception.

Splitting individual devices, or groups of devices, over multiple tests can be used to have more control of when previews are generated and achieve faster feedback.


## When to run tests

Tests that execute in the 10s of seconds range can significantly slow down a test run. As such it is recommended that email preview tests are configured to be explicit in `Debug` mode.

```cs
[Test]
#if DEBUG
[Explicit]
#endif
public async Task GeneratePreview()
{
    var preview = new EmailPreview
    {
        Html = html,
        Devices = [Device.Outlook2019]
    };
    await Verify(preview);
}
```

This way developer can opt in to manually run specific previews, and on the build server previews test will allways be executed.


## Icon

[Preview pane](https://thenounproject.com/icon/preview-pane-5625474/) designed by [M. Oki Orlando](https://thenounproject.com/creator/orvipixel/) from [The Noun Project](https://thenounproject.com).


