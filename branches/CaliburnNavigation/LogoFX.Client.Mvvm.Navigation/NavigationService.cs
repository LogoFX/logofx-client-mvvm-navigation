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
        private sealed class HistoryItem
        {
            /// <summary>
            /// Type of the navigation target.
            /// </summary>
            public Type Type { get; private set; }

            /// <summary>
            /// Gets the navigation argument.
            /// </summary>
            /// <value>
            /// The navigation argument.
            /// </value>
            public object Argument { get; private set; }

            /// <summary>
            /// Gets or sets the navigation target.
            /// </summary>
            /// <value>
            /// The navigation target.
            /// </value>
            public WeakReference Object { get; set; }

            /// <summary>
            /// Gets or sets a value indicating whether this <see cref="HistoryItem"/> should be skipped.
            /// </summary>
            /// <value>
            ///   <c>true</c> if skipped; otherwise, <c>false</c>.
            /// </value>
            public bool Skip { get; private set; }

            public HistoryItem(Type type, object argument, bool skip)
            {
                Type = type;
                Argument = argument;
                Skip = skip;
            }
        }

        #endregion

        #region Fields

        private int _stopTrack;
        private int _stopEvents;

        private int _currentIndex = -1;

        private readonly List<HistoryItem> _history =
            new List<HistoryItem>();

        private readonly Dictionary<Type, INavigationBuilder> _builders =
            new Dictionary<Type, INavigationBuilder>();

        #endregion

        #region Public Methods

        /// <summary>
        /// Registers the attribute.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="attribute">The attribute.</param>
        /// <param name="container">The container.</param>
        public void RegisterAttribute(Type type, NavigationViewModelAttribute attribute, IIocContainer container)
        {
            var types = new List<Type> { type };
            var synonymAttributes = type.GetTypeInfo().GetCustomAttributes<NavigationSynonymAttribute>(inherit: false);
            types.AddRange(synonymAttributes.Select(x => x.SynonymType));

            foreach (var t in types)
            {
                var builder = new AttributeBuilder(type, attribute, container);
                _builders.Add(t, builder);
            }
        }

        #endregion

        #region Private Members

        private NavigationParameter CreateParameter<T>(object argument)
        {
            return new NavigationParameter<T>(this, argument);
        }

        private INavigationConductor ActivateConductorAsync(NavigationMode mode, Type conductorType)
        {
            var builder = GetBuilder(conductorType);

            if (builder.IsRoot)
            {
                return (INavigationConductor)builder.GetValue();
            }

            INavigationConductor result;

            StopTrack = true;
            StopEvents = true;

            try
            {
                result = (INavigationConductor) NavigateInternal(mode, conductorType, null, true);
            }

            finally
            {
                StopEvents = false;
                StopTrack = false;
            }

            return result;
        }

        private object NavigateInternal(NavigationMode mode, Type itemType, object argument, bool noCheckHistory = false)
        {
            var sourcePageType = _sourcePageType;
            _sourcePageType = itemType;

            NavigationEventArgs navEventArgs = new NavigationEventArgs();
            navEventArgs.NavigationMode = mode;
            navEventArgs.Parameter = argument;
            navEventArgs.SourcePageType = itemType;

            var cancelEventArgs = new NavigatingCancelEventArgs(mode);
            OnNavigating(cancelEventArgs);
            if (cancelEventArgs.Cancel)
            {
                OnNavigationStopped(navEventArgs);
                _sourcePageType = sourcePageType;
                return null;
            }

            if (_currentIndex >= 0 && _history.Count > 0 && !noCheckHistory)
            {
                //if current is same v-m
                int index = _currentIndex;
                while (index >= 0 && _history[index].Skip)
                {
                    --index;
                }
                HistoryItem historyItem = _history[index];
                var obj = historyItem.Object.Target;
                if (historyItem.Type == itemType && historyItem.Argument == argument && obj != null)
                {
                    if (index != _currentIndex)
                    {
                        var obj2 = obj as INavigationViewModel;
                        if (!StopEvents && obj2 != null)
                        {
                            obj2.OnNavigated(NavigationMode.New, argument);
                        }
                        _currentIndex = index;
                        UpdateProperties();
                    }

                    _currentSourcePageType = itemType;
                    navEventArgs.Content = obj;
                    OnNavigated(navEventArgs);
                    return obj;
                }
            }

            var builder = GetBuilder(itemType);
            INavigationConductor conductor;
            try
            {
                conductor = ActivateConductorAsync(mode, builder.ConductorType);
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
            conductor.NavigateTo(viewModel, argument);

            var navigationViewModel = viewModel as INavigationViewModel;
            if (!StopEvents && navigationViewModel != null)
            {
                navigationViewModel.OnNavigated(NavigationMode.New, argument);
            }

            if (!StopTrack)
            {
                ++_currentIndex;
                Debug.Assert(_history.Count >= _currentIndex);
                if (_history.Count != _currentIndex)
                {
                    _history.RemoveRange(_currentIndex, _history.Count - _currentIndex);
                }

                var historyItem = new HistoryItem(itemType, argument, builder.NotRemember)
                {
                    Object = new WeakReference(viewModel)
                };
                _history.Insert(_currentIndex, historyItem);
                UpdateProperties();
            }

            _currentSourcePageType = itemType;
            navEventArgs.Content = viewModel;
            OnNavigated(navEventArgs);
            return viewModel;
        }

        /// <summary>
        /// Navigates forward.
        /// </summary>
        private void GoForwardInternal()
        {
            HistoryItem historyItem;
            do
            {
                ++_currentIndex;
                historyItem = _history[_currentIndex];
            } while (historyItem.Skip);

            UpdateProperties();
            StopTrack = true;
            try
            {
                NavigateInternal(NavigationMode.Forward, historyItem.Type, historyItem.Argument, true);
            }
            finally
            {
                StopTrack = false;
            }
        }

        /// <summary>
        /// Navigates back.
        /// </summary>
        private void GoBackInternal()
        {
            HistoryItem historyItem;

            do
            {
                --_currentIndex;
                historyItem = _history[_currentIndex];
            } while (historyItem.Skip);
            UpdateProperties();

            StopTrack = true;
            try
            {
                NavigateInternal(NavigationMode.Back, historyItem.Type, historyItem.Argument, true);
            }

            finally
            {
                StopTrack = false;
            }
        }

        private bool StopTrack
        {
            get { return _stopTrack > 0; }
            set
            {
                if (value)
                {
                    ++_stopTrack;
                }
                else
                {
                    --_stopTrack;
                }

                Debug.Assert(_stopTrack >= 0);
            }
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
        }

        private void OnNavigating(NavigatingCancelEventArgs e)
        {
            var handler = Navigating;

            if (handler == null)
            {
                return;
            }

            handler(this, e);
        }

        private void OnNavigated(NavigationEventArgs e)
        {
            var handler = Navigated;
            if (handler == null)
            {
                return;
            }

            handler(this, e);
        }

        private void OnNavigationStopped(NavigationEventArgs e)
        {
            var handler = NavigationStopped;
            if (handler == null)
            {
                return;
            }

            handler(this, e);
        }

        private void OnNavigationFailed(NavigationFailedEventArgs e)
        {
            var handler = NavigationFailed;
            if (handler == null)
            {
                return;
            }

            handler(this, e);
        }

        #endregion
    }
}