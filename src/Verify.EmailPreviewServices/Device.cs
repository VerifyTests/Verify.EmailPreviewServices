using System.ComponentModel;
// ReSharper disable InconsistentNaming

namespace VerifyTests;

public enum Device
{
    [Description("android_9")]
    Android9,

    [Description("aol_basic")]
    AOLBasic,

    [Description("aol_chrome")]
    AOLChrome,

    [Description("aol_firefox")]
    AOLFirefox,

    [Description("apple_mail_dark")]
    AppleMailDark,

    [Description("apple_mail_light")]
    AppleMailLight,

    [Description("DevTesting")]
    DevTesting,

    [Description("em_client")]
    eMClient,

    [Description("freenetde")]
    Freenet,

    [Description("gmail_firefox")]
    GmailFirefox,

    [Description("gmx")]
    GMX,

    [Description("icloud")]
    iCloud,

    [Description("iPad_Air_13")]
    iPadAir,

    [Description("iphone11ios13")]
    iPhone11,

    [Description("iPhone12")]
    iPhone12,

    [Description("iPhone12Pro")]
    iPhone12Pro,

    [Description("iPhone13")]
    iPhone13,

    [Description("iPhone13ProMax")]
    iPhone13ProMax,

    [Description("iphone8_13")]
    iPhone8,

    [Description("iphone_se")]
    iPhoneSE,

    [Description("mailbird_dark")]
    Mailbirddark,

    [Description("mailbird_light")]
    Mailbirdlight,

    [Description("microsoft_outlook_2003")]
    Outlook2003,

    [Description("microsoft_outlook_2007")]
    Outlook2007,

    [Description("microsoft_outlook_2010")]
    Outlook2010,

    [Description("microsoft_outlook_2013")]
    Outlook2013,

    [Description("microsoft_outlook_2016")]
    Outlook2016,

    [Description("microsoft_outlook_2016_plain_text")]
    Outlook2016PlainText,

    [Description("microsoft_outlook_2019")]
    Outlook2019,

    [Description("microsoft_outlook_2019_mac_dark")]
    Outlook2019MacDark,

    [Description("microsoft_outlook_2019_mac_light")]
    Outlook2019MacLight,

    [Description("o2_pl")]
    o2pl,

    [Description("office365_dark")]
    Office365Dark,

    [Description("office365_light")]
    Office365Light,

    [Description("onet_pl")]
    onetpl,

    [Description("outlook_dark_chrome")]
    OutlookWebDarkModeChrome,

    [Description("outlook_dark_firefox")]
    OutlookWebDarkModeFirefox,

    [Description("outlook_chrome")]
    OutlookWebChrome,

    [Description("outlook_firefox")]
    OutlookWebFirefox,

    [Description("postbox6")]
    Postbox6,

    [Description("roundcube_chrome")]
    RoundcubeChrome,

    [Description("seznam_cz")]
    Seznam,

    [Description("thunderbird")]
    Thunderbird,

    [Description("win10mail_dark")]
    Windows10MailDark,

    [Description("win10mail_light")]
    Windows10MailLight,

    [Description("windows_live_mail_2012")]
    WindowsLiveMail2012,

    [Description("wppl")]
    WPpl,

    [Description("yahoo_basic")]
    YahooBasic,

    [Description("yahoo_chrome")]
    YahooChrome,

    [Description("yahoo_firefox")]
    YahooFirefox,

    [Description("zimbra_desktop")]
    ZimbraDesktop,

    [Description("zoho_dark")]
    ZohoDark,

    [Description("zoho_light")]
    ZohoLight
}