public static class PreviewScrubber
{
    public static async Task<Stream> Scrub(Stream stream, Device device)
    {
        var memoryStream = new MemoryStream();
        if (Devices.ScrubForDeivce(device, out var spec))
        {
            using var image = await Image.LoadAsync<Rgba32>(stream);
            Crop(image, spec);
            RemoveBottom(image, spec.BottomTolerance);

            await image.SaveAsJpegAsync(memoryStream);
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

    static void Crop(Image<Rgba32> image, ScrubSpec spec)
    {
        if (spec is {Top: 0, Bottom: 0, Left: 0, Right: 0})
        {
            return;
        }

        image.Mutate(x => x.Crop(new(
            spec.Left,
            spec.Top,
            image.Width - spec.Left - spec.Right,
            image.Height - spec.Top - spec.Bottom
        )));
    }

    static void RemoveBottom(Image<Rgba32> image, int? tolerance)
    {
        if (tolerance == null)
        {
            return;
        }

        var toleranceValue = tolerance.Value;
        // Detect border color from bottom middle pixel
        var height = image.Height;
        var borderColor = image[image.Width / 2, height - 1];

        var bottom = height - 1;

        for (var y = height - 1; y >= 0; y -= 15)
        {
            var hasContent = false;
            for (var x = 0; x < image.Width; x++)
            {
                if (!ColorsMatch(image[x, y], borderColor, toleranceValue))
                {
                    hasContent = true;
                    break;
                }
            }

            if (hasContent)
            {
                bottom = y;
                break;
            }
        }

        if (bottom + 25 < height)
        {
            bottom += 25;
        }
        // Crop to content bounds
        if (bottom < height)
        {
            image.Mutate(_ =>
                _.Crop(new(
                    0,
                    0,
                    image.Width,
                    bottom
                )));
        }
    }

    static bool ColorsMatch(Rgba32 c1, Rgba32 c2, int tolerance) =>
        Math.Abs(c1.R - c2.R) <= tolerance &&
        Math.Abs(c1.G - c2.G) <= tolerance &&
        Math.Abs(c1.B - c2.B) <= tolerance &&
        Math.Abs(c1.A - c2.A) <= tolerance;
}