﻿using System.ComponentModel;
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
            [Device.Outlook2016] = new()
            {
                Top = 220,
                BorderTolerance = 11,
                Left = 10,
                Right = 50
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
    public int? BorderTolerance { get; set; }
}