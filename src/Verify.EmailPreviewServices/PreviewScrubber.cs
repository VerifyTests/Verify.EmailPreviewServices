public static class PreviewScrubber
{
    public static async Task<Stream> Scrub(Stream stream, Device device)
    {
        var memoryStream = new MemoryStream();
        if (Devices.ScrubForDeivce(device, out var spec))
        {
            using var image = await Image.LoadAsync<Rgba32>(stream);
            Crop(image, spec);
            RemoveBorder(image,spec.BorderTolerance);

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
            image.Height - spec.Top- spec.Bottom
        )));

    static void RemoveBorder(Image<Rgba32> image, int? tolerance)
    {
        if (tolerance == null)
        {
            return;
        }

        var toleranceValue = tolerance.Value;
        // Detect border color from corner pixel
        var borderColor = image[0, 0];

        int left = 0, top = 0, right = image.Width - 1, bottom = image.Height - 1;

        // Find left border
        for (var x = 0; x < image.Width; x++)
        {
            var hasContent = false;
            for (var y = 0; y < image.Height; y++)
            {
                if (!ColorsMatch(image[x, y], borderColor, toleranceValue))
                {
                    hasContent = true;
                    break;
                }
            }

            if (hasContent)
            {
                left = x;
                break;
            }
        }

        // Find right border
        for (var x = image.Width - 1; x >= 0; x--)
        {
            var hasContent = false;
            for (var y = 0; y < image.Height; y++)
            {
                if (!ColorsMatch(image[x, y], borderColor, toleranceValue))
                {
                    hasContent = true;
                    break;
                }
            }

            if (hasContent)
            {
                right = x;
                break;
            }
        }

        // Find top border
        for (var y = 0; y < image.Height; y++)
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
                top = y;
                break;
            }
        }

        // Find bottom border
        for (var y = image.Height - 1; y >= 0; y--)
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

        // Crop to content bounds
        if (left <= right && top <= bottom)
        {
            image.Mutate(_ => _.Crop(new(
                left,
                top,
                right - left + 1,
                bottom - top + 1
            )));
        }
    }

    static bool ColorsMatch(Rgba32 c1, Rgba32 c2, int tolerance) =>
        Math.Abs(c1.R - c2.R) <= tolerance &&
        Math.Abs(c1.G - c2.G) <= tolerance &&
        Math.Abs(c1.B - c2.B) <= tolerance &&
        Math.Abs(c1.A - c2.A) <= tolerance;
}