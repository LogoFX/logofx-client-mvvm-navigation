using System;
using System.Collections.Generic;
using System.ComponentModel;
using Solid.Practices.IoC;

namespace LogoFX.Client.Mvvm.Navigation
{
    public interface INavigationService : INotifyPropertyChanged
    {
        /// <summary>
        /// Registers the view model for navigation using container-resolution strategy
        /// </summary>
        /// <typeparam name="T">Type of view model</typeparam>
        /// <param name="resolver">The IoC container resolver.</param>
        /// <returns></returns>
        IRootableNavigationBuilder<T> RegisterViewModel<T>(IDependencyResolver resolver) where T : class;

        /// <summary>
        /// Registers the view model instance for navigation.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="viewModel">The view model.</param>
        /// <returns></returns>
        IRootableNavigationBuilder<T> RegisterViewModel<T>(T viewModel);

        /// <summary>
        /// Registers the view model creator function.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="createFunc">The create function.</param>
        /// <returns></returns>
        IRootableNavigationBuilder<T> RegisterViewModel<T>(Func<T> createFunc);

        /// <summary>
        /// Registers the attribute.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="attribute">The attribute.</param>
        /// <param name="resolver">The IoC container resolver.</param>
        void RegisterAttribute(Type type, NavigationViewModelAttribute attribute, IDependencyResolver resolver);

        /// <summary>
        /// Creates navigation parameter using navigation target and specified argument.
        /// </summary>
        /// <param name="argument"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        NavigationParameter CreateParameter<T>(object argument);

        /// <summary>
        /// Creates navigation parameter using navigation target.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        NavigationParameter CreateParameter<T>();


        /// <summary>
        ///   Raised after navigation.
        /// </summary>
        event NavigatedEventHandler Navigated;

        /// <summary>
        ///   Raised prior to navigation.
        /// </summary>
        event NavigatingCancelEventHandler Navigating;

        /// <summary>
        ///   Raised when navigation fails.
        /// </summary>
        event NavigationFailedEventHandler NavigationFailed;

        /// <summary>
        ///   Raised when navigation is stopped.
        /// </summary>
        event NavigationStoppedEventHandler NavigationStopped;

        /// <summary>
        /// Gets or sets the data type of the current content, or the content that should be navigated to.
        /// </summary>
        Type SourcePageType { get; set; }

        /// <summary>
        /// Gets the data type of the content that is currently displayed.
        /// </summary>
        Type CurrentSourcePageType { get; }

        /// <summary>
        ///   Indicates whether the navigator can navigate forward.
        /// </summary>
        bool CanGoForward { get; }

        /// <summary>
        ///   Indicates whether the navigator can navigate back.
        /// </summary>
        bool CanGoBack { get; }

        /// <summary>
        ///   Navigates to the specified content.
        /// </summary>
        /// <param name="sourcePageType"> The <see cref="System.Type" /> to navigate to. </param>
        /// <returns> Whether or not navigation succeeded. </returns>
        bool Navigate(Type sourcePageType);

        /// <summary>
        ///   Navigates to the specified content.
        /// </summary>
        /// <param name="sourcePageType"> The <see cref="System.Type" /> to navigate to. </param>
        /// <param name="parameter">The object parameter to pass to the target.</param>
        /// <returns> Whether or not navigation succeeded. </returns>
        bool Navigate(Type sourcePageType, object parameter);

        /// <summary>
        /// Navigates the specified stack entry.
        /// </summary>
        /// <param name="stackEntry">The stack entry.</param>
        /// <returns>Whether or not navigation succeeded.</returns>
        bool Navigate(INavigationStackEntry stackEntry);

        /// <summary>
        ///   Navigates forward.
        /// </summary>
        void GoForward();

        /// <summary>
        ///   Navigates back.
        /// </summary>
        void GoBack();

        /// <summary>
        /// Gets a collection of PageStackEntry instances representing the backward navigation history.
        /// </summary>
        IList<INavigationStackEntry> BackStack { get; }

        INavigationStackEntry CurrentEntry { get; }

        /// <summary>
        /// Gets a collection of PageStackEntry instances representing the forward navigation history.
        /// </summary>
        IList<INavigationStackEntry> ForwardStack { get; }

        /// <summary>
        /// Stores the frame navigation state in local settings if it can.
        /// </summary>
        /// <returns>Whether the suspension was sucessful</returns>
        //bool SuspendState();

        /// <summary>
        /// Tries to restore the frame navigation state from local settings.
        /// </summary>
        /// <returns>Whether the restoration of successful.</returns>
        //bool ResumeState();
    }
}