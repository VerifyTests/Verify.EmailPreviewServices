public static class PreviewScrubber
{
    public static async Task<Stream> Scrub(Stream stream, Device device)
    {
        var memoryStream = new MemoryStream();
        var srubber = GetSrubber(device);
        if (srubber == null)
        {
            await stream.CopyToAsync(memoryStream);
        }
        else
        {
            using var image = await Image.LoadAsync<Rgba32>(stream);
            srubber(image);
            await image.SaveAsPngAsync(memoryStream);
        }

        memoryStream.Position = 0;
        return memoryStream;
    }

    static Action<Image<Rgba32>>? GetSrubber(Device device) =>
        device switch
        {
            //Device.Outlook2016 => ScrubOutlook2016,
            _ => null
        };

    static void ScrubOutlook2016(Image<Rgba32> image)
    {
        var cropHeight = image.Height;
        var width = image.Width;
        for (var y = image.Height - 1; y >= 0; y--)
        {
            var hasContent = false;
            for (var x = 0; x < width; x++)
            {
                var pixel = image[x, y];
                if (pixel.R < 240 || pixel.G < 240 || pixel.B < 240)
                {
                    hasContent = true;
                    break;
                }
            }

            if (hasContent)
            {
                cropHeight = y + 1;
                break;
            }
        }

        var topCrop = 220;
        var newHeight = cropHeight - topCrop;

        image.Mutate(ctx => ctx.Crop(new(0, topCrop, width, newHeight)));
    }
}