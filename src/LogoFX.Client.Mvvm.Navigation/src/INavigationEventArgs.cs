using System;

namespace LogoFX.Client.Mvvm.Navigation
{
    internal interface INavigationEventArgs
    {
        object Content { get; }
        NavigationMode NavigationMode { get; }
        object Parameter { get; }
        Type SourcePageType { get; }
    }
}