using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using LogoFX.Client.Core;
using Solid.Practices.IoC;

namespace LogoFX.Client.Mvvm.Navigation
{
    public sealed partial class NavigationService : NotifyPropertyChangedBase<NavigationService>
    {
        #region Nested Types

        /// <summary>
        /// The History Item.
        /// </summary>
        private sealed class HistoryItem : INavigationStackEntry
        {
            /// <summary>
            /// Type of the navigation target.
            /// </summary>
            public Type Type { get; private set; }

            /// <summary>
            /// Gets or sets the navigation target.
            /// </summary>
            /// <value>
            /// The navigation target.
            /// </value>
            public object Content { get; set; }

            /// <summary>
            /// Gets the navigation parameter.
            /// </summary>
            /// <value>
            /// The navigation parameter.
            /// </value>
            public object Parameter { get; private set; }

            public HistoryItem(Type type, object parameter)
            {
                Type = type;
                Parameter = parameter;
            }
        }

        #endregion

        #region Fields

        private int _stopEvents;

        private INavigationStackEntry _currentEntry;

        private readonly List<INavigationStackEntry> _backStack =
            new List<INavigationStackEntry>();

        private readonly List<INavigationStackEntry> _forwardStack =
            new List<INavigationStackEntry>();

        private readonly Dictionary<Type, INavigationBuilder> _builders =
            new Dictionary<Type, INavigationBuilder>();

        #endregion

        #region Public Methods

        /// <summary>
        /// Registers the attribute.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="attribute">The attribute.</param>
        /// <param name="resolver">The IoC container resolver.</param>
        public void RegisterAttribute(Type type, NavigationViewModelAttribute attribute, IDependencyResolver resolver)
        {
            var types = new List<Type> { type };
            var synonymAttributes = type.GetTypeInfo().GetCustomAttributes<NavigationSynonymAttribute>(inherit: false);
            types.AddRange(synonymAttributes.Select(x => x.SynonymType));

            foreach (var t in types)
            {
                var builder = new AttributeBuilder(type, attribute, resolver);
                _builders.Add(t, builder);
            }
        }

        #endregion

        #region Private Members

        private NavigationParameter CreateParameter<T>(object argument)
        {
            return new NavigationParameter<T>(this, argument);
        }

        private INavigationConductor ActivateConductorAsync(Type conductorType)
        {
            var builder = GetBuilder(conductorType);

            if (builder.IsRoot)
            {
                return (INavigationConductor)builder.GetValue();
            }

            INavigationConductor result;

            StopEvents = true;

            try
            {
                result = (INavigationConductor) NavigateInternal(NavigationMode.Refresh, conductorType, null, true);
            }

            finally
            {
                StopEvents = false;
            }

            return result;
        }

        private object NavigateInternal(NavigationMode mode, Type itemType, object parameter, bool noCheckHistory = false)
        {
            NavigationEventArgs navEventArgs = new NavigationEventArgs
            {
                NavigationMode = mode,
                Parameter = parameter,
                SourcePageType = itemType
            };

            if (mode != NavigationMode.Refresh)
            {
                var sourcePageType = _sourcePageType;
                _sourcePageType = itemType;

                var cancelEventArgs = new NavigatingCancelEventArgs(mode);
                OnNavigating(cancelEventArgs);
                if (cancelEventArgs.Cancel)
                {
                    OnNavigationStopped(navEventArgs);
                    _sourcePageType = sourcePageType;
                    return null;
                }

                if (!noCheckHistory && _currentEntry != null)
                {
                    //if current is same v-m
                    var obj = _currentEntry.Content;
                    if (_currentEntry.Type == itemType &&
                        _currentEntry.Parameter == parameter &&
                        obj != null)
                    {
                        _currentSourcePageType = itemType;
                        UpdateProperties();

                        navEventArgs.Content = obj;
                        OnNavigated(navEventArgs);
                        return obj;
                    }
                }
            }

            var builder = GetBuilder(itemType);
            INavigationConductor conductor;
            try
            {
                conductor = ActivateConductorAsync(builder.ConductorType);
            }

            catch (Exception err)
            {
                //Trace.TraceError("ActivateConductorAsync throws error: {0}", err);
                var failedEventArgs = new NavigationFailedEventArgs(err);
                OnNavigationFailed(failedEventArgs);
                if (failedEventArgs.Handled)
                {
                    return null;
                }
                throw;
            }

            object viewModel = builder.GetValue();
            conductor.NavigateTo(viewModel, parameter);

            var navigationViewModel = viewModel as INavigationViewModel;
            if (!StopEvents && navigationViewModel != null)
            {
                navigationViewModel.OnNavigated(NavigationMode.New, parameter);
            }

            switch (mode)
            {
                case NavigationMode.New:
                    _forwardStack.Clear();
                    if (_currentEntry != null)
                    {
                        _backStack.Add(_currentEntry);
                    }
                    _currentEntry = new HistoryItem(itemType, parameter)
                    {
                        Content = viewModel
                    };
                    break;
                case NavigationMode.Back:
                    _forwardStack.Add(_currentEntry);
                    _currentEntry = _backStack.Last();
                    _backStack.Remove(_currentEntry);
                    break;
                case NavigationMode.Forward:
                    _backStack.Add(_currentEntry);
                    _currentEntry = _forwardStack.First();
                    _forwardStack.Remove(_currentEntry);
                    break;
                case NavigationMode.Refresh:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(mode), mode, null);
            }

            _currentSourcePageType = itemType;

            UpdateProperties();

            navEventArgs.Content = viewModel;
            OnNavigated(navEventArgs);
            return viewModel;
        }

        /// <summary>
        /// Navigates forward.
        /// </summary>
        private void GoForwardInternal()
        {
            var historyItem = _forwardStack.First();
            NavigateInternal(NavigationMode.Forward, historyItem.Type, historyItem.Parameter, true);
        }

        /// <summary>
        /// Navigates back.
        /// </summary>
        private void GoBackInternal()
        {
            var historyItem = _backStack.Last();
            NavigateInternal(NavigationMode.Back, historyItem.Type, historyItem.Parameter, true);
        }

        private bool StopEvents
        {
            get { return _stopEvents > 0; }
            set
            {
                if (value)
                {
                    ++_stopEvents;
                }
                else
                {
                    --_stopEvents;
                }

                Debug.Assert(_stopEvents >= 0);
            }
        }

        private INavigationBuilder GetBuilder(Type type)
        {
            INavigationBuilder navigationBuilder;

            if (!_builders.TryGetValue(type, out navigationBuilder))
            {
                throw new UnregisteredTypeException($"Not registered type '{type}'.");
            }

            return navigationBuilder;
        }

        private void UpdateProperties()
        {
            NotifyOfPropertyChange(() => ((INavigationService) this).CanGoBack);
            NotifyOfPropertyChange(() => ((INavigationService) this).CanGoForward);
            NotifyOfPropertyChange(() => ((INavigationService) this).SourcePageType);
            NotifyOfPropertyChange(() => ((INavigationService) this).CurrentSourcePageType);
            NotifyOfPropertyChange(() => ((INavigationService) this).BackStack);
            NotifyOfPropertyChange(() => ((INavigationService) this).CurrentEntry);
            NotifyOfPropertyChange(() => ((INavigationService) this).ForwardStack);
        }

        private void OnNavigating(NavigatingCancelEventArgs e)
        {
            var handler = NavigatingInternal;

            if (handler == null)
            {
                return;
            }

            handler(this, e);
        }

        private void OnNavigated(NavigationEventArgs e)
        {
            var handler = NavigatedInternal;
            if (handler == null)
            {
                return;
            }

            handler(this, e);
        }

        private void OnNavigationStopped(NavigationEventArgs e)
        {
            var handler = NavigationStoppedInternal;
            if (handler == null)
            {
                return;
            }

            handler(this, e);
        }

        private void OnNavigationFailed(NavigationFailedEventArgs e)
        {
            var handler = NavigationFailedInternal;
            if (handler == null)
            {
                return;
            }

            handler(this, e);
        }

        #endregion
    }
}