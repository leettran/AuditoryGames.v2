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

namespace AuditoryGames.GameFramework
{
    public class AnimationData
    {
        public string[] frames = null;
        public double fps = 0;
        public int width = 0;
        public int height = 0;

        public AnimationData(string[] frames, double fps)
        {
            this.frames = frames;
            this.fps = fps;
        }
    }

    /*
        A game object with an animated bitmap image
    */
    public class AnimatedGameObject : GameObject
    {
        protected double timeSinceLastFrame = 0;
        protected int currentFrame = 0;
        protected AnimationData animationData = null;
        protected bool playOnce = false;

        private static ResourcePool<AnimatedGameObject> animatedGameObjectPool = new ResourcePool<AnimatedGameObject>();

        static public AnimatedGameObject UnusedAnimatedGameObject
        {
            get
            {
                return animatedGameObjectPool.UnusedObject;
            }
        }

        public AnimatedGameObject startupAnimatedGameObject(Point dimensions, AnimationData animationData, int zLayer, bool playOnce)
        {
            // initialise variables
            this.animationData = animationData;
            this.playOnce = playOnce;
            currentFrame = 0;
            timeSinceLastFrame = 0;

            base.startupGameObject(dimensions, animationData.frames[currentFrame], zLayer);

            return this;
        }

        public override void shutdown()
        {
            (AuditoryGames.GameFramework.App.Current.RootVisual as Page).LayoutRoot.Children.Remove(rect);
            rect = null;
            base.shutdown();
        }

        public override void enterFrame(double dt)
        {
            base.enterFrame(dt);

            if (inUse)
            {
                timeSinceLastFrame += dt;

                if (timeSinceLastFrame >= 1 / animationData.fps)
                {
                    // we don't get a render event if the page is not displayed. Using this while
                    // loop means that if we come back to the game after browsing another page
                    // the animations don't go crazy
                    while (timeSinceLastFrame >= 1 / animationData.fps)
                    {
                        timeSinceLastFrame -= 1 / animationData.fps;
                        ++currentFrame;
                        currentFrame %= animationData.frames.Length;                        
                    }

                    if (playOnce && currentFrame == 0)
                        shutdown();
                    else
                        prepareImage(animationData.frames[currentFrame]);
                }

                cropToWindow();
            }
        }

        override protected void prepareImage(string image)
        {
            // get the embedded image and display it on the rectangle
            imageBrush = new ImageBrush
            {
                Stretch = Stretch.None,
                AlignmentX = AlignmentX.Left,
                AlignmentY = AlignmentY.Top
            };
            imageBrush.ImageSource = ResourceHelper.GetBitmap(image);
            rect.Fill = imageBrush;
        }
    }


}
