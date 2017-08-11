namespace LogoFX.Client.Mvvm.Navigation
{
    public sealed partial class NavigationService
    {
        private sealed class NavigationParameter<T> : NavigationParameter
        {
            private readonly INavigationService _service;
            private readonly object _argument;

            public NavigationParameter(INavigationService service, object argument)
            {
                _service = service;
                _argument = argument;
            }

            public override void Navigate()
            {
                _service.Navigate(typeof(T), _argument);
            }
        }
    }
}