using Solid.Practices.IoC;

namespace LogoFX.Client.Mvvm.Navigation
{
    public sealed partial class NavigationService
    {
        private sealed class GenericBuilder<T> : RootableNavigationBuilder<T> where T : class
        {
            private readonly IDependencyResolver _resolver;

            public GenericBuilder(IDependencyResolver resolver)
            {
                _resolver = resolver;
            }

            protected override T GetValueInternal()
            {
                return _resolver.Resolve<T>();
            }
        }
    }
}