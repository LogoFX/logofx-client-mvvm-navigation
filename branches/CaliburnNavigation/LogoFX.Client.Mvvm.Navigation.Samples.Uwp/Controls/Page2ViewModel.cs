using Caliburn.Micro;

namespace LogoFX.Client.Mvvm.Navigation.Samples.Uwp.Controls
{
    [NavigationViewModel(ConductorType = typeof(MainViewModel))]
    public sealed class Page2ViewModel : Screen
    {
        public override string DisplayName
        {
            get { return "Page2"; }
            set { }
        }
    }
}