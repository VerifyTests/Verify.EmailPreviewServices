﻿public static class PreviewScrubber
{
    public static async Task<Stream> Scrub(Stream stream, Device device)
    {
        var memoryStream = new MemoryStream();
        if (Devices.ScrubForDeivce(device, out var spec))
        {
            using var image = await Image.LoadAsync<Rgba32>(stream);
            Crop(image, spec);
                ScrubBottom(image, spec.Background);

            await image.SaveAsPngAsync(memoryStream);
        }
        else
        {
            await stream.CopyToAsync(memoryStream);
        }

        memoryStream.Position = 0;
        return memoryStream;
    }

    static void ScrubBottom(Image<Rgba32> image, int? color)
    {
        if (color == null)
        {
            return;
        }

        var colorValue = color.Value;
        var cropHeight = image.Height;
        var width = image.Width;
        for (var y = image.Height - 1; y >= 0; y--)
        {
            var hasContent = false;
            for (var x = 0; x < width; x++)
            {
                var pixel = image[x, y];
                if (pixel.R < colorValue || pixel.G < colorValue || pixel.B < colorValue)
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

        image.Mutate(_ => _.Crop(new(0, 0, width, cropHeight)));
    }

    static void Crop(Image<Rgba32> image, ScrubSpec spec) =>
        image.Mutate(x => x.Crop(new(
            spec.Left,
            spec.Top,
            image.Width - spec.Left - spec.Right,
            image.Height - spec.Top
        )));
}