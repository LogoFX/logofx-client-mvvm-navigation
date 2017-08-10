using System;

namespace LogoFX.Client.Mvvm.Navigation
{
    public interface INavigationStackEntry
    {
        object Parameter { get; }
        Type Type { get; }
        object Content { get; }
    }
}