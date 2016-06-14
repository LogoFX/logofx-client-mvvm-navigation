﻿using LogoFX.Client.Bootstrapping;
using LogoFX.Client.Bootstrapping.Adapters.SimpleContainer;
using LogoFX.Client.Mvvm.Navigation.Samples.Wpf.ViewModels;

namespace LogoFX.Client.Mvvm.Navigation.Samples.Wpf
{
    public class AppBootstrapper : BootstrapperContainerBase<ExtendedSimpleContainerAdapter>
        .WithRootObject<ShellViewModel>
    {
        private static readonly ExtendedSimpleContainerAdapter s_containerAdapter =
            new ExtendedSimpleContainerAdapter();

        public AppBootstrapper()
            : base(s_containerAdapter)
        {
            this.UseNavigation<ShellViewModel, ExtendedSimpleContainerAdapter>(s_containerAdapter);
        }
    }
}