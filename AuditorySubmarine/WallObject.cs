using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using LSRI.AuditoryGames.GameFramework;
using System.Windows.Media.Imaging;
using System.Collections.Generic;
using System.Diagnostics;

namespace LSRI.Submarine
{
    public class WallObject : GameObject
    {

        public WallObject()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dimensions"></param>
        /// <param name="image"></param>
        /// <param name="zLayer"></param>
       new  public void startupGameObject(Point dimensions, string image, int zLayer)
        {
            base.startupGameObject(dimensions, image, zLayer);
            this.collisionName = CollisionIdentifiers.ENEMYWEAPON;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="image"></param>
        override protected void prepareImage(string image)
        {
            imageBrush = new ImageBrush
            {
                Stretch = Stretch.None,
                AlignmentX = AlignmentX.Left,
                AlignmentY = AlignmentY.Top
            };
            imageBrush.ImageSource = tileImage(image);
            rect.Fill = imageBrush;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        private WriteableBitmap tileImage(string image)
        {
            WriteableBitmap src = new WriteableBitmap((BitmapSource)ResourceHelper.GetBitmap(image));
            WriteableBitmap dest = new WriteableBitmap((int)rect.Width, (int)rect.Height);

            int start = 0;
            List<int> pixels = new List<int>(src.Pixels);
            List<List<int>> linelist = new List<List<int>>();
            for (int i = 0; i < src.PixelHeight; i++)
            {
                List<int> srcrow = pixels.GetRange(start, src.PixelWidth);
                List<int> newline = new List<int>();
                start += src.PixelWidth;
                do
                {
                    newline.AddRange(srcrow);

                } while (newline.Count < dest.PixelWidth);
                int excess = newline.Count - (int)rect.Width;
                if (excess > 0)
                    newline.RemoveRange(newline.Count - excess, excess);
                linelist.Add(newline);
            }
            pixels.Clear();
            start = 0;
            for (int y = 0; y < rect.Height - 1; y++)
            {
                pixels.AddRange(linelist[start]);
                start++;
                if (start >= src.PixelHeight) start = 0;
            }
            Array.Copy(pixels.ToArray(), dest.Pixels, pixels.Count);
            // dest.Invalidate();

            return dest;
        }

        override public void enterFrame(double dt)
        {
            if (GameLevelDescriptor.CurrentGate != 5)
            {
                Point pt = (IAppManager.Instance as SubmarineApplicationManager)._submarine.Position;
                this.Rect.Opacity = (Position.X - pt.X+200) / (Position.X);
                //Debug.WriteLine("OPACITY : {0}", this.Rect.Opacity);
            }
        }


    }
}
