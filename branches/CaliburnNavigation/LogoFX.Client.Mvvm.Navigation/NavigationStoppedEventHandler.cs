using System.Runtime.InteropServices;

namespace LogoFX.Client.Mvvm.Navigation
{
    /// <summary>
    /// Provides event data for the NavigationStopped event.
    /// </summary>
    /// <param name="sender">The object where the handler is attached.</param><param name="e">Event data for the event.</param>
    public delegate void NavigationStoppedEventHandler([In] object sender, [In] NavigationEventArgs e);
}