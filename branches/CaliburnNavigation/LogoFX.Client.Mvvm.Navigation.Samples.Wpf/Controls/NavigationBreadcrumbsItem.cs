using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace LogoFX.Client.Mvvm.Navigation.Samples.Wpf.Controls
{
    [TemplatePart(Name = "PART_Button", Type = typeof(Button))]
    [TemplatePart(Name = "PART_Separator", Type = typeof(Separator))]
    public class NavigationBreadcrumbsItem : Control
    {
        public static readonly DependencyProperty ButtonStyleProperty =
            DependencyProperty.Register(
                "ButtonStyle",
                typeof(Style),
                typeof(NavigationBreadcrumbsItem),
                new PropertyMetadata(null));

        public Style ButtonStyle
        {
            get { return (Style)GetValue(ButtonStyleProperty); }
            set { SetValue(ButtonStyleProperty, value); }
        }

        public static readonly DependencyProperty SeparatorStyleProperty =
            DependencyProperty.Register(
                "SeparatorStyle",
                typeof(Style),
                typeof(NavigationBreadcrumbsItem),
                new PropertyMetadata(null));

        public Style SeparatorStyle
        {
            get { return (Style)GetValue(SeparatorStyleProperty); }
            set { SetValue(SeparatorStyleProperty, value); }
        }

        public static readonly RoutedEvent ClickEvent =
            ButtonBase.ClickEvent.AddOwner(typeof(NavigationBreadcrumbsItem));

        public event RoutedEventHandler Click
        {
            add { AddHandler(ClickEvent, value); }
            remove { RemoveHandler(ClickEvent, value); }
        }

        private void RaiseClick(object source)
        {
            RoutedEventArgs eventArgs = new RoutedEventArgs(ClickEvent, source);
            RaiseEvent(eventArgs);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            var button = GetTemplateChild("PART_Button") as Button;
            if (button != null)
            {
                button.Click += (s, e) =>
                {
                    RaiseClick(s);
                };
            }
        }
    }
}