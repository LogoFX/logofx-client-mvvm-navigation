namespace LogoFX.Client.Mvvm.Navigation
{
    /// <summary>
    /// Navigation view model which allows custom handling of the navigation event
    /// </summary>
    public interface INavigationViewModel
    {
        /// <summary>
        /// Called when the view model is navigated to or navigated from.
        /// </summary>
        /// <param name="mode">The navigation mode.</param>
        /// <param name="argument">The navigation argument.</param>
        void OnNavigated(NavigationMode mode, object argument);
    }
}