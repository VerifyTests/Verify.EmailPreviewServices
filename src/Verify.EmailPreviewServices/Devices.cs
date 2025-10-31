using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace VerifyTests;

static class Devices
{
    static Dictionary<Device, string> deviceToKey = [];
    static Dictionary<Device, ScrubSpec> deviceScrubbing;
    static Dictionary<string, Device> keyToDevice = [];

    static Devices()
    {
        foreach (var device in Enum.GetValues<Device>())
        {
            var fieldInfo = device.GetType().GetField(device.ToString())!;
            var attribute = fieldInfo.GetCustomAttribute<DescriptionAttribute>()!;
            var key = attribute.Description;
            deviceToKey[device] = key;
            keyToDevice[key] = device;
        }

        deviceScrubbing = new()
        {
            [Device.Android9] = new()
            {
                Top = 550,
                Bottom = 261,
                Left = 21,
                Right = 21
            },
            [Device.AOLBasic] = new()
            {
                Top = 250,
                Bottom = 0,
                Left = 222,
                Right = 315,
                BottomTolerance = 10,
            },
            [Device.AOLChrome] = new()
            {
                Top = 250,
                Bottom = 50,
                Left = 225,
                Right = 360,
                BottomTolerance = 10,
            },
            [Device.AOLFirefox] = new()
            {
                Top = 250,
                Bottom = 50,
                Left = 225,
                Right = 360,
                BottomTolerance = 50,
            },
            [Device.AppleMailDark] = new()
            {
                Top = 142,
                Bottom = 10,
                Left = 10,
                Right = 10,
                BottomTolerance = 20,
            },
            [Device.AppleMailLight] = new()
            {
                Top = 142,
                Bottom = 10,
                Left = 10,
                Right = 10,
                BottomTolerance = 20,
            },
            [Device.eMClient] = new()
            {
                Top = 181,
                Bottom = 10,
                Left = 10,
                Right = 10,
                BottomTolerance = 10,
            },
            [Device.Freenet] = new()
            {
                Top = 401,
                Bottom = 20,
                Left = 332,
                Right = 222,
                BottomTolerance = 20,
            },
            [Device.GmailFirefox] = new()
            {
                Top = 270,
                Bottom = 16,
                Left = 256,
                Right = 73,
                BottomTolerance = 10,
            },
            [Device.GMX] = new()
            {
                Top = 163,
                Bottom = 52,
                Left = 252,
                Right = 73,
                BottomTolerance = 10,
            },
            [Device.iCloud] = new()
            {
                Top = 205,
                Bottom = 20,
                Left = 400,
                Right = 73,
                BottomTolerance = 20,
            },
            [Device.iPadAir] = new()
            {
                Top = 119,
                Bottom = 0,
                Left = 0,
                Right = 0,
                BottomTolerance = 10,
            },
            [Device.iPhone8] = new()
            {
                Top = 119,
                Bottom = 0,
                Left = 0,
                Right = 0,
                BottomTolerance = 10,
            },
            [Device.iPhone11] = new()
            {
                Top = 119,
                Bottom = 0,
                Left = 0,
                Right = 0,
                BottomTolerance = 10,
            },
            [Device.iPhone12] = new()
            {
                Top = 177,
                Bottom = 0,
                Left = 0,
                Right = 0,
                BottomTolerance = 10,
            },
            [Device.iPhone12Pro] = new()
            {
                Top = 177,
                Bottom = 0,
                Left = 0,
                Right = 0,
                BottomTolerance = 10,
            },
            [Device.iPhone13] = new()
            {
                Top = 177,
                Bottom = 0,
                Left = 0,
                Right = 0,
                BottomTolerance = 10,
            },
            [Device.iPhone13ProMax] = new()
            {
                Top = 119,
                Bottom = 0,
                Left = 0,
                Right = 0,
                BottomTolerance = 10,
            },
            [Device.Mailbirddark] = new()
            {
                Top = 135,
                Bottom = 10,
                Left = 410,
                Right = 10,
                BottomTolerance = 10,
            },
            [Device.Mailbirdlight] = new()
            {
                Top = 135,
                Bottom = 10,
                Left = 410,
                Right = 10,
                BottomTolerance = 10,
            },
            [Device.o2pl] = new()
            {
                Top = 300,
                Bottom = 10,
                Left = 245,
                Right = 347,
                BottomTolerance = 10,
            },
            [Device.Outlook2016] = new()
            {
                Top = 220,
                BottomTolerance = 10,
                Left = 8,
                Right = 25,
                Bottom = 15
            },
        };
    }

    public static Device DeviceForKey(string key) =>
        keyToDevice[key];

    public static string KeyForDevice(Device device) =>
        deviceToKey[device];

    public static bool ScrubForDeivce(Device device, [NotNullWhen(true)] out ScrubSpec? spec) =>
        deviceScrubbing.TryGetValue(device, out spec);
}

class ScrubSpec
{
    public int Top { get; set; }
    public int Bottom { get; set; }
    public int Right { get; set; }
    public int Left { get; set; }
    public int? BottomTolerance { get; set; }
}