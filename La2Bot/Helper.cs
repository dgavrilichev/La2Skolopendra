using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace La2Bot
{
    internal static class Helper
    {
        [DllImport("User32.dll")]
        static extern int SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        internal static extern bool SendMessage(IntPtr hWnd, Int32 msg, Int32 wParam, Int32 lParam);
        static Int32 WM_SYSCOMMAND = 0x0112;
        static Int32 SC_RESTORE = 0xF120;


        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool GetWindowRect(IntPtr hWnd, ref Rect lpRect);
        [StructLayout(LayoutKind.Sequential)]
        public struct Rect
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
        }
        
        public static Bitmap SaveScreenshot(IntPtr hWnd)
        {
            SetForegroundWindow(hWnd);
            SendMessage(hWnd, WM_SYSCOMMAND, SC_RESTORE, 0);

            var rect = new Rect();
            GetWindowRect(hWnd, ref rect);

            int width = rect.right - rect.left;
            int height = rect.bottom - rect.top;

            var bmp = new Bitmap(width, height, PixelFormat.Format32bppArgb);
            Graphics graphics = Graphics.FromImage(bmp);
            graphics.CopyFromScreen(rect.left, rect.top, 0, 0, new Size(width, height), CopyPixelOperation.SourceCopy);
            graphics.FillRectangle(Brushes.Black, 330, 230, 150, 100);
            graphics.FillRectangle(Brushes.Black, 0, 330, 180, 20);
            graphics.FillRectangle(Brushes.Black, 0, 370, 2000, 2000);
            graphics.FillRectangle(Brushes.Black, 0, 0, 2000,30);

            var grey = MakeGrayscale3(bmp);

            bmp.Save("test.png", System.Drawing.Imaging.ImageFormat.Png);
            return grey;
        }

        public static Bitmap GetHp(IntPtr hWnd)
        {
            SetForegroundWindow(hWnd);
            SendMessage(hWnd, WM_SYSCOMMAND, SC_RESTORE, 0);

            var rect = new Rect();
            GetWindowRect(hWnd, ref rect);

            var bmp = new Bitmap(150, 5, PixelFormat.Format32bppArgb);
            Graphics graphics = Graphics.FromImage(bmp);
            graphics.CopyFromScreen(rect.left + 26, rect.top + 400, 0, 0, new Size(150, 5), CopyPixelOperation.SourceCopy);

            var grey = MakeGrayscale3(bmp);

            bmp.Save("hp.png", System.Drawing.Imaging.ImageFormat.Png);
            return grey;
        }

        private static Bitmap MakeGrayscale3(Bitmap original)
        {
            //create a blank bitmap the same size as original
            Bitmap newBitmap = new Bitmap(original.Width, original.Height);

            //get a graphics object from the new image
            Graphics g = Graphics.FromImage(newBitmap);

            //create the grayscale ColorMatrix
            ColorMatrix colorMatrix = new ColorMatrix(
                new float[][]
                {
                    new float[] {.3f, .3f, .3f, 0, 0},
                    new float[] {.59f, .59f, .59f, 0, 0},
                    new float[] {.11f, .11f, .11f, 0, 0},
                    new float[] {0, 0, 0, 1, 0},
                    new float[] {0, 0, 0, 0, 1}
                });

            //create some image attributes
            ImageAttributes attributes = new ImageAttributes();

            //set the color matrix attribute
            attributes.SetColorMatrix(colorMatrix);

            //draw the original image on the new image
            //using the grayscale color matrix
            g.DrawImage(original, new Rectangle(0, 0, original.Width, original.Height),
                0, 0, original.Width, original.Height, GraphicsUnit.Pixel, attributes);

            //dispose the Graphics object
            g.Dispose();
            return newBitmap;
        }
    }
}
