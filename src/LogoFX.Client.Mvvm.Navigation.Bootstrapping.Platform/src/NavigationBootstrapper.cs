using LogoFX.Client.Bootstrapping;
using LogoFX.Client.Bootstrapping.Adapters.Contracts;
using Solid.Practices.IoC;

namespace LogoFX.Client.Mvvm.Navigation
{
    /// <summary>
    /// Navigation bootstrapper which registers the common navigation facilities into the IoC Container; 
    /// this includes the navigation view models
    /// </summary>
    /// <typeparam name="TRootViewModel">The type of the root view model.</typeparam>
    /// <typeparam name="TIocContainerAdapter">The type of the IoC container adapter.</typeparam>
    public class NavigationBootstrapper<TRootViewModel, TIocContainerAdapter> : 
        BootstrapperContainerBase<TRootViewModel, TIocContainerAdapter> 
        where TRootViewModel : class 
        where TIocContainerAdapter : class, IIocContainer, IIocContainerAdapter, IBootstrapperAdapter, new()
    {        
        /// <summary>
        /// Initializes a new instance of the <see cref="NavigationBootstrapper{TRootViewModel, TIocContainerAdapter}"/> class.
        /// </summary>
        /// <param name="iocContainerAdapter">The ioc container adapter.</param>        
        protected NavigationBootstrapper(TIocContainerAdapter iocContainerAdapter)
            : this(iocContainerAdapter, new BootstrapperCreationOptions())
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NavigationBootstrapper{TRootViewModel, TIocContainerAdapter}"/> class.
        /// </summary>
        /// <param name="iocContainerAdapter">The ioc container adapter.</param>
        /// <param name="creationOptions">The creation options.</param>
        protected NavigationBootstrapper(TIocContainerAdapter iocContainerAdapter, BootstrapperCreationOptions creationOptions)
            :base(iocContainerAdapter, creationOptions)
        {
            this.UseNavigation();
        }
    }
}