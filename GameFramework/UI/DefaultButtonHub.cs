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
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;

namespace LSRI.AuditoryGames.GameFramework.UI
{
    public class DefaultButtonHub
    {
        ButtonAutomationPeer peer = null;

        private void Attach(DependencyObject source)
        {
            if (source is Button)
            {
                peer = new ButtonAutomationPeer(source as Button);
            }
            else if (source is TextBox)
            {
                TextBox tb = source as TextBox;
                tb.KeyUp += OnKeyUp;
            }
            else if (source is PasswordBox)
            {
                PasswordBox pb = source as PasswordBox;
                pb.KeyUp += OnKeyUp;
            }
        }

        private void OnKeyUp(object sender, KeyEventArgs arg)
        {
            if (arg.Key == Key.Enter)
                if (peer != null)
                    ((IInvokeProvider)peer).Invoke();
        }

        public static DefaultButtonHub GetDefaultHub(DependencyObject obj)
        {
            return (DefaultButtonHub)obj.GetValue(DefaultHubProperty);
        }

        public static void SetDefaultHub(DependencyObject obj, DefaultButtonHub value)
        {
            obj.SetValue(DefaultHubProperty, value);
        }

        // Using a DependencyProperty as the backing store for DefaultHub.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DefaultHubProperty =
            DependencyProperty.RegisterAttached("DefaultHub", typeof(DefaultButtonHub), typeof(DefaultButtonHub), new PropertyMetadata(OnHubAttach));

        private static void OnHubAttach(DependencyObject source, DependencyPropertyChangedEventArgs prop)
        {
            DefaultButtonHub hub = prop.NewValue as DefaultButtonHub;
            hub.Attach(source);
        }

    }

   	

    public static class DefaultButtonService
    {
    public static DependencyProperty DefaultButtonProperty =
          DependencyProperty.RegisterAttached("DefaultButton",
                                              typeof(Button),
                                              typeof(DefaultButtonService),
                                              new PropertyMetadata(null, DefaultButtonChanged));

    private static void DefaultButtonChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
        var uiElement = d as UIElement;
        var button = e.NewValue as Button;
        if (uiElement != null && button != null) {
            uiElement.KeyUp += (sender, arg) => {
                var peer = new ButtonAutomationPeer(button);

                if (arg.Key == Key.Enter) {
                    peer.SetFocus();
                    uiElement.Dispatcher.BeginInvoke((Action)delegate {

                        var invokeProv =
                            peer.GetPattern(PatternInterface.Invoke) as IInvokeProvider;
                        if (invokeProv != null)
                            invokeProv.Invoke();
                    });
                }
            };
        }

    }

    public static Button GetDefaultButton(UIElement obj) {
        return (Button)obj.GetValue(DefaultButtonProperty);
    }

    public static void SetDefaultButton(DependencyObject obj, Button button) {
        obj.SetValue(DefaultButtonProperty, button);
    }       
}



}
