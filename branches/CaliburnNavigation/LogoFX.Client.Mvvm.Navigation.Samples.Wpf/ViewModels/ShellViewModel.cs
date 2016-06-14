using Caliburn.Micro;
using JetBrains.Annotations;
using LogoFX.Client.Mvvm.ViewModel.Services;

namespace LogoFX.Client.Mvvm.Navigation.Samples.Wpf.ViewModels
{
    [UsedImplicitly]
    public sealed class ShellViewModel : Conductor<IScreen>
    {
        private readonly IViewModelCreatorService _viewModelCreatorService;

        public ShellViewModel(IViewModelCreatorService viewModelCreatorService)
        {
            _viewModelCreatorService = viewModelCreatorService;
        }

        public override string DisplayName
        {
            get { return "Navigation Sample - WPF Application"; }
            set { }
        }

        protected override void OnActivate()
        {
            base.OnActivate();

            var mainViewModel = _viewModelCreatorService.CreateViewModel<MainViewModel>();
            ActivateItem(mainViewModel);
        }
    }
}