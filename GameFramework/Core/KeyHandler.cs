using System.Windows;
using System.Windows.Input;
using System.Collections.Generic;
using System.Diagnostics;

namespace AuditoryGames.GameFramework
{
    /*
        Code originally from http://www.bluerosegames.com/silverlight-games-101/ 
    */
    public class KeyHandler
    {
        protected static KeyHandler instance = null;
        Dictionary<Key, bool> isPressed = new Dictionary<Key, bool>();
        FrameworkElement targetElement = null;

        public bool IskeyUpOnly { get; set; }

        public static KeyHandler Instance
        {
            get
            {
                if (instance == null)
                    instance = new KeyHandler();
                return instance;
            }
        }

        public void clearKeyPresses()
        {
            isPressed.Clear();
        }

        public KeyHandler()
        {
            IskeyUpOnly = false;
        }

        public void startupKeyHandler(FrameworkElement target)
        {
            clearKeyPresses();
            targetElement = target;

            target.KeyDown += new KeyEventHandler(target_KeyDown);
            target.KeyUp += new KeyEventHandler(target_KeyUp);
            target.LostFocus += new RoutedEventHandler(target_LostFocus);
        }

        public void shutdown()
        {
            clearKeyPresses();
            targetElement.KeyDown -= new KeyEventHandler(target_KeyDown);
            targetElement.KeyUp -= new KeyEventHandler(target_KeyUp);
            targetElement.LostFocus -= new RoutedEventHandler(target_LostFocus);
            targetElement = null;
        }

        void target_KeyDown(object sender, KeyEventArgs e)
        {
            if (!isPressed.ContainsKey(e.Key))
            {
                if (!IskeyUpOnly)
                {
                    isPressed.Add(e.Key, true);
                    Debug.WriteLine("key DOWN: " + e.Key.ToString());
                }
            }
        }

        void target_KeyUp(object sender, KeyEventArgs e)
        {
            if (!IskeyUpOnly)
            {
                if (isPressed.ContainsKey(e.Key))
                {
                    isPressed.Remove(e.Key);
                    Debug.WriteLine("clear key : " + e.Key.ToString());
                }
            }
            else
            {
                 if (!isPressed.ContainsKey(e.Key))
                    {
                        isPressed.Add(e.Key, true);
                        Debug.WriteLine("key UP: " + e.Key.ToString());
                    }
            }
        }

        void target_LostFocus(object sender, RoutedEventArgs e)
        {
            clearKeyPresses();
        }

        public bool isKeyPressed(Key k)
        {
            bool ret = isPressed.ContainsKey(k);
            if (!IskeyUpOnly)
            {
            }
            else
            {
                if (ret) isPressed.Remove(k);
            }
            return ret;
        }
    }
}