using System;

namespace LogoFX.Client.Mvvm.Navigation
{
    public sealed class NavigationEventArgs : INavigationEventArgs
    {
        /// <summary>
        /// Gets the root node of the target page's content.
        /// </summary>
        /// 
        /// <returns>
        /// The root node of the target page's content.
        /// </returns>
        public object Content { get; set; }
        /// <summary>
        /// Gets a value that indicates the direction of movement during navigation
        /// </summary>
        /// 
        /// <returns>
        /// A value of the enumeration.
        /// </returns>
        public NavigationMode NavigationMode { get; set; }
        /// <summary>
        /// Gets any Parameter object passed to the target page for the navigation.
        /// </summary>
        /// 
        /// <returns>
        /// An object that potentially passes parameters to the navigation target. May be null.
        /// </returns>
        public object Parameter { get; set; }
        /// <summary>
        /// Gets the data type of the source page.
        /// </summary>
        /// 
        /// <returns>
        /// The data type of the source page, represented as namespace.type or simply type.
        /// </returns>
        public Type SourcePageType { get; set; }
    }
}