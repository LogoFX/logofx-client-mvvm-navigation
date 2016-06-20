using System;
using System.Collections.Generic;
using Solid.Practices.IoC;

namespace LogoFX.Client.Mvvm.Navigation
{
    public sealed partial class NavigationService : INavigationService
    {
        private Type _sourcePageType;
        private Type _currentSourcePageType;

        private event NavigatedEventHandler NavigatedInternal;
        private event NavigatingCancelEventHandler NavigatingInternal;
        private event NavigationFailedEventHandler NavigationFailedInternal;
        private event NavigationStoppedEventHandler NavigationStoppedInternal;

        IRootableNavigationBuilder<T> INavigationService.RegisterViewModel<T>(IIocContainerResolver resolver)
        {
            var builder = new GenericBuilder<T>(resolver);
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

        event NavigatedEventHandler INavigationService.Navigated
        {
            add { NavigatedInternal += value; }
            remove { NavigatedInternal -= value; }
        }

        event NavigatingCancelEventHandler INavigationService.Navigating
        {
            add { NavigatingInternal += value; }
            remove { NavigatingInternal -= value; }
        }

        event NavigationFailedEventHandler INavigationService.NavigationFailed
        {
            add { NavigationFailedInternal += value; }
            remove { NavigationFailedInternal -= value; }
        }

        event NavigationStoppedEventHandler INavigationService.NavigationStopped
        {
            add { NavigationStoppedInternal += value; }
            remove { NavigationStoppedInternal -= value; }
        }

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
            get { return _forwardStack.Count > 0; }
        }

        bool INavigationService.CanGoBack
        {
            get { return _backStack.Count > 0; }
        }

        bool INavigationService.Navigate(Type sourcePageType)
        {
            return NavigateInternal(NavigationMode.New, sourcePageType, null) != null;
        }

        bool INavigationService.Navigate(Type sourcePageType, object parameter)
        {
            return NavigateInternal(NavigationMode.New, sourcePageType, parameter) != null;
        }

        bool INavigationService.Navigate(INavigationStackEntry stackEntry)
        {
            var index = _backStack.IndexOf(stackEntry);
            if (index >= 0)
            {
                _currentEntry = null;
                _backStack.RemoveRange(index, _backStack.Count - index);
            }

            return NavigateInternal(NavigationMode.New, stackEntry.Type, stackEntry.Parameter) != null;
        }

        void INavigationService.GoForward()
        {
            GoForwardInternal();
        }

        void INavigationService.GoBack()
        {
            GoBackInternal();
        }

        IList<INavigationStackEntry> INavigationService.BackStack
        {
            get { return _backStack; }
        }

        INavigationStackEntry INavigationService.CurrentEntry
        {
            get { return _currentEntry; }
        }

        IList<INavigationStackEntry> INavigationService.ForwardStack
        {
            get { return _forwardStack; }
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