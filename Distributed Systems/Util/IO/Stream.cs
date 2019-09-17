using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Util.IO
{
    public static class StreamUtil
    {
        public static byte[] ReadStream(Stream stream)
        {
            using (MemoryStream stream2 = CopyStream(stream))
            {
                return stream2.ToArray();
            }
        }
        public static MemoryStream CopyStream(Stream stream)
        {
            MemoryStream stream2 = new MemoryStream();
            int count = 0;
            byte[] buffer = new byte[0x400];
            while ((count = stream.Read(buffer, 0, 0x400)) > 0)
            {
                stream2.Write(buffer, 0, count);
            }
            return stream2;
        }
        public static Bitmap CovertDataToBitmap(byte[] data)
        {
            Bitmap bitmap;
            try
            {
                bitmap = new Bitmap(new MemoryStream(data));
            }
            catch
            {
                throw new FileNotFoundException("此文件不是图形文件");
            }
            return bitmap;
        }
        public static Bitmap ResizeImage(Bitmap mg, Size newSize)
        {
            double num = 0.0;
            double num2 = 0.0;
            double num3 = 0.0;
            if ((mg.Width * mg.Height) <= (newSize.Height * newSize.Width))
            {
                return mg;
            }
            if (mg.Width > newSize.Width)
            {
                num = Convert.ToDouble(mg.Width) / Convert.ToDouble(newSize.Width);
            }
            else
            {
                num = Convert.ToDouble(mg.Height) / Convert.ToDouble(newSize.Height);
            }
            num3 = Math.Ceiling((double)(((double)mg.Height) / num));
            num2 = Math.Ceiling((double)(((double)mg.Width) / num));
            Size size = new Size((int)num2, (int)num3);
            Bitmap image = new Bitmap(size.Width, size.Height);
            Graphics graphics = Graphics.FromImage(image);
            graphics.SmoothingMode = SmoothingMode.HighQuality;
            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
            Rectangle destRect = new Rectangle(0, 0, size.Width, size.Height);
            graphics.DrawImage(mg, destRect, 0, 0, mg.Width, mg.Height, GraphicsUnit.Pixel);
            return image;
        }
        public static void ResizeImage(string sourcefullname, int dispMaxWidth, int dispMaxHeight)
        {
            FileInfo info = new FileInfo(sourcefullname);
            ResizeImage(sourcefullname, dispMaxWidth, dispMaxHeight, info.FullName);
        }
        public static void ResizeImage(string sourcefullname, int dispMaxWidth, int dispMaxHeight, string outputFullName)
        {
            MemoryStream stream = new MemoryStream();
            using (Bitmap bitmap = new Bitmap(sourcefullname))
            {
                using (Bitmap bitmap2 = ResizeImage(bitmap, new Size(dispMaxWidth, dispMaxHeight)))
                {
                    if (bitmap2 != null)
                    {
                        bitmap2.Save(stream, ImageFormat.Jpeg);
                    }
                }
            }
            System.IO.File.WriteAllBytes(outputFullName, stream.ToArray());
        }

    }
}
