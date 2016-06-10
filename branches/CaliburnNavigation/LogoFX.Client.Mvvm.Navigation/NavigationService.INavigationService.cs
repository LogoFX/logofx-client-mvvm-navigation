using System;
using Solid.Practices.IoC;

namespace LogoFX.Client.Mvvm.Navigation
{
    public sealed partial class NavigationService : INavigationService
    {
        private Type _sourcePageType;
        private Type _currentSourcePageType;

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

        Type INavigationService.SourcePageType
        {
            get { return _sourcePageType; }
            set
            {
                if (_sourcePageType == value)
                {
                    return;
                }

                NavigateInternal(NavigationMode.New, value, null);
            }
        }

        Type INavigationService.CurrentSourcePageType
        {
            get { return _currentSourcePageType; }
        }

        bool INavigationService.CanGoForward
        {
            get
            {
                int index = _currentIndex + 1;
                while (index < _history.Count && _history[index].Skip)
                {
                    ++index;
                }
                return index < _history.Count;
            }
        }

        bool INavigationService.CanGoBack
        {
            get
            {
                int index = _currentIndex - 1;
                while (index > 0 && _history[index].Skip)
                {
                    --index;
                }
                return index >= 0;
            }
        }

        bool INavigationService.Navigate(Type sourcePageType)
        {
            return NavigateInternal(NavigationMode.New, sourcePageType, null) != null;
        }

        bool INavigationService.Navigate(Type sourcePageType, object parameter)
        {
            return NavigateInternal(NavigationMode.New, sourcePageType, parameter) != null;
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