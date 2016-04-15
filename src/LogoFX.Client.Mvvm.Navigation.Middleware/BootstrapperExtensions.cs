using LogoFX.Bootstrapping;
using LogoFX.Client.Bootstrapping.Adapters.Contracts;
using Solid.Practices.IoC;

namespace LogoFX.Client.Mvvm.Navigation
{
    /// <summary>
    /// Bootstrapper extensions.
    /// </summary>
    public static class BootstrapperExtensions
    {
        /// <summary>
        /// Uses the navigation middleware.
        /// </summary>
        /// <typeparam name="TRootViewModel">The type of the root view model.</typeparam>
        /// <typeparam name="TIocContainerAdapter">The type of the ioc container adapter.</typeparam>
        /// <param name="bootstrapper">The bootstrapper.</param>
        /// <returns></returns>
        public static IBootstrapperWithContainerAdapter<TRootViewModel, TIocContainerAdapter>            
            UseNavigation<TRootViewModel, TIocContainerAdapter>(
            this IBootstrapperWithContainerAdapter<TRootViewModel, TIocContainerAdapter> bootstrapper) 
            where TRootViewModel : class 
            where TIocContainerAdapter : class, IIocContainer, IIocContainerAdapter, IBootstrapperAdapter, new()
        {
            return bootstrapper.Use(new NavigationMiddleware<TRootViewModel, TIocContainerAdapter>());
        }
    }
}