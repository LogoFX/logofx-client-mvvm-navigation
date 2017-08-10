namespace LogoFX.Client.Mvvm.Navigation
{
    public sealed partial class NavigationService
    {
        private abstract class RootableNavigationBuilder<T> : NavigationBuilder<T>, IRootableNavigationBuilder<T>
        {
            public void AsRoot()
            {
                IsRoot = true;
                IsSingleton = true;
            }
        }
    }
}