using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Caliburn.Micro;
using JetBrains.Annotations;
using LogoFX.Client.Mvvm.Commanding;

namespace LogoFX.Client.Mvvm.Navigation.Samples.Wpf.ViewModels
{
    [UsedImplicitly]
    public sealed class ShellViewModel : Conductor<IScreen>, INavigationConductor
    {
        private readonly INavigationService _navigationService;

        public ShellViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            _navigationService.Navigated += (s, e) =>
            {
                NotifyOfPropertyChange(() => NavigationBackStack);
                NotifyOfPropertyChange(() => NavigationCurrentEntry);
            };
        }

        private ICommand _navigateBackCommand;

        public ICommand NavigateBackCommand
        {
            get
            {
                return _navigateBackCommand ??
                       (_navigateBackCommand = ActionCommand
                           .When(() => _navigationService.CanGoBack)
                           .Do(() =>
                           {
                               _navigationService.GoBack();
                           })
                           .RequeryOnPropertyChanged(_navigationService, () => _navigationService.CanGoBack));
            }
        }

        private ICommand _navigateForwardCommand;

        public ICommand NavigateForwardCommand
        {
            get
            {
                return _navigateForwardCommand ??
                       (_navigateForwardCommand = ActionCommand
                           .When(() => _navigationService.CanGoForward)
                           .Do(() =>
                           {
                               _navigationService.GoForward();
                           })
                           .RequeryOnPropertyChanged(_navigationService, () => _navigationService.CanGoForward));
            }
        }

        public IEnumerable<IScreen> NavigationBackStack
        {
            get { return _navigationService.BackStack.Select(x => x.Content).OfType<IScreen>(); }
        }

        public IScreen NavigationCurrentEntry
        {
            get
            {
                if (_navigationService.CurrentEntry == null)
                {
                    return null;
                }
                return _navigationService.CurrentEntry.Content as IScreen;
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