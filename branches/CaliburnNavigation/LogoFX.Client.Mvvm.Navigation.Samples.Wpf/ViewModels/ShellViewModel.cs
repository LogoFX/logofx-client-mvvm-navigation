using Caliburn.Micro;
using JetBrains.Annotations;

namespace LogoFX.Client.Mvvm.Navigation.Samples.Wpf.ViewModels
{
    [UsedImplicitly]
    public class ShellViewModel : Screen
    {
        public override string DisplayName
        {
            get { return "Navigation Sample - WPF Application"; }
            set { }
        }
    }
}