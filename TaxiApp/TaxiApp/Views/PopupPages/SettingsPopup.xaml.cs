using Rg.Plugins.Popup.Pages;
using System;
using Xamarin.Essentials;
using Xamarin.Forms.Xaml;

namespace TaxiApp.Views.PopupPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsPopup : PopupPage
    {
        public EventHandler OnTableViewModeChange { get; set; }
        public bool TableViewMode
        {
            get => Preferences.Get("TableViewMode", true);
            set
            {
                Preferences.Set("TableViewMode", value);
                OnTableViewModeChange?.Invoke(Switch, null);
            }
        }
        public SettingsPopup()
        {
            InitializeComponent();
            BindingContext = this;
        }
    }
}