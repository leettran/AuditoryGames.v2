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

namespace LSRI.TreasureHunter
{
    public class ScoreObject : GameObject
    {
        protected const double SPEED = 200;

        private static ResourcePool<ScoreObject> resourcePool = new ResourcePool<ScoreObject>();

        public delegate void ScoreLogic(double dt);
        private ScoreLogic _scoreLogic = null;

        private GameObject _player = null;
        protected TextBlock _bkg;

        static public ScoreObject UnusedScore
        {
            get
            {
                return resourcePool.UnusedObject;
            }
        }

        public ScoreObject()
        {

        }

        new public Point Position
        {
            set
            {
                position = value;

                // make sure we are only moving in whole pixels (i.e. integer values)
                // this stops the display from looking fuzzy
                rect.SetValue(Canvas.LeftProperty, Math.Round(position.X));
                rect.SetValue(Canvas.TopProperty, Math.Round(position.Y));
                _bkg.SetValue(Canvas.LeftProperty, Math.Round(position.X));
                _bkg.SetValue(Canvas.TopProperty, Math.Round(position.Y));
            }
            get
            {
                return position;
            }
        }


        public override void enterFrame(double dt)
        {
            base.enterFrame(dt);
            if (_scoreLogic != null) _scoreLogic(dt);
        }

        public ScoreObject startupGameObject(GameObject parent,int score)
        {
            this.ImageStretch = Stretch.UniformToFill;
            this.IsCroppable = CropTo.Both;

            base.startupGameObject(new Point(80, 29), "Media/board_score.png", ZLayers.PLAYER_Z);

            _bkg = new TextBlock()
            {
                Height = dimensions.Y,
                Width = dimensions.X
            };
            _bkg.Padding = new Thickness(25, 3, 2, 3);
            _bkg.Text = score.ToString();
            _bkg.FontSize = 16;
            _bkg.FontWeight = FontWeights.Bold;
            _bkg.SetValue(Canvas.LeftProperty, 0.0d);
            _bkg.SetValue(Canvas.TopProperty, 0.0d);

            (AuditoryGameApp.Current.RootVisual as GamePage).LayoutRoot.Children.Remove(this.rect);
            (AuditoryGameApp.Current.RootVisual as GamePage).LayoutRoot.Children.Add(rect);
            (AuditoryGameApp.Current.RootVisual as GamePage).LayoutRoot.Children.Add(_bkg);

            _player = parent;
            _scoreLogic = (score==0) ? new ScoreLogic(this.ZeroScoreLogic) : new ScoreLogic(this.AddScoreLogic);
            this._collisionName = CollisionIdentifiers.NONE;
            return this;
        }

        protected void AddScoreLogic(double dt)
        {
            Position = new Point(Position.X, Position.Y - SPEED * dt);
            this.Rect.Opacity = this.Rect.Opacity - SPEED / 15000;
            this._bkg.Opacity = this.Rect.Opacity;

            if (Rect.Opacity <=0) this.shutdown();
            offscreenCheck();
        }

        protected void ZeroScoreLogic(double dt)
        {
            Position = new Point(Position.X, Position.Y + SPEED * dt / 4);
            this.Rect.Opacity = this.Rect.Opacity - SPEED / 15000;
            this._bkg.Opacity = this.Rect.Opacity;

            if (Rect.Opacity <= 0) this.shutdown();
            offscreenCheck();
        }

        public override void shutdown()
        {
            (AuditoryGameApp.Current.RootVisual as GamePage).LayoutRoot.Children.Remove(_bkg);
            _bkg = null;
            base.shutdown();
            _scoreLogic = null;
            if (_player != null) _player.collision(null);
        }


    }
}
