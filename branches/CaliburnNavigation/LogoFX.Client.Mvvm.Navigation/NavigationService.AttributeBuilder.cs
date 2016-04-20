using System;
using Solid.Practices.IoC;

namespace LogoFX.Client.Mvvm.Navigation
{
    public sealed partial class NavigationService
    {
        private sealed class AttributeBuilder : NavigationBuilder
        {
            private readonly Type _vmType;
            private readonly IIocContainer _container;

            public AttributeBuilder(Type vmType, NavigationViewModelAttribute attr, IIocContainer container)
            {
                _vmType = vmType;
                _container = container;

                IsSingleton = attr.IsSingleton;
                ConductorType = attr.ConductorType;
                NotRemember = attr.NotRemember;
            }

            protected override object GetValue()
            {
                return _container.Resolve(_vmType);
            }
        }
    }
}