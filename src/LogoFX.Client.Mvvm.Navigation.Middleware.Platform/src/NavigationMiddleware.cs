using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using LogoFX.Client.Bootstrapping;
using LogoFX.Client.Bootstrapping.Adapters.Contracts;
using Solid.Practices.IoC;
using Solid.Practices.Middleware;

namespace LogoFX.Client.Mvvm.Navigation
{
    /// <summary>
    /// Navigation middleware.
    /// </summary>
    /// <typeparam name="TRootViewModel">The type of the root view model.</typeparam>
    /// <typeparam name="TIocContainerAdapter">The type of the ioc container adapter.</typeparam>    
    public class NavigationMiddleware<TRootViewModel, TIocContainerAdapter> : 
        IMiddleware<BootstrapperContainerBase<TRootViewModel, TIocContainerAdapter>>        
        where TRootViewModel : class
        where TIocContainerAdapter : class, IIocContainerAdapter, IIocContainer, IBootstrapperAdapter, new()
    {
        private NavigationService _navigationService = new NavigationService();
        private INavigationService NavigationService
        {
            get { return _navigationService ?? (_navigationService = new NavigationService()); }
        }       

        /// <summary>
        /// Override this method to inject custom logic during root view model registration.
        /// </summary>
        /// <param name="navigationService"></param>
        /// <param name="container"></param>
        public virtual void OnRegisterRoot(INavigationService navigationService, IIocContainer container)
        {
            navigationService.RegisterViewModel<TRootViewModel>(container).AsRoot();
        }

        /// <summary>
        /// Override this method to inject custom logic during navigation view models registration.
        /// </summary>
        /// <param name="navigationService">The navigation service.</param>
        /// <param name="container">The IoC container.</param>
        /// <param name="assemblies">The list of assemblies to be inspected.</param>
        protected virtual void OnPrepareNavigation(
            INavigationService navigationService, 
            IIocContainer container, 
            IEnumerable<Assembly> assemblies)
        {
            RegisterNavigationViewModels(container, assemblies);
        }

        /// <summary>
        /// Applies the middleware on the specified object.
        /// </summary>
        /// <param name="object">The object.</param>
        /// <returns></returns>
        public BootstrapperContainerBase<TRootViewModel, TIocContainerAdapter> Apply(
            BootstrapperContainerBase<TRootViewModel, TIocContainerAdapter> @object)
        {            
            @object.ContainerAdapter.RegisterInstance(NavigationService);
            OnRegisterRoot(NavigationService, @object.ContainerAdapter);
            OnPrepareNavigation(NavigationService, @object.ContainerAdapter, @object.Assemblies);
            return @object;
        }

        private void RegisterNavigationViewModels(IIocContainer container, IEnumerable<Assembly> assemblies)
        {
            var viewModelTypes = assemblies.ToArray()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => type != typeof(TRootViewModel) && type.GetTypeInfo().IsClass)
                .Select(type => new
                {
                    Type = type,
                    Attr = type.GetTypeInfo().GetCustomAttribute<NavigationViewModelAttribute>()
                })
                .Where(x => x.Attr != null);

            foreach (var viewModelType in viewModelTypes)
            {
                _navigationService.RegisterAttribute(viewModelType.Type, viewModelType.Attr, container);
            }
        }        
    }
}