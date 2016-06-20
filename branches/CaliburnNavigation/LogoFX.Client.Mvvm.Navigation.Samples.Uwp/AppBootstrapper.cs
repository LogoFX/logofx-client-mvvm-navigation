using LogoFX.Bootstrapping;
using LogoFX.Client.Bootstrapping;
using LogoFX.Client.Bootstrapping.Adapters.SimpleContainer;
using LogoFX.Client.Mvvm.Navigation.Samples.Uwp.ViewModels;
using LogoFX.Client.Mvvm.ViewModel.Services;

namespace LogoFX.Client.Mvvm.Navigation.Samples.Uwp
{
    public abstract class AppBootstrapper : BootstrapperContainerBase<ExtendedSimpleContainerAdapter>
        .WithRootObject<ShellViewModel>
    {
        private static readonly ExtendedSimpleContainerAdapter s_iocContainer = new ExtendedSimpleContainerAdapter();

        protected AppBootstrapper()
            : base(s_iocContainer)
        {
            this.UseResolver(s_iocContainer);
            this.UseViewModelCreatorService();
            this.UseNavigation<ShellViewModel, ExtendedSimpleContainerAdapter>(s_iocContainer);
        }
    }
}