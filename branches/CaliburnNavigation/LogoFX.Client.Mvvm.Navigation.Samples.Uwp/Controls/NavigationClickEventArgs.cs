using Windows.UI.Xaml;

namespace LogoFX.Client.Mvvm.Navigation.Samples.Uwp.Controls
{
    public class NavigationClickEventArgs : RoutedEventArgs
    {
        public object NavigationItem { get; private set; }

        public NavigationClickEventArgs(object navigationItem)
        {
            NavigationItem = navigationItem;
        }
    }
}