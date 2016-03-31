using System;
#if NET45
using System.Windows;
using System.Windows.Interactivity;
#endif
#if NETFX_CORE || WINDOWS_UWP
using Windows.UI.Xaml;
#endif

namespace LogoFX.Client.Mvvm.Navigation
{
    /// <summary>
    /// Navigate action.
    /// </summary>
    public sealed class NavigateAction : TriggerAction
#if NET45
        <UIElement>
#endif
    {
#region Fields

        private static readonly Type Type = typeof(NavigateAction);

#endregion

#region Dependency Properties

        /// <summary>
        /// Navigation parameter.
        /// </summary>
        public static readonly DependencyProperty ParameterProperty =
            DependencyProperty.Register(
                "Parameter",
                typeof(NavigationParameter),
                Type,
                new PropertyMetadata(null));

        /// <summary>
        /// Navigation target type.
        /// </summary>
        public static readonly DependencyProperty ItemTypeProperty =
            DependencyProperty.Register(
                "ItemType",
                typeof(Type),
                Type,
                new PropertyMetadata(null));

        /// <summary>
        /// Navigation argument.
        /// </summary>
        public static readonly DependencyProperty ArgumentProperty =
            DependencyProperty.Register(
                "Argument",
                typeof(object),
                Type,
                new PropertyMetadata(null));

        /// <summary>
        /// Navigation service.
        /// </summary>
        public static readonly DependencyProperty NavigationServiceProperty =
            DependencyProperty.Register(
                "NavigationService",
                typeof (INavigationService),
                Type,
                new PropertyMetadata(null));

#endregion

#region Public Properties

        /// <summary>
        /// Navigation parameter.
        /// </summary>
        public NavigationParameter Parameter
        {
            get { return (NavigationParameter)GetValue(ParameterProperty); }
            set { SetValue(ParameterProperty, value); }
        }

        /// <summary>
        /// Navigation target type.
        /// </summary>
        public Type ItemType
        {
            get { return (Type)GetValue(ItemTypeProperty); }
            set { SetValue(ItemTypeProperty, value); }
        }

        /// <summary>
        /// Gets or sets the navigation argument.
        /// </summary>
        /// <value>
        /// The argument.
        /// </value>
        public object Argument
        {
            get { return GetValue(ArgumentProperty); }
            set { SetValue(ArgumentProperty, value); }
        }

        /// <summary>
        /// Gets or sets the navigation service.
        /// </summary>
        /// <value>
        /// The navigation service.
        /// </value>
        public INavigationService NavigationService
        {
            get { return (INavigationService)GetValue(NavigationServiceProperty); }
            set { SetValue(NavigationServiceProperty, value); }
        }

#endregion
        
        /// <summary>
        /// Invokes the action.
        /// </summary>
        /// <param name="parameter">The parameter to the action. If the action does not require a parameter, the parameter may be set to a null reference.</param>
        protected override void Invoke(object parameter)
        {          
            if (NavigationService == null)
            {
                throw new NavigationServiceNullException("Navigation service must be set");                
            }

            if (Parameter != null)
            {
                Parameter.Navigate();
            }
            else if (ItemType != null)
            {
                NavigationService.Navigate(ItemType, Argument);
            }
            else
            {                
                throw new ArgumentException("You must set Parameter or ItemType.");                
            }
        }
    }
}