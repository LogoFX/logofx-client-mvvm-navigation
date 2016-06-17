using System;
using System.Windows;

namespace LogoFX.Client.Mvvm.Navigation.Samples.Wpf.Controls
{
    public class NavigationClickEventArgs : RoutedEventArgs
    {
        public object NavigationItem { get; private set; }

        public NavigationClickEventArgs(RoutedEvent routedEvent, object navigationItem)
            : base(routedEvent)
        {
            NavigationItem = navigationItem;
        }
    }
}