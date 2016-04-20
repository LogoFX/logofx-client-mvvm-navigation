using System;

namespace LogoFX.Client.Mvvm.Navigation
{
    /// <summary>
    /// Provides data for the <see cref="E:System.Windows.Navigation.NavigationService.NavigationFailed"/> event of the <see cref="T:System.Windows.Navigation.NavigationService"/> class and the <see cref="E:System.Windows.Controls.Frame.NavigationFailed"/> event of the <see cref="T:System.Windows.Controls.Frame"/> class.
    /// </summary>
    public sealed class NavigationFailedEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the error from the failed navigation.
        /// </summary>
        /// 
        /// <returns>
        /// A value that represents the error.
        /// </returns>
        public Exception Exception { get; private set; }

        /// <summary>
        /// Gets or sets a value that indicates whether the failure event has been handled.
        /// </summary>
        /// 
        /// <returns>
        /// true if the event has been handled; otherwise, false.
        /// </returns>
        public bool Handled { get; set; }

        private NavigationFailedEventArgs()
        {
            
        }
    }
}