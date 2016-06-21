using System;
using System.Windows.Input;
using Caliburn.Micro;
using JetBrains.Annotations;
using LogoFX.Client.Mvvm.Commanding;
using LogoFX.Client.Mvvm.ViewModel.Services;

namespace LogoFX.Client.Mvvm.Navigation.Samples.Uwp.ViewModels
{
    [UsedImplicitly]
    [NavigationViewModel(ConductorType = typeof(ShellViewModel))]
    public sealed class MainViewModel : Conductor<IScreen>, INavigationConductor
    {
        private readonly INavigationService _navigationService;

        public MainViewModel(
            IViewModelCreatorService viewModelCreatorService,
            INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        private ICommand _navigateCommand;

        public ICommand NavigateCommand
        {
            get
            {
                return _navigateCommand ??
                       (_navigateCommand = ActionCommand<string>
                           .Do(t =>
                           {
                               t = string.Format("{0}.{1}", GetType().Namespace, t);
                               var type = Type.GetType(t);
                               _navigationService.Navigate(type);
                           }));
            }
        }

        public override string DisplayName
        {
            get { return "Main"; }
            set { }
        }

        public void NavigateTo(object viewModel, object argument)
        {
            ActivateItem((IScreen)viewModel);
        }
    }
}