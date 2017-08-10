using System.Windows;
using System.Windows.Controls;

namespace LogoFX.Client.Mvvm.Navigation.Samples.Wpf.Controls
{
    public class NavigationBreadcrumbs : ItemsControl
    {
        static NavigationBreadcrumbs()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(NavigationBreadcrumbs), new FrameworkPropertyMetadata(typeof(NavigationBreadcrumbs)));
        }

        public static readonly RoutedEvent NavigationClickEvent =
            EventManager.RegisterRoutedEvent(
                "NavigationClick",
                RoutingStrategy.Bubble,
                typeof(RoutedEventHandler),
                typeof(NavigationBreadcrumbs));

        public event RoutedEventHandler NavigationClick
        {
            add { AddHandler(NavigationClickEvent, value); }
            remove { RemoveHandler(NavigationClickEvent, value); }
        }

        private void RaiseNavigationClick(object navigationItem)
        {
            NavigationClickEventArgs eventArgs = new NavigationClickEventArgs(NavigationClickEvent, navigationItem);
            RaiseEvent(eventArgs);
        }

        public static readonly DependencyProperty CurrentValueProperty =
            DependencyProperty.Register(
                "CurrentValue",
                typeof(object),
                typeof(NavigationBreadcrumbs),
                new PropertyMetadata(null));

        public object CurrentValue
        {
            get { return GetValue(CurrentValueProperty); }
            set { SetValue(CurrentValueProperty, value); }
        }

        public static readonly DependencyProperty CurrentItemTemplateProperty =
            DependencyProperty.Register(
                "CurrentItemTemplate",
                typeof(DataTemplate),
                typeof(NavigationBreadcrumbs),
                new PropertyMetadata(null));

        public DataTemplate CurrentItemTemplate
        {
            get { return (DataTemplate)GetValue(CurrentItemTemplateProperty); }
            set { SetValue(CurrentItemTemplateProperty, value); }
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            return new NavigationBreadcrumbsItem();
        }

        protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
        {
            base.PrepareContainerForItemOverride(element, item);

            NavigationBreadcrumbsItem nbi;
            if ((nbi = element as NavigationBreadcrumbsItem) != null)
            {
                nbi.Click += (s, e) =>
                {
                    RaiseNavigationClick(item);
                };
            }
        }
    }
}
