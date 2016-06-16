using System;

namespace LogoFX.Client.Mvvm.Navigation
{
    internal sealed class NavigationStackEntry : INavigationStackEntry
    {
        public object Parameter { get; private set; }
        public Type Type { get; private set; }

        public NavigationStackEntry(Type type, object parameter)
        {
            Type = type;
            Parameter = parameter;
        }
    }
}