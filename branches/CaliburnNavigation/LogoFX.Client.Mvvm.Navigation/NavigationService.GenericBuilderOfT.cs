using Solid.Practices.IoC;

namespace LogoFX.Client.Mvvm.Navigation
{
    public sealed partial class NavigationService
    {
        private sealed class GenericBuilder<T> : RootableNavigationBuilder<T> where T : class
        {
            private readonly IIocContainer _container;

            public GenericBuilder(IIocContainer container)
            {
                _container = container;
            }

            protected override T GetValueInternal()
            {
                return _container.Resolve<T>();
            }
        }
    }
}