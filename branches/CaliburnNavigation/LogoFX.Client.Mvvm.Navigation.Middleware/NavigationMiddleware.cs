using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using LogoFX.Bootstrapping;
using LogoFX.Client.Bootstrapping.Adapters.Contracts;
using Solid.Practices.IoC;
using Solid.Practices.Middleware;

namespace LogoFX.Client.Mvvm.Navigation
{
    /// <summary>
    /// Navigation middleware.
    /// </summary>
    /// <typeparam name="TRootObject">The type of the root object.</typeparam>
    /// <typeparam name="TIocContainerAdapter">The type of the ioc container adapter.</typeparam>    
    public class NavigationMiddleware<TRootObject, TIocContainerAdapter> : 
        IMiddleware<IBootstrapperWithContainerAdapter<TIocContainerAdapter>>        
        where TRootObject : class
        where TIocContainerAdapter : class, IIocContainerAdapter, IIocContainer, IIocContainerResolver, IBootstrapperAdapter, new()
    {
        private readonly IIocContainerResolver _resolver;

        public NavigationMiddleware(IIocContainerResolver resolver)
        {
            _resolver = resolver;
        }

        private NavigationService _navigationService = new NavigationService();
        private INavigationService NavigationService
        {
            get { return _navigationService ?? (_navigationService = new NavigationService()); }
        }

        /// <summary>
        /// Override this method to inject custom logic during root view model registration.
        /// </summary>
        /// <param name="navigationService"></param>
        /// <param name="resolver"></param>
        protected virtual void OnRegisterRoot(INavigationService navigationService, IIocContainerResolver resolver)
        {
            navigationService.RegisterViewModel<TRootObject>(resolver).AsRoot();
        }

        /// <summary>
        /// Override this method to inject custom logic during navigation view models registration.
        /// </summary>
        /// <param name="navigationService">The navigation service.</param>
        /// <param name="resolver">The IoC container resolver.</param>
        /// <param name="assemblies">The list of assemblies to be inspected.</param>
        protected virtual void OnPrepareNavigation(INavigationService navigationService, IIocContainerResolver resolver, IEnumerable<Assembly> assemblies)
        {
            RegisterNavigationViewModels(resolver, assemblies);
        }

        /// <summary>
        /// Applies the middleware on the specified object.
        /// </summary>
        /// <param name="object">The object.</param>
        /// <returns></returns>
        public IBootstrapperWithContainerAdapter<TIocContainerAdapter> Apply(IBootstrapperWithContainerAdapter<TIocContainerAdapter> @object)
        {            
            @object.Registrator.RegisterInstance(NavigationService);
            OnRegisterRoot(NavigationService, _resolver);
            OnPrepareNavigation(NavigationService, _resolver, @object.Assemblies);
            return @object;
        }

        private void RegisterNavigationViewModels(IIocContainerResolver resolver, IEnumerable<Assembly> assemblies)
        {
            var viewModelTypes = assemblies.ToArray()
                .SelectMany(assembly => assembly.DefinedTypes)
                .Where(typeInfo => typeInfo.Equals(typeof(TRootObject).GetTypeInfo()) == false && typeInfo.IsClass)
                .Select(typeInfo => new
                {
                    Type = typeInfo.AsType(),
                    Attr = typeInfo.GetCustomAttribute<NavigationViewModelAttribute>()
                })
                .Where(x => x.Attr != null);

            foreach (var viewModelType in viewModelTypes)
            {
                _navigationService.RegisterAttribute(viewModelType.Type, viewModelType.Attr, resolver);
            }
        }        
    }
}