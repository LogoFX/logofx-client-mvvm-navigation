﻿using Caliburn.Micro;

namespace LogoFX.Client.Mvvm.Navigation.Samples.Uwp.ViewModels
{
    [NavigationViewModel(ConductorType=typeof(MainViewModel))]
    public sealed class Page1ViewModel : Screen
    {
        public override string DisplayName
        {
            get { return "Page1"; }
            set { }
        }
    }
}