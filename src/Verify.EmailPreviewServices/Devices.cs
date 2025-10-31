using System.ComponentModel;

namespace VerifyTests;

static class Devices
{
    static Dictionary<Device, string> deviceToKey = [];
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
    }

    public static Device DeviceForKey(string key) =>
        keyToDevice[key];
    public static string KeyForDevice(Device device) =>
        deviceToKey[device];
}