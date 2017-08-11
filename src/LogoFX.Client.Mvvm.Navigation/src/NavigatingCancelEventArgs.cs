using System.ComponentModel;

namespace LogoFX.Client.Mvvm.Navigation
{
    public sealed class NavigatingCancelEventArgs : CancelEventArgs
    {
        /// <summary>
        /// Gets a value that indicates the type of navigation that is occurring.
        /// </summary>
        /// 
        /// <returns>
        /// A value that indicates the type of navigation (<see cref="F:System.Windows.Navigation.NavigationMode.Back"/>, <see cref="F:System.Windows.Navigation.NavigationMode.Forward"/>, or <see cref="F:System.Windows.Navigation.NavigationMode.New"/>) that is occurring.
        /// </returns>
        public NavigationMode NavigationMode { get; private set; }

        /// <summary>
        /// Gets a value that indicates whether you can cancel the navigation.
        /// </summary>
        /// 
        /// <returns>
        /// true if you can cancel the navigation; otherwise, false.
        /// </returns>
        public bool IsCancelable { get; private set; }

        /// <summary>
        /// Gets a value that indicates whether the current application is the origin and destination of the navigation.
        /// </summary>
        /// 
        /// <returns>
        /// true if the navigation starts and ends within the current application; false if the navigation starts or ends at an external location.
        /// </returns>
        public bool IsNavigationInitiator { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Windows.Navigation.NavigatingCancelEventArgs"/> class, based on mode.
        /// </summary>
        /// <param name="mode">A value that indicates the type of navigation that is occurring.</param>
        public NavigatingCancelEventArgs(NavigationMode mode)
            :this(mode, true, true)
        {
            
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Windows.Navigation.NavigatingCancelEventArgs"/> class, setting all initial property values.
        /// </summary>
        /// <param name="mode">A value that indicates the type of navigation that is occurring.</param>
        public NavigatingCancelEventArgs(NavigationMode mode, bool isCancelable, bool isNavigationInitiator)
        {
            NavigationMode = mode;
            IsCancelable = isCancelable;
            IsNavigationInitiator = IsNavigationInitiator;
        }
    }
}