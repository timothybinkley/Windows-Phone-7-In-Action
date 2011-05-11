using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Windows;
using Microsoft.Phone.Controls;

namespace Pivot
{
    public partial class MainPage : PhoneApplicationPage
    {
        IEnumerable<SampleData> data;

        public MainPage()
        {
            InitializeComponent();
            data = SampleData.GenerateSampleData();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            bool loadAllData = false;
            IsolatedStorageSettings.ApplicationSettings.TryGetValue("loadAllData", out loadAllData);
            allDataOption.IsChecked = loadAllData;
            asNeededOption.IsChecked = !loadAllData;
        }

        protected override void OnNavigatedFrom(System.Windows.Navigation.NavigationEventArgs e)
        {
            IsolatedStorageSettings.ApplicationSettings["loadAllData"] = allDataOption.IsChecked.Value;
            State["selection"] = pivot.SelectedIndex;
            base.OnNavigatedFrom(e);
        }

        private void pivot_Loaded(object sender, RoutedEventArgs e)
        {
            if(State.ContainsKey("selection"))
            {
                pivot.SelectedIndex = (int)State["selection"];
            }

            if (allDataOption.IsChecked.Value)
            {
                allDataList.ItemsSource = data;
                filteredDataList.ItemsSource = data.Where(item => item.Category == SampleCategory.Even);
            }
        }

        private void pivot_LoadedPivotItem(object sender, PivotItemEventArgs e)
        {
            if (e.Item == allDataItem && allDataList.ItemsSource == null)
            {
                allDataList.ItemsSource = data;
            }
            else if (e.Item == filteredDataItem && filteredDataList.ItemsSource == null)
            {
                filteredDataList.ItemsSource = data.Where(item => item.Category == SampleCategory.Even);
            }
        }

        private void pivot_UnloadedPivotItem(object sender, PivotItemEventArgs e)
        {
            if (!allDataOption.IsChecked.Value)
            {
                if (e.Item == allDataItem)
                {
                    allDataList.ItemsSource = null;
                }
                else if (e.Item == filteredDataItem)
                {
                    filteredDataList.ItemsSource = null;
                }
            }
        }

    }
}