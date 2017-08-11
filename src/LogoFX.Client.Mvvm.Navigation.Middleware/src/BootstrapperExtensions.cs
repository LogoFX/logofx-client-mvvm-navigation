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
        /// <typeparam name="TRootObject">The type of the root object.</typeparam>
        /// <typeparam name="TIocContainerAdapter">The type of the ioc container adapter.</typeparam>
        /// <param name="bootstrapper">The bootstrapper.</param>
        /// <returns></returns>
        public static IBootstrapperWithContainerAdapter<TIocContainerAdapter> 
            UseNavigation<TRootObject, TIocContainerAdapter>(
            this IBootstrapperWithContainerAdapter<TIocContainerAdapter> bootstrapper,
            IIocContainerResolver resolver) 
            where TRootObject : class 
            where TIocContainerAdapter : class, IIocContainer, IIocContainerAdapter, IBootstrapperAdapter, new()
        {
            return bootstrapper.Use(new NavigationMiddleware<TRootObject, TIocContainerAdapter>(resolver));
        }
    }
}