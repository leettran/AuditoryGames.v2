﻿using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace AuditoryGames.GameFramework
{
    public static class MathUtil
    {
        public static bool RectIntersect(Rectangle rectangle1, Rectangle rectangle2)
        {
            return (((double)rectangle1.GetValue(Canvas.LeftProperty) <= (double)rectangle2.GetValue(Canvas.LeftProperty) + rectangle2.Width)
              && ((double)rectangle1.GetValue(Canvas.LeftProperty) + rectangle1.Width >= (double)rectangle2.GetValue(Canvas.LeftProperty))
              && ((double)rectangle1.GetValue(Canvas.TopProperty) <= (double)rectangle2.GetValue(Canvas.TopProperty) + rectangle2.Height)
              && ((double)rectangle1.GetValue(Canvas.TopProperty) + rectangle1.Height >= (double)rectangle2.GetValue(Canvas.TopProperty)));
        }

        public static bool PointInRect(Rectangle rectangle1, Rectangle rectangle2)
        {
            Point pt1 = new Point(
                (double)rectangle1.GetValue(Canvas.LeftProperty) + rectangle1.Width/2,
                (double)rectangle1.GetValue(Canvas.TopProperty) + rectangle1.Height / 2
            );

            return
                (pt1.X >= ((double)rectangle2.GetValue(Canvas.LeftProperty))) &&
                (pt1.X <= ((double)rectangle2.GetValue(Canvas.LeftProperty) + rectangle2.Width)) &&
                (pt1.Y >= ((double)rectangle2.GetValue(Canvas.TopProperty))) &&
                (pt1.Y <= ((double)rectangle2.GetValue(Canvas.TopProperty) + rectangle2.Height))
                ;
        }

    }
}
