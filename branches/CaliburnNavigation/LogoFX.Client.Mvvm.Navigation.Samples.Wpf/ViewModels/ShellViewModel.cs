using Caliburn.Micro;
using JetBrains.Annotations;
using LogoFX.Client.Mvvm.ViewModel.Services;

namespace LogoFX.Client.Mvvm.Navigation.Samples.Wpf.ViewModels
{
    [UsedImplicitly]
    public sealed class ShellViewModel : Conductor<IScreen>, INavigationConductor
    {
        private readonly INavigationService _navigationService;
        private readonly IViewModelCreatorService _viewModelCreatorService;

        public ShellViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        public override string DisplayName
        {
            get { return "Navigation Sample - WPF Application"; }
            set { }
        }

        protected override void OnActivate()
        {
            base.OnActivate();

            _navigationService.Navigate(typeof(MainViewModel));
        }

        public void NavigateTo(object viewModel, object argument)
        {
            ActivateItem((IScreen) viewModel);
        }
    }
}