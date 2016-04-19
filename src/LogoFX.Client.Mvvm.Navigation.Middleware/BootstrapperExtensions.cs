using LogoFX.Bootstrapping;
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
        /// <typeparam name="TIocContainerAdapter">The type of the ioc container adapter.</typeparam>
        /// <typeparam name="TRootObject">The type of root object.</typeparam>
        /// <param name="bootstrapper">The bootstrapper.</param>
        /// <returns></returns>
        public static IBootstrapperWithContainerAdapter<TIocContainerAdapter>            
            UseNavigation<TIocContainerAdapter, TRootObject>(
            this IBootstrapperWithContainerAdapter<TIocContainerAdapter> bootstrapper)             
            where TIocContainerAdapter : class, IIocContainer
            where TRootObject : class
        {
            return bootstrapper.Use(new NavigationMiddleware<TIocContainerAdapter, TRootObject>());
        }
    }
}