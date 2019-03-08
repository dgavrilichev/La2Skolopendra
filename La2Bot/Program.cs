using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using Tesseract;
using Rect = Tesseract.Rect;

namespace La2Bot
{
    class Program
    {
        private static IntPtr _hWnd;

        public static void Main(string[] args)
        {
            _hWnd = GetWindowByName("Lineage II");
            using (var engine = new TesseractEngine(@"./tessdata", "eng", EngineMode.Default))
            {
                while (true)
                {
                    var image = Helper.SaveScreenshot(_hWnd);
                    var src = Mat.FromImageData(ImageToByte2(image));
                    var blacked = new Mat();
 
                    Cv2.Threshold(src, blacked, 127, 255, ThresholdTypes.Binary);

                    var foundTarget = false;
                    using (var page = engine.Process(blacked.ToBitmap()))
                    {
                        using (var iter = page.GetIterator())
                        {
                            iter.Begin();
                            do
                            {
                                if (iter.TryGetBoundingBox(PageIteratorLevel.TextLine, out var rect))
                                {
                                    var curText = iter.GetText(PageIteratorLevel.TextLine);
                                    var modifiedString = curText.Replace("\n", "");

                                    if (modifiedString.Length > 5 && rect.Height > 3 && rect.Height < 20)
                                    {
                                        foundTarget = true;
                                        Console.WriteLine($"{modifiedString}  | {rect.ToString()}");
                                        Attack(rect);
                                        break;
                                    }
                                }
                            } while (iter.Next(PageIteratorLevel.TextLine));
                        }
                    }

                    if (!foundTarget)
                        SpinCamera();

                    Thread.Sleep(100);
                }
            }
        }

        [DllImport("User32.dll")]
        static extern int SetForegroundWindow(IntPtr hWnd);

        private static void SpinCamera()
        {
            SetForegroundWindow(_hWnd);
            var rect = new Helper.Rect();
            GetWindowRect(_hWnd, ref rect);

            SetCursorPos(rect.left + 100, rect.top + 100);
            mouse_event(MOUSEEVENTF_LEFTDOWN, rect.left + 400, rect.top + 400, 0, 0);
            mouse_event(MOUSEEVENTF_LEFTUP, rect.left + 400, rect.top + 400, 0, 0);

            Thread.Sleep(500);

            for (var i = 0; i < 20; i++)
            {
                PostMessage(_hWnd, WM_KEYDOWN, VK_RIGHT, 0);
                Thread.Sleep(100);
            }
        }

        private static IntPtr GetWindowByName(string name)
        {
            var hWnd = IntPtr.Zero;
            foreach (var pList in Process.GetProcesses())
            {
                if (pList.MainWindowTitle.Contains(name))
                {
                    hWnd = pList.MainWindowHandle;
                }
            }
            return hWnd;
        }

        private static void Attack(Rect rect)
        {
            LeftMouseClick((rect.X1 + rect.X2) / 2, rect.Y1 + 30);
        }

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool GetWindowRect(IntPtr hWnd, ref Helper.Rect lpRect);


        //This is a replacement for Cursor.Position in WinForms
        [DllImport("user32.dll")]
        static extern bool SetCursorPos(int x, int y);

        [DllImport("user32.dll")]
        private static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);

        private const int MOUSEEVENTF_LEFTDOWN = 0x02;
        private const int MOUSEEVENTF_LEFTUP = 0x04;
        private const int MOUSEEVENTF_RIGHTDOWN = 0x0008;
        private const int MOUSEEVENTF_RIGHTUP = 0x0010;

        const UInt32 WM_KEYDOWN = 0x0100;
        const UInt32 WM_KEYUP = 0x0101;
        const int VK_F5 = 0x74;
        const int VK_RIGHT = 0x27;

        [DllImport("user32.dll")]
        static extern bool PostMessage(IntPtr hWnd, UInt32 Msg, int wParam, int lParam);

        //This simulates a left mouse click
        public static void LeftMouseClick(int xpos, int ypos)
        {
            var rect = new Helper.Rect();
            GetWindowRect(_hWnd, ref rect);

            xpos = xpos + rect.left;
            ypos = ypos + rect.top;

            SetCursorPos(xpos, ypos);
            Thread.Sleep(200);
            mouse_event(MOUSEEVENTF_LEFTDOWN, xpos, ypos, 0, 0);
            Thread.Sleep(200);
            mouse_event(MOUSEEVENTF_LEFTUP, xpos, ypos, 0, 0);

            Thread.Sleep(200);
            SetCursorPos(xpos, ypos);
            mouse_event(MOUSEEVENTF_LEFTDOWN, xpos, ypos, 0, 0);
            Thread.Sleep(200);
            mouse_event(MOUSEEVENTF_LEFTUP, xpos, ypos, 0, 0);

            Thread.Sleep(200);
            PostMessage(_hWnd, WM_KEYDOWN, VK_F5, 0);
            Thread.Sleep(400);

            while (true)
            {
                var hp = Helper.GetHp(_hWnd);
                var pixel = hp.GetPixel(2, 2);

                if(pixel.R > 80)
                    Console.WriteLine("Alive!");
                else
                {
                    Console.WriteLine("Dead! time to find next target!");
                    break;                   
                }

                Thread.Sleep(100);
            }
        }

        private static byte[] ImageToByte2(Image img)
        {
            using (var stream = new MemoryStream())
            {
                img.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                return stream.ToArray();
            }
        }      
    }
}