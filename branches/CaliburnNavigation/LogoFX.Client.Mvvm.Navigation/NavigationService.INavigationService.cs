using System;
using Solid.Practices.IoC;

namespace LogoFX.Client.Mvvm.Navigation
{
    public sealed partial class NavigationService : INavigationService
    {
        IRootableNavigationBuilder<T> INavigationService.RegisterViewModel<T>(IIocContainer container)
        {
            var builder = new GenericBuilder<T>(container);
            _builders.Add(typeof(T), builder);
            return builder;
        }

        IRootableNavigationBuilder<T> INavigationService.RegisterViewModel<T>(T viewModel)
        {
            var builder = new InstanceBuilder<T>(this, viewModel);
            _builders[viewModel.GetType()] = builder;
            return builder;
        }

        IRootableNavigationBuilder<T> INavigationService.RegisterViewModel<T>(Func<T> createFunc)
        {
            var builder = new ResolverBuilder<T>(this, createFunc);
            _builders.Add(typeof(T), builder);
            return builder;
        }

        NavigationParameter INavigationService.CreateParameter<T>(object argument)
        {
            return CreateParameter<T>(argument);
        }

        NavigationParameter INavigationService.CreateParameter<T>()
        {
            return CreateParameter<T>(null);
        }

        public event NavigatedEventHandler Navigated;

        public event NavigatingCancelEventHandler Navigating;

        public event NavigationFailedEventHandler NavigationFailed;

        public event NavigationStoppedEventHandler NavigationStopped;

        Type INavigationService.SourcePageType { get; set; }

        Type INavigationService.CurrentSourcePageType { get; }

        bool INavigationService.CanGoForward { get; }

        bool INavigationService.CanGoBack { get; }

        bool INavigationService.Navigate(Type sourcePageType)
        {
            return NavigateInternal(sourcePageType, null) != null;
        }

        bool INavigationService.Navigate(Type sourcePageType, object parameter)
        {
            return NavigateInternal(sourcePageType, parameter) != null;
        }

        void INavigationService.GoForward()
        {
            GoForwardInternal();
        }

        void INavigationService.GoBack()
        {
            GoBackInternal();
        }

        bool INavigationService.SuspendState()
        {
            throw new NotImplementedException();
        }

        bool INavigationService.ResumeState()
        {
            throw new NotImplementedException();
        }
    }
}