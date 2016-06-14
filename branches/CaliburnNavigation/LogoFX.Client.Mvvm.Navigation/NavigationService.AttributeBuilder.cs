using System;
using Solid.Practices.IoC;

namespace LogoFX.Client.Mvvm.Navigation
{
    public sealed partial class NavigationService
    {
        private sealed class AttributeBuilder : NavigationBuilder
        {
            private readonly Type _vmType;
            private readonly IIocContainerResolver _resolver;

            public AttributeBuilder(Type vmType, NavigationViewModelAttribute attr, IIocContainerResolver resolver)
            {
                _vmType = vmType;
                _resolver = resolver;

                IsSingleton = attr.IsSingleton;
                ConductorType = attr.ConductorType;
                NotRemember = attr.NotRemember;
            }

            protected override object GetValue()
            {
                return _resolver.Resolve(_vmType);
            }
        }
    }
}