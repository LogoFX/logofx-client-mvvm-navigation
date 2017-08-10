using Caliburn.Micro;

namespace LogoFX.Client.Mvvm.Navigation.Samples.Wpf.ViewModels
{
    [NavigationViewModel(ConductorType = typeof(MainViewModel))]
    public sealed class Page3ViewModel : Screen
    {
        public override string DisplayName
        {
            get { return "Page3"; }
            set { }
        }
    }
}