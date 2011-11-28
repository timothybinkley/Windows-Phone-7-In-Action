using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Wp7OData.ViewModels;
using System.Windows.Navigation;

namespace Wp7OData {
    public partial class EditableDetailsPage : PhoneApplicationPage {
        public EditableDetailsPage() {
            InitializeComponent();
        }

        // When page is navigated to set data context to selected item in list
        protected override void OnNavigatedTo(NavigationEventArgs e) {
            string selectedIndex = "";
            if (NavigationContext.QueryString.TryGetValue("selectedItem", out selectedIndex)) {
                int index = int.Parse(selectedIndex);
                DataContext = App.ViewModel.Items[index];
            }
            else {
                DataContext = new CategoryModel();
            }
        }
    }
}