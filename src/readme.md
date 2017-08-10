# logofx-client-mvvm-navigation
Navigation facilities

## .NET 4.5 usage

* Add new project of type "WPF Application".
* Add folowing Nuget packages:
  * [LogoFX.Client.Mvvm.Navigation](http://www.nuget.org/packages/LogoFX.Client.Mvvm.Navigation/) with its dependencies.
  * [LogoFX.Client.Bootstrapping.Adapters](https://www.nuget.org/packages/LogoFX.Client.Bootstrapping.Adapters.SimpleContainer/) with its dependencies.
  * [LogoFX.Client.Mvvm.ViewModel.Services.Core](https://www.nuget.org/packages/LogoFX.Client.Mvvm.ViewModel.Services.Core/) with its dependencies.
* Add ViewModels and Views project folders.
* Add ShellViewModel class and ShellView window.

*ShellViewModel class sample*

```C#
public sealed class ShellViewModel : Conductor<IScreen>, INavigationConductor
{
    private readonly INavigationService _navigationService;

    public ShellViewModel(INavigationService navigationService)
    {
        _navigationService = navigationService;
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
```

* Add AppBootstrapper class
```C#
using LogoFX.Client.Bootstrapping;
using LogoFX.Client.Bootstrapping.Adapters.SimpleContainer;
using LogoFX.Client.Mvvm.Navigation.Samples.Wpf.ViewModels;
using LogoFX.Client.Mvvm.ViewModel.Services;

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
            this.UseResolver();
            this.UseViewModelCreatorService();
            this.UseNavigation<ShellViewModel, ExtendedSimpleContainerAdapter>(s_containerAdapter);
        }
    }
}
```

* Remove project files: MainWindow.xaml, MainWindow.xaml.cs. Remove StartupUri tag in App.xaml.
* Add constructor for App class
```C#
        public App()
        {
            var appBootstrapper = new AppBootstrapper();
            appBootstrapper.Initialize();
        }
```
* Add all needed views and view-models.

## UWP usage

* Add new project of type "Blank App (Universal Windows)".
* Add folowing Nuget packages:
  * [LogoFX.Client.Mvvm.Navigation](http://www.nuget.org/packages/LogoFX.Client.Mvvm.Navigation/) with its dependencies.
  * [LogoFX.Client.Bootstrapping.Adapters](https://www.nuget.org/packages/LogoFX.Client.Bootstrapping.Adapters.SimpleContainer/) with its dependencies.
  * [LogoFX.Client.Mvvm.ViewModel.Services.Core](https://www.nuget.org/packages/LogoFX.Client.Mvvm.ViewModel.Services.Core/) with its dependencies.
* Add ViewModels and Views project folders.
* Add ShellViewModel class and ShellView window.

*ShellViewModel class sample*

```C#
public sealed class ShellViewModel : Conductor<IScreen>, INavigationConductor
{
    private readonly INavigationService _navigationService;

    public ShellViewModel(INavigationService navigationService)
    {
        _navigationService = navigationService;
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
```

* Add AppBootstrapper class
```C#
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
```

* Remove project files: MainWindow.xaml, MainWindow.xaml.cs. Remove StartupUri tag in App.xaml.
* Made App class inherited from AppBootstrapper:
```XAML
<local:AppBootstrapper
    x:Class="LogoFX.Client.Mvvm.Navigation.Samples.Uwp.App"
    xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
    xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
    xmlns:local="using:LogoFX.Client.Mvvm.Navigation.Samples.Uwp"
    RequestedTheme="Light">

</local:AppBootstrapper>
```
* Add constructor for App class
```C#
        public App()
        {
            InitializeComponent();
            Initialize();
        }
```
* Add all needed views and view-models.
