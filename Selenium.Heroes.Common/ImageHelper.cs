using System.Net;
using System.Drawing;
using System.Drawing.Imaging;
using Selenium.Heroes.Common.Extensions;

namespace Selenium.Heroes.Common;

public class ImageHelper
{
    public static Image GetImage(string url)
    {
        var client = new WebClient();
        var stream = client.OpenRead(url);
        return new Bitmap(stream);
    }

    public static string ReadCaptchaImage(string url)
    {
        var client = new WebClient();
        var stream = client.OpenRead(url);
        return stream.ConvertToBase64();
    }

    public static void SaveImage(Image image, string path)
    {     
        image.Save(path, ImageFormat.Jpeg);
        Console.WriteLine($"Image saved. Path: '{path}'");
    }
}