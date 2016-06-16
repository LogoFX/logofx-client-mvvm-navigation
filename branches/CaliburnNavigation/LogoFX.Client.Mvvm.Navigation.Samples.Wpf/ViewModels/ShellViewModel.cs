using System.Windows.Input;
using Caliburn.Micro;
using JetBrains.Annotations;
using LogoFX.Client.Mvvm.Commanding;
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

        private ICommand _navigateBack;

        public ICommand NavigateBack
        {
            get
            {
                return _navigateBack ??
                       (_navigateBack = ActionCommand
                           .When(() => _navigationService.CanGoBack)
                           .Do(() =>
                           {
                               _navigationService.GoBack();
                           })
                           .RequeryOnPropertyChanged(_navigationService, () => _navigationService.CanGoBack));
            }
        }

        private ICommand _navigateForward;

        public ICommand NavigateForward
        {
            get
            {
                return _navigateForward ??
                       (_navigateForward = ActionCommand
                           .When(() => _navigationService.CanGoForward)
                           .Do(() =>
                           {
                               _navigationService.GoForward();
                           })
                           .RequeryOnPropertyChanged(_navigationService, () => _navigationService.CanGoForward));
            }
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