using Rg.Plugins.Popup.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TaxiApp.Views.PopupPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsPopup : PopupPage
    {
        public EventHandler OnTableViewModeChange { get; set; }
        public bool TableViewMode
        {
            get
            {
                return Preferences.Get("TableViewMode", true);
            }
            set
            {
                Preferences.Set("TableViewMode", value);
                OnTableViewModeChange?.Invoke(Switch,null);
            }
        }
        public SettingsPopup()
        {
            InitializeComponent();
            BindingContext = this;
        }
    }
}