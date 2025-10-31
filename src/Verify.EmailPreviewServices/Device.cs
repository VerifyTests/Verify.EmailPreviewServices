using System.ComponentModel;
// ReSharper disable InconsistentNaming

namespace VerifyTests;

public enum Device
{
    [Description("aol_chrome")]
    AOLChrome,

    [Description("microsoft_outlook_2016")]
    Outlook2016,

    [Description("iphone8_13")]
    iPhone8,

    [Description("outlook_chrome")]
    OutlookcomChrome,

    [Description("zimbra_desktop")]
    ZimbraDesktop,

    [Description("gmx")]
    GMX,

    [Description("microsoft_outlook_2010")]
    Outlook2010,

    [Description("microsoft_outlook_2013")]
    Outlook2013,

    [Description("outlook_firefox")]
    OutlookcomFirefox,

    [Description("microsoft_outlook_2007")]
    Outlook2007,

    [Description("yahoo_chrome")]
    YahooChrome,

    [Description("outlook_dark_chrome")]
    OutlookcomDarkModeChrome,

    [Description("gmail_firefox")]
    GmailFirefox,

    [Description("icloud")]
    iCloud,

    [Description("yahoo_firefox")]
    YahooFirefox,

    [Description("mailbird_dark")]
    Mailbirddark,

    [Description("aol_firefox")]
    AOLFirefox,

    [Description("o2_pl")]
    o2pl,

    [Description("em_client")]
    eMClient,

    [Description("postbox6")]
    Postbox6,

    [Description("wppl")]
    WPpl,

    [Description("yahoo_basic")]
    YahooBasic,

    [Description("mailbird_light")]
    Mailbirdlight,

    [Description("aol_basic")]
    AOLBasic,

    [Description("iphone_se")]
    iPhoneSE,

    [Description("onet_pl")]
    onetpl,

    [Description("outlook_dark_firefox")]
    OutlookcomDarkModeFirefox,

    [Description("seznam_cz")]
    Seznam,

    [Description("thunderbird")]
    Thunderbird,

    [Description("microsoft_outlook_2016_plain_text")]
    Outlook2016PlainText,

    [Description("windows_live_mail_2012")]
    WindowsLiveMail2012,

    [Description("win10mail_light")]
    Windows10MailLight,

    [Description("win10mail_dark")]
    Windows10MailDark,

    [Description("microsoft_outlook_2003")]
    Outlook2003,

    [Description("microsoft_outlook_2019")]
    Outlook2019,

    [Description("iphone11ios13")]
    iPhone11,

    [Description("iPad_Air_13")]
    iPadAir,

    [Description("android_9")]
    Android9,

    [Description("microsoft_outlook_2019_mac_dark")]
    Outlook2019MacDark,

    [Description("microsoft_outlook_2019_mac_light")]
    Outlook2019MacLight,

    [Description("apple_mail_dark")]
    AppleMailDark,

    [Description("apple_mail_light")]
    AppleMailLight,

    [Description("zoho_dark")]
    ZohoDark,

    [Description("zoho_light")]
    ZohoLight,

    [Description("office365_light")]
    Office365Light,

    [Description("office365_dark")]
    Office365Dark,

    [Description("freenetde")]
    Freenet,

    [Description("roundcube_chrome")]
    RoundcubeChrome,

    [Description("iPhone12")]
    iPhone12,

    [Description("iPhone13")]
    iPhone13,

    [Description("iPhone12Pro")]
    iPhone12Pro,

    [Description("iPhone13ProMax")]
    iPhone13ProMax,

    [Description("DevTesting")]
    DevTesting,
}