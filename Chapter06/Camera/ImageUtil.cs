using System;
using System.IO;
using System.Windows.Media.Imaging;
using ExifLib;

namespace Camera
{
    public static class ImageUtil
    {
        public static int GetAngleFromChosenPhoto(Stream stream, string fileName)
        {
            var angle = 0;

            stream.Position = 0;
            JpegInfo info = ExifReader.ReadJpeg(stream, fileName);

            switch (info.Orientation)
            {
                case ExifOrientation.TopLeft:
                case ExifOrientation.Undefined:
                    angle = 0;
                    break;
                case ExifOrientation.TopRight:
                    angle = 90;
                    break;
                case ExifOrientation.BottomRight:
                    angle = 180;
                    break;
                case ExifOrientation.BottomLeft:
                    angle = 270;
                    break;
            }
            return angle;
        }

        public static WriteableBitmap GetBitmap(Stream stream)
        {
            BitmapImage bitmap = new BitmapImage();
            bitmap.SetSource(stream);
            return new WriteableBitmap(bitmap);

        }

        public static WriteableBitmap GetBitmap(Stream stream, int angle)
        {
            stream.Position = 0;
            if (angle % 90 != 0 || angle < 0) throw new ArgumentException();

            WriteableBitmap wbSource = GetBitmap(stream);
            if (angle % 360 == 0) 
                return wbSource;

            WriteableBitmap wbTarget = null;

            if (angle % 180 == 0)
            {
                wbTarget = new WriteableBitmap(wbSource.PixelWidth, wbSource.PixelHeight);
            }
            else
            {
                wbTarget = new WriteableBitmap(wbSource.PixelHeight, wbSource.PixelWidth);
            }

            for (int x = 0; x < wbSource.PixelWidth; x++)
            {
                for (int y = 0; y < wbSource.PixelHeight; y++)
                {
                    switch (angle % 360)
                    {
                        case 90:
                            wbTarget.Pixels[(wbSource.PixelHeight - y - 1) + x * wbTarget.PixelWidth] = wbSource.Pixels[x + y * wbSource.PixelWidth];
                            break;
                        case 180:
                            wbTarget.Pixels[(wbSource.PixelWidth - x - 1) + (wbSource.PixelHeight - y - 1) * wbSource.PixelWidth] = wbSource.Pixels[x + y * wbSource.PixelWidth];
                            break;
                        case 270:
                            wbTarget.Pixels[y + (wbSource.PixelWidth - x - 1) * wbTarget.PixelWidth] = wbSource.Pixels[x + y * wbSource.PixelWidth];
                            break;
                    }
                }
            }
            return wbTarget;
        }
    }
}
