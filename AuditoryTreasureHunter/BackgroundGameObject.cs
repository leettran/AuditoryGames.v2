/*  Auditory Training Games in Silverlight
    Copyright (C) 2008-2012 Nicolas Van Labeke (LSRI, Nottingham University)

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 2 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/
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
using LSRI.TreasureHunter.Model;

namespace LSRI.TreasureHunter
{
    public class BackgroundTreasureGameObject : GameObject
    {
        protected const double SPEED = 35;

        public Boolean ShowZone { set; get; }
        public Polyline[] Zones = { };

        public BackgroundTreasureGameObject()
        {
            this.ImageStretch = Stretch.None;
            this.ShowZone = false;
        }

        public override void enterFrame(double dt)
        {
            base.enterFrame(dt);
        }

        new public Point Position
        {
            set
            {
                position = value;

                // make sure we are only moving in whole pixels (i.e. integer vacompendiumlues)
                // this stops the display from looking fuzzy
                rect.SetValue(Canvas.LeftProperty, Math.Round(position.X));
                rect.SetValue(Canvas.TopProperty, Math.Round(position.Y));
                for (int i = 0; i < Zones.Length; i++)
                    if (Zones[i] != null) Zones[i].SetValue(Canvas.TopProperty, Math.Round(position.Y));
            }
            get
            {
                return position;
            }
        }

        public BackgroundTreasureGameObject startupBackgroundGameObject(Point dimensions, string image, int zLayer)
        {
            base.startupGameObject(dimensions, image, zLayer);
            Zones = new Polyline[TreasureOptions.Instance.Game.Zones];
            int rr = TreasureOptions.Instance.Game._sizeZones;
            for (int i = 0; i < TreasureOptions.Instance.Game.Zones; i++)
            {

                Zones[i] = new Polyline();
                Zones[i].Points = new PointCollection();
                Zones[i].Points.Add(new Point(i * rr, 0));
                Zones[i].Points.Add(new Point(i * rr, dimensions.Y));
                Zones[i].Points.Add(new Point((i + 1) * rr, dimensions.Y));
                Zones[i].Points.Add(new Point((i + 1) * rr, 0));
                Zones[i].Stroke = new SolidColorBrush(Color.FromArgb(25, 0, 0, 0));
                Zones[i].StrokeThickness = 3;
                Zones[i].Height = dimensions.Y;
                Zones[i].Width = Dimensions.X;
                /*if (i == (GameLevelInfo._nbTreasureZones / 2))
                {
                    Zones[i].Fill = new SolidColorBrush(Color.FromArgb(25, 255, 0, 0));
                }*/
                (AuditoryGameApp.Current.RootVisual as GamePage).LayoutRoot.Children.Add(Zones[i]);


            }
            return this;
        }

        public override void shutdown()
        {
            Zones = null;
            base.shutdown();
        }
    }
}
